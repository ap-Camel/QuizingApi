using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizingApi.Dtos.CustomeDtos;
using QuizingApi.Dtos.ExaminationDtos;
using QuizingApi.Dtos.QuestionDtos;
using QuizingApi.DtosChoosenQuestionAnswersDtos;
using QuizingApi.Helpers;
using QuizingApi.Models;
using QuizingApi.Services.LocalDb.Interfaces;

namespace QuizingApi.Controllers {

    [ApiController]
    [Route("quiz")]
    [Authorize]
    public class QuizController : ControllerBase {

        private readonly IExamData examData;
        private readonly IQuestionData questionData;
        private readonly IAnswerData answerData;
        private readonly IExaminationData examinationData;
        private readonly IChoosenQAData choosenQAData;

        public QuizController(IExamData examData, IQuestionData questionData, 
                                IAnswerData answerData, IExaminationData examinationData, IChoosenQAData choosenQAData) {
            this.examData = examData;
            this.questionData = questionData;
            this.answerData = answerData;
            this.examinationData = examinationData;
            this.choosenQAData = choosenQAData;
        }


        // must add protection against user sending questionID that they didnt recieve for their exam
        // must add protection for if user sends back more qustions with answers than the exam has
        // when evaluating, should also add to the table handling the quiz history


        [HttpGet("{id}")]
        public async Task<ActionResult<QuizSendDto>> getQuizAsync(int id) {

            int userID = JwtHelpers.getGeneralID(HttpContext.Request.Headers["Authorization"]);

            //var exam = await examData.getExamAsync(userID, id);
            var exam = await examData.getExamByIdAsync(id);
            var questions = await questionData.getQuestionsByExamIdAsync(exam.ID);

            if(!questions.Any()) {
                return(NotFound("this exam does not have any questoins"));
            }

            QuizSendDto send = new QuizSendDto();
            List<QuestionMinimumDto> tempList = new List<QuestionMinimumDto>();
            send.examID = exam.ID;
            send.duration = exam.duration;

            var result = randomizeQuestions(questions.ToList(), exam.difficulty, exam.numOfQuestions);

            foreach(QuestionModel q in result) {
                var answers = await answerData.getAnswersByQuestionIdAync(q.ID, userID);
                List<string> randomizedAnswers = randomizeAnswers(answers.ToList());
                QuestionMinimumDto temp = new QuestionMinimumDto {
                    questionID = q.ID,
                    question = q.question,
                    answers = randomizedAnswers
                };
                tempList.Add(temp);
            }

            send.quiz = tempList;

            var insertResult = await examinationData.insertExaminationAsync(new ExaminationInsertDto { examID = exam.ID, userID = userID, result = 0 });
            if(insertResult > 0) {
                send.examinationID = insertResult;
                int[] tempArr = new int[tempList.Count()];
                bool failed = false;
                for(int i = 0; i < tempArr.Count(); i++) {
                    tempArr[i] = await choosenQAData.insertCQA_WithoutAnswerAsync(new ChoosenQAInsertDto { 
                        questionID = tempList[i].questionID,
                        answerID = 0,
                        examinationID = insertResult
                        });
                    if(tempArr[i] <= 0) {
                        failed = true;
                    }
                }

                if(failed) {
                    bool deleteFailed = false;
                    foreach(int i in tempArr) {
                        bool deleteResult = await choosenQAData.deleteCQA_ByIdAsync(i, insertResult);
                        if(!deleteResult) {
                            deleteFailed = false;
                        }
                    }

                    if(deleteFailed) {
                        return BadRequest("failed to create quiz, failed to insert to history, and failed to delete the ones for the cleanup");
                    }

                    return BadRequest("failed to create quiz, failed to insert to history");
                }

                return Ok(send);
            }

            return BadRequest("failed to create examination");
        }


        [HttpPost]
        public async Task<ActionResult<int>> evaluateQuiz(QuizRecieveDto quiz) {

            int userID = JwtHelpers.getGeneralID(HttpContext.Request.Headers["Authorization"]);

            List<int> questionIds = (await choosenQAData.getQuestionIdsFromExaminationAsync(quiz.examinationID, userID)).ToList();
            ExamModel exam = await examData.getExamByIdAsync(quiz.examID);
            ExaminationModel examination = await examinationData.getExaminationByUserIdAsync(quiz.examinationID, userID);

            if(examination.evaluated) {
                return BadRequest("examination has already been evaluated");
            }

            DateTime currentDate = DateTime.Now;
            if((currentDate - examination.atDate).TotalMinutes > exam.duration) {
                return BadRequest("the examination period has expired");
            }

            if(quiz.quiz.Count() > exam.numOfQuestions) {
                return BadRequest("evaluation failed, number of returned question exceeds the exam limit");
            }

            bool extraQuestions = false;
            foreach(QuestionEvaluateDto q in quiz.quiz) {
                if(!questionIds.Contains(q.questionID)) {
                    extraQuestions = true;
                }
            }

            if(extraQuestions) {
                return BadRequest("evaluation failed, a question was recieved that wasnt sent to the user");
            }

            int mark = 0;

            // QuestionModel tempQ = new QuestionModel();
            AnswerModel tempA = new AnswerModel();
            bool updateFailed = false;
            foreach(QuestionEvaluateDto q in quiz.quiz) {
                var answerResult = await answerData.checkAnswerAsync(q.answer, q.questionID);
                var updateCQA_Result = await choosenQAData.updateCQA_AnswerIdAsync(answerResult.ID, q.questionID, quiz.examinationID, answerResult.correct);
                if(answerResult.correct == true) {
                    mark += 1;
                }
                if(!updateCQA_Result) {
                    updateFailed = true;
                }
            }

            string message = "";
            if(updateFailed) {
                message += ", failed to update answer in history";
            }

            var updateResult = await examinationData.updateExaminationResultAsync(mark, quiz.examinationID, userID);

            return updateResult ? Ok(mark) : BadRequest("failed to set examination result" + message);
        }


        private QuestionModel[] randomizeQuestions(List<QuestionModel> list, int difficulty, int num) {

            int count = list.Count() >= num ? num : list.Count();
            QuestionModel[] arr = new QuestionModel[count];

            List<int> randArr = new List<int>();
            for(int i = 0; i < count; i++) {
                randArr.Add(i);
            }

            int veryEasy = 0, easy = 0, normal = 0, hard = 0, veryHard = 0;
            double veryEasyPer = 0.4075, easyPer = 0.3175, normalPer = 0.2375, hardPer = 0.03, veryHarsPer = 0.0025;
            int sum = 0;

            double step = 1.1;
            double difficulty02 = difficulty * step;

            veryHard = (int)Math.Round(( veryHarsPer + (((difficulty02 / 10.00) / 2) * (0.33 + (difficulty - 3) * 0.08))) * num);
            hard = (int)Math.Round(( hardPer + (((difficulty02 / 10.00) / 2) * (0.66 - (difficulty - 3) * 0.08))) * num);
            normal = (int)Math.Round((normalPer - (((difficulty02 / 10.00) / 2) * 0.1)) * num);
            easy = (int)Math.Round((easyPer - (((difficulty02 / 10.00) / 2) * 0.3)) * num);
            veryEasy = (int)Math.Round((veryEasyPer - (((difficulty02 / 10.00) / 2) * 0.6)) * num);

            sum = (veryHard + hard + normal + easy + veryEasy);

            if(sum > num) {
                veryHard -= (sum - num);
            }

            if(sum < num) {
                veryEasy += (num - sum);
            }

            int[] arrNumbers = new int[] {
                veryEasy, easy, normal, hard, veryHard
            };

            
            Random rand = new Random();
            int index = 0;
            int index2 = 0;
            int choosen = 0;
            int limit = 0;
            int actual = list.Count();
            int add = 0;
            sum = (veryHard + hard + normal + easy + veryEasy);

            for(int i = 5; i >= 1; i--) {
                List<QuestionModel> temp = list.Where(question => question.difficulty == i).ToList();
                limit = arrNumbers[i - 1];
                if(limit > temp.Count()) {
                    add = arrNumbers[i - (i != 1 ? 2 : 1)] + (limit - temp.Count());
                    arrNumbers[i - (i != 1 ? 2 : 1)] = add;
                    limit = temp.Count(); 
                }
                if((sum - limit) > (actual - temp.Count())){
                    limit += (sum - limit) - (actual - temp.Count());
                }
                actual -= temp.Count();
                for(int j = 0; j < limit;) {
                    index = rand.Next(0, randArr.Count());
                    index2 = randArr[index];
                    if(arr[index2] == null) {
                        choosen = rand.Next(0, temp.Count());
                        arr[index2] = temp[choosen];
                        temp.RemoveAt(choosen);
                        randArr.RemoveAt(index);
                        sum -= 1;
                        j++;
                    }
                } 
            }

            return arr;
        }

        private List<string> randomizeAnswers(List<AnswerModel> list) {

            Random rand = new Random();

            List<AnswerModel> corrects = list.Where( x => x.correct == true ).ToList();
            List<AnswerModel> wrongs = list.Where(x => x.correct == false).ToList();

            AnswerModel correctAnswer;
            if(corrects.Count() == 0) {
                correctAnswer = new AnswerModel {answer = ""};
            }

            int index = rand.Next(0, corrects.Count());
            correctAnswer = corrects[index];

            int count = correctAnswer.answer != "" ? wrongs.Count() + 1 : wrongs.Count();
            if(count > 4) {
                count = 4;
            }

            AnswerModel[] arr = new AnswerModel[count];
            int num = rand.Next(count);
            arr[num] = correctAnswer.answer != "" ? correctAnswer : null;

            for(int i = 0; i < count;) {

                if(arr[i] == correctAnswer) {
                    i++;
                    continue;
                }

                num = rand.Next(wrongs.Count());
                if(arr[i] is null) {
                    arr[i] = wrongs[num];
                    wrongs.RemoveAt(num);
                    i++;

                }
            }

            List<string> newList = new List<string>();
            foreach(AnswerModel answer in arr) {
                newList.Add(answer.answer);
            }

             return newList;
        }

    }
}
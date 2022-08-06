using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizingApi.Dtos.CustomeDtos;
using QuizingApi.Dtos.QuestionDtos;
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

        public QuizController(IExamData examData, IQuestionData questionData, IAnswerData answerData) {
            this.examData = examData;
            this.questionData = questionData;
            this.answerData = answerData;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> getQuizAsync(int id) {

            int userID = JwtHelpers.getGeneralID(HttpContext.Request.Headers["Authorization"]);

            var exam = await examData.getExamAsync(userID, id);
            var questions = await questionData.getQuestionsByExamIdAsync(exam.ID);

            QuizSendDto send = new QuizSendDto();
            List<QuestionMinimumDto> tempList = new List<QuestionMinimumDto>();
            send.examID = exam.ID;

            var result = randomizeQuestions(questions.ToList(), exam.difficulty, exam.numOfQuestions);

            foreach(QuestionModel q in result) {
                var answers = await answerData.getAnswersByQuestionIdAync(q.ID, userID);
                List<string> randomizedAnswers = randomizeAnswers(answers.ToList());
                QuestionMinimumDto temp = new QuestionMinimumDto {
                    question = q.question,
                    answers = randomizedAnswers
                };
                tempList.Add(temp);
            }

            send.quiz = tempList;
            
            return Ok(send);
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
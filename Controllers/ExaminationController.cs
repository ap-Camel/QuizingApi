using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizingApi.Dtos.ChoosenQuestionAnswersDtos;
using QuizingApi.Dtos.ExaminationDtos;
using QuizingApi.Helpers;
using QuizingApi.Models;
using QuizingApi.Services.LocalDb.Interfaces;

namespace QuizingApi.Controllers {

    [ApiController]
    [Route("examination")]
    [Authorize]
    public class ExaminationController : ControllerBase {

        private readonly IExaminationData examinationData;
        private readonly IChoosenQAData choosenQAData;
        private readonly IExamData examData;
        private readonly IQuestionData questionData;
        private readonly IAnswerData answerData;

        public ExaminationController(
            IExaminationData examinationData, IChoosenQAData choosenQAData, IExamData examData,
            IQuestionData questionData, IAnswerData answerData
        ) {
            this.examinationData = examinationData;
            this.choosenQAData = choosenQAData;
            this.examData = examData;
            this.questionData = questionData;
            this.answerData = answerData;
        }

        [HttpGet]
        public async Task<ActionResult<List<ExaminationEssentialsDto>>> getExaminationsAsync() {

            int userID = JwtHelpers.getGeneralID(HttpContext.Request.Headers["Authorization"]);

            List<ExaminationModel> examinations = (await examinationData.getExaminationsByUserIdAsync(userID)).ToList();
            List<ExaminationEssentialsDto> list = new List<ExaminationEssentialsDto>();
            ExamModel tempE;
            foreach(ExaminationModel e in examinations) {
                tempE = await examData.getExamByIdAsync(e.examID);
                list.Add(new ExaminationEssentialsDto {
                    ID = e.ID,
                    examID = e.examID,
                    examTitle = tempE.title,
                    atDate = e.atDate,
                    result = e.result
                });
            }

            return Ok(list);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ExaminationEssentialsDto>> getExaminationInfoAsync(int id) {

            int userID = JwtHelpers.getGeneralID(HttpContext.Request.Headers["Authorization"]);

            var CQAs = await choosenQAData.getCQAbyExaminationIdAsync(id);
            CQAs = CQAs.Where( cqa => cqa.answerID != 0);
            List<ChoosenQAEssentials> list = new List<ChoosenQAEssentials>();
            AnswerModel tempA;
            QuestionModel tempQ;
            foreach(ChooseQuestionsAnswersModel q in CQAs) {
                tempA = await answerData.getAnswerByIdAsync(q.answerID);
                tempQ = await questionData.getQuestionByIdAsync(q.questionID);
                list.Add(new ChoosenQAEssentials {
                    question = tempQ.question,
                    questionID = tempQ.ID,
                    answer = tempA.answer,
                    answerID = tempA.ID,
                    correct = tempA.correct,
                    examinationID = q.examinationID
                });
            }

            return Ok(list);
        }
    }
}
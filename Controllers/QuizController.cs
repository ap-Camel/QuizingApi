using Microsoft.AspNetCore.Mvc;
using QuizingApi.Dtos.CustomeDtos;
using QuizingApi.Helpers;
using QuizingApi.Models;
using QuizingApi.Services.LocalDb.Interfaces;

namespace QuizingApi.Controllers {

    [ApiController]
    [Route("quiz")]
    public class QuizController : ControllerBase {

        private readonly IExamData examData;
        private readonly IQuestionData questionData;

        public QuizController(IExamData examData, IQuestionData questionData) {
            this.examData = examData;
            this.questionData = questionData;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuizSendDto>> getQuizAsync(int id) {

            int userID = JwtHelpers.getGeneralID(HttpContext.Request.Headers["Authorization"]);

            var exam = await examData.getExamAsync(userID, id);
            var questions = await questionData.getQuestionsByExamIdAsync(exam.ID);
            



            
        }



        private List<QuestionModel> randomizeQuestions(List<QuestionModel> list, int difficulty, int num) {

            double veryEasy = 0, easy = 0, normal = 0, hard = 0, veryHard = 0;
            double veryEasyPer = 0.4075, easyPer = 0.3175, normalPer = 0.2375, hardPer = 0.03, veryHarsPer = 0.0025;
            double sum = 0;

            double step = 1.1;
            double difficulty02 = difficulty * step;

            veryHard = Math.Round(( veryHarsPer + (((difficulty02 / 10.00) / 2) * (0.33 + (difficulty - 3) * 0.08))) * num);
            hard = Math.Round(( hardPer + (((difficulty02 / 10.00) / 2) * (0.66 - (difficulty - 3) * 0.08))) * num);
            normal = Math.Round((normalPer - (((difficulty02 / 10.00) / 2) * 0.1)) * num);
            easy = Math.Round((easyPer - (((difficulty02 / 10.00) / 2) * 0.3)) * num);
            veryEasy = Math.Round((veryEasyPer - (((difficulty02 / 10.00) / 2) * 0.6)) * num);

            if(sum > num) {
                veryHard -= (sum - num);
            }

            if(sum < num) {
                veryEasy += (num - sum);
            }
            
            

            for(int i = 0; i < 5; i++) {
                foreach(QuestionModel q in list) {

                }
            }





            return new List<QuestionModel>();
        }

    }
}
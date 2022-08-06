using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizingApi.Dtos.AnswerDtos;
using QuizingApi.Dtos.QuestionDtos;
using QuizingApi.Helpers;
using QuizingApi.Models;
using QuizingApi.Services.LocalDb.Interfaces;

namespace QuizingApi.Controllers {

    [ApiController]
    [Route("question")]
    [Authorize]
    public class QuestionController : ControllerBase {
        private readonly IQuestionData questionData;
        private readonly IExamData examData;
        private readonly IAnswerData answerData;

        public QuestionController(IQuestionData questionData, IExamData examData, IAnswerData answerData) {
            this.questionData = questionData;
            this.examData = examData;
            this.answerData = answerData;
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionEssentialsDto>> getQuestionAsync(int id) {
            
            int userID = JwtHelpers.getGeneralID(HttpContext.Request.Headers["Authorization"]);

            var result = await questionData.getQuestionAsync(id, userID);

            return result is not null ? Ok(result) : NotFound("no question was found");
        }


        [HttpGet]
        public async Task<ActionResult<List<QuestionEssentialsDto>>> getQuestionsAsync() {

            int userID = JwtHelpers.getGeneralID(HttpContext.Request.Headers["Authorization"]);

            var result = await questionData.getQuestionsByUserIdAsync(userID);

            if(result.Count() == 0) {
                return NotFound("no questions were found");
            }

            List<QuestionEssentialsDto> temp = new List<QuestionEssentialsDto>();
            foreach(QuestionModel q in result) {
                temp.Add(Helpers.Converting.toQuestionEssentials(q));
            }

            return Ok(temp);
        }


        [HttpPost]
        public async Task<ActionResult> insertQuestionAsync(QuestionInsertDto question) {

            int userID = JwtHelpers.getGeneralID(HttpContext.Request.Headers["Authorization"]);

            var verifyOwner = await examData.verifyExamOwnerAsync(userID, question.examID);

            if(verifyOwner is null) {
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            var result = await questionData.insertQuestionAsync(question);

            return result > 0 ? CreatedAtAction(nameof(getQuestionAsync), new {id = result}, question) : BadRequest("question was not created");
        }


        [HttpPut]
        public async Task<ActionResult<bool>> updateQuestionAsync(QuestionUpdateDto question) {

            int userID = JwtHelpers.getGeneralID(HttpContext.Request.Headers["Authorization"]);

            var verifyQuestion = await questionData.verifyQuestionOwnerAsync(userID, question.ID);

            if(verifyQuestion is null) {
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            var result = await questionData.updateQuestionAsync(question);

            return result ? StatusCode(StatusCodes.Status204NoContent) : BadRequest("failed to updated question");
        }


        [HttpPost("/question/withAnswers")]
        public async Task<ActionResult<bool>> insertQuestionWithAnswersAsync(QuestionInsertWithAnswerDto question) {

            int userID = JwtHelpers.getGeneralID(HttpContext.Request.Headers["Authorization"]);

            var verifyOwner = await examData.verifyExamOwnerAsync(userID, question.question.examID);

            if(verifyOwner is null) {
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            var result = await questionData.insertQuestionAsync(question.question);

            if(result > 0) {
                AnswerInsertWithQuestionDto temp;
                int[] answerIds = new int[question.answers.Count()];
                for(int i = 0; i < question.answers.Count(); i++) {
                    temp = question.answers[i];
                    answerIds[i] = await answerData.insertAnswerAsync(new AnswerInsertDto { 
                        answer = temp.answer,
                        correct = temp.correct,
                        active = temp.active,
                        hasImage = temp.hasImage,
                        imgUrl = temp.imgUrl,
                        questionID = result
                     });
                }

                bool failed = false;
                foreach(int i in answerIds) {
                    if(i <= 0) {
                        failed = true;
                    }
                }

                if(failed) {
                    bool deleteResult = await answerData.deleteAnswerByQuestionIdAsync(result);
                    return deleteResult ? NoContent() : BadRequest("answers were not inserted properly and were not deleted properly for the cleanup");
                }

                return CreatedAtAction(nameof(getQuestionAsync), new { id = result }, question);
            }

            return BadRequest("were not able to insert question with answers");
        }


        // [HttpDelete("{id}")]
        // public async Task<ActionResult<bool>> deleteQuestionAsync(int id){

        //     int userID = JwtHelpers.getGeneralID(HttpContext.Request.Headers["Authorization"]);

        //     var verifyQuestion = await questionData.verifyQuestionOwnerAsync(userID, id);

        //     if(verifyQuestion is null) {
        //         return StatusCode(StatusCodes.Status403Forbidden);
        //     }

        //     var result = await questionData.deleteQuestionAsync(id, verifyQuestion.examID);

        //     return result ? StatusCode(StatusCodes.Status204NoContent) : BadRequest("failed to updated question");
        // }

    }
}
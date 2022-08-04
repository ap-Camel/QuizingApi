using Microsoft.AspNetCore.Mvc;
using QuizingApi.Dtos.QuestionDtos;
using QuizingApi.Helpers;
using QuizingApi.Models;
using QuizingApi.Services.LocalDb.Interfaces;

namespace QuizingApi.Controllers {

    [ApiController]
    [Route("question")]
    public class QuestionController : ControllerBase {
        private readonly IQuestionData questionData;
        private readonly IExamData examData;

        public QuestionController(IQuestionData questionData, IExamData examData) {
            this.questionData = questionData;
            this.examData = examData;
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


        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> deleteQuestionAsync(int id){

            int userID = JwtHelpers.getGeneralID(HttpContext.Request.Headers["Authorization"]);

            var verifyQuestion = await questionData.verifyQuestionOwnerAsync(userID, id);

            if(verifyQuestion is null) {
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            var result = await questionData.deleteQuestionAsync(id, verifyQuestion.examID);

            return result ? StatusCode(StatusCodes.Status204NoContent) : BadRequest("failed to updated question");
        }

    }
}
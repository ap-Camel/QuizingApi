using Microsoft.AspNetCore.Mvc;
using QuizingApi.Dtos.AnswerDtos;
using QuizingApi.Helpers;
using QuizingApi.Models;
using QuizingApi.Services.LocalDb.Interfaces;

namespace QuizingApi.Controllers {

    [ApiController]
    [Route("answer")]
    public class AnswerController : ControllerBase {
        private readonly IAnswerData answerData;
        private readonly IQuestionData questionData;

        public AnswerController(IAnswerData answerData, IQuestionData questionData) {
            this.answerData = answerData;
            this.questionData = questionData;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<AnswerEssentialsDto>> getAnswerAsync(int id){
            
            int userID = JwtHelpers.getGeneralID(HttpContext.Request.Headers["Authorization"]);

            var result = await answerData.getAnswerByUserIdAsync(id, userID);

            return result is not null ? Ok(result) : NotFound("no answer was found");
        }


        [HttpGet]
        public async Task<ActionResult<List<AnswerEssentialsDto>>> getAnswersAsync() {

            int userID = JwtHelpers.getGeneralID(HttpContext.Request.Headers["Authorization"]);

            var result = await answerData.getAnswersByUserIdAsync(userID);

            if(result.Count() == 0) {
                return NotFound("no answers were found");
            }

            List<AnswerEssentialsDto> temp = new List<AnswerEssentialsDto>();
            foreach(AnswerModel a in result) {
                temp.Add(Converting.toAnswerEssentials(a));
            }

            return Ok(temp);
        }


        [HttpGet("/question/{questionId}")]
        public async Task<ActionResult<List<AnswerEssentialsDto>>> getAnswersByQuestoinIdAsync(int questionId) {
            int userID = JwtHelpers.getGeneralID(HttpContext.Request.Headers["Authorization"]);

            var result = await answerData.getAnswersByQuestionIdAync(questionId);

            if(result.Count() == 0) {
                return NotFound("no answers were found");
            }

            List<AnswerEssentialsDto> temp = new List<AnswerEssentialsDto>();
            foreach(AnswerModel a in result) {
                temp.Add(Converting.toAnswerEssentials(a));
            }

            return Ok(temp);
        }


        [HttpPost]
        public async Task<ActionResult> insertAnswerAsync(AnswerInsertDto answer) {

            int userID = JwtHelpers.getGeneralID(HttpContext.Request.Headers["Authorization"]);

            var verifyOwner = await questionData.verifyQuestionOwnerAsync(userID, answer.questionID);

            if(verifyOwner is null) {
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            var result = await answerData.insertAnswerAsync(answer);

            return result > 0 ? CreatedAtAction(nameof(getAnswerAsync), new {id = result}, answer) : BadRequest("answer was not added");
        }

        [HttpPut]
        public async Task<ActionResult<bool>> updateAnswerAsync(AnswerUpdateDto answer) {

            int userID = JwtHelpers.getGeneralID(HttpContext.Request.Headers["Authorization"]);

            var verifyOwner = await questionData.verifyQuestionOwnerAsync(userID, answer.questionID);

            if(verifyOwner is null) {
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            var result = await answerData.updateAnswerAsync(answer);

            return result ? StatusCode(StatusCodes.Status204NoContent) : BadRequest("answer was not updated");
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> deleteAnswerAsync(int id) {

            int userID = JwtHelpers.getGeneralID(HttpContext.Request.Headers["Authorization"]);

            var verifyOwner = await answerData.getAnswerByUserIdAsync(id, userID);

            if(verifyOwner is null) {
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            var result = await answerData.deleteAnswerAsync(id, verifyOwner.questionID);

            return result ? StatusCode(StatusCodes.Status204NoContent) : BadRequest("answer was not deleted");
        }
     }
}
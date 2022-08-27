using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizingApi.Dtos.ExamDtos;
using QuizingApi.Helpers;
using QuizingApi.Models;
using QuizingApi.Services.LocalDb.Interfaces;

namespace QuizingApi.Controllers {

    [ApiController]
    [Route("exam")]
    [Authorize]
    public class ExamController : ControllerBase {
        private readonly IExamData examData;
        private readonly IQuestionData questionData;
        private readonly IAnswerData answerData;
        private readonly IWebUserData webUserData;

        public ExamController(IExamData examData, IQuestionData questionData, IAnswerData answerData, IWebUserData webUserData){
            this.examData = examData;
            this.questionData = questionData;
            this.answerData = answerData;
            this.webUserData = webUserData;
        }

        
        [HttpGet("/exam/search/{title}")]
        public async Task<ActionResult<List<ExamSearchReturnDto>>> searchExam(string title) {

            IQueryable<ExamSearchReturnDto> query = await examData.getAllExamsAsync();

            if(!string.IsNullOrEmpty(title)) {
                query = query.Where( e => e.title.ToLower().Contains(title));
            }

            return Ok(query);
        }


        [HttpGet("/exam/topExams/{number}")]
        public async Task<ActionResult<List<ExamSearchReturnDto>>> getTopExamsAsync(int number) {

            var list = await examData.getTopExams(number);

            return Ok(list);
        }


        [HttpGet("/exam/username/{username}")]
        public async Task<ActionResult> getExamsByUsernameAsync(string username) {

            var list = await examData.getExamsByUsername(username);

            return Ok(list);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ExamEssentialsDto>> getExamAsync(int id) {

            int userID = JwtHelpers.getGeneralID(HttpContext.Request.Headers["Authorization"]);

            //var result = await examData.getExamAsync(userID, id);
            var result = await examData.getExamByIdAsync(id);

            if(result is null) {
                return NotFound("no exam was found");
            }

            var user = await webUserData.getWebUserByIdAsync(result.userID);
            ExamEssentialsDto exam = Helpers.Converting.toExamEssentials(result);
            exam.username = user.userName;

            return Ok(exam);
        }

        [HttpGet]
        public async Task<ActionResult<List<ExamEssentialsDto>>> getExamsAsync() {

            int userID = JwtHelpers.getGeneralID(HttpContext.Request.Headers["Authorization"]);

            var result = await examData.getExamsAsync(userID);

            if(result.Count() == 0 ) {
                return NotFound("you dont have any exam");
            }

            List<ExamEssentialsDto> temp = new List<ExamEssentialsDto>();
            foreach(ExamModel e in result) {
                temp.Add(Helpers.Converting.toExamEssentials(e));
            }

            return Ok(temp);
        }


        [HttpPost]
        public async Task<ActionResult<bool>> insertExamAsync(ExamInsertDto exam) {
            
            int userID = JwtHelpers.getGeneralID(HttpContext.Request.Headers["Authorization"]);
            
            int result = await examData.insertExamAsync(exam, userID);

            return result > 0 ? Created(nameof(getExamAsync), exam) : BadRequest("exam was not added");
        }


        [HttpPut]
        public async Task<ActionResult<bool>> updateExamAsync(ExamUpdateDto updateExam) {

            int userID = JwtHelpers.getGeneralID(HttpContext.Request.Headers["Authorization"]);

            bool result = await examData.updateExamAsync(updateExam, userID);

            return result ? StatusCode(StatusCodes.Status204NoContent) : BadRequest("exam info was not updated");
        }


        [HttpDelete("{examID}")]
        public async Task<ActionResult<bool>> deleteExamAsync(int examID) {

            int userID = JwtHelpers.getGeneralID(HttpContext.Request.Headers["Authorization"]);

            var questions = await questionData.getQuestionsByExamIdAsync(examID);

            bool deleted = true;
            foreach(QuestionModel q in questions) {
                var deleteA = await answerData.deleteAnswerByQuestionIdAsync(q.ID);
                if(deleteA == false) {
                    deleted = false;
                }
            }

            if(!deleted) {
                return BadRequest("something wrong happened while deleting the answers, was not able to delete exam");
            }

            var deleteQ = await questionData.deleteQuestionsByExamIdAsync(examID);

            if(!deleteQ) {
                return BadRequest("answers were deleted but something wrong happened while deleting questions, " +
                "exam was not deleted");
            }

            var deleteE = await examData.deleteExamAsync(examID, userID);

            if(!deleteE) {
                return BadRequest("questions and asnwers were deleted, but something wrong happened while deleting exam, was not deleted");
            }

            return StatusCode(StatusCodes.Status204NoContent);
        }


    }
}
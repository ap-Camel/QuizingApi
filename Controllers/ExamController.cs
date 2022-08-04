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

        public ExamController(IExamData examData){
            this.examData = examData;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExamEssentialsDto>> getExamAsync(int id) {

            int userID = JwtHelpers.getGeneralID(HttpContext.Request.Headers["Authorization"]);

            var result = await examData.getExamAsync(userID, id);

            if(result is null) {
                return NotFound("no exam was found");
            }

            return Ok(Helpers.Converting.toExamEssentials(result));
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


        // [HttpDelete]
        // public async Task<ActionResult<bool>> deleteExamAsync(int examID) {

        //     int userID = JwtHelpers.getGeneralID(HttpContext.Request.Headers["Authorization"]);
        // }


    }
}
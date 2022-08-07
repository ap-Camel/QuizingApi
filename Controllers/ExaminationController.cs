using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizingApi.Dtos.ExaminationDtos;
using QuizingApi.Helpers;
using QuizingApi.Services.LocalDb.Interfaces;

namespace QuizingApi.Controllers {

    [ApiController]
    [Route("examination")]
    [Authorize]
    public class ExaminationController : ControllerBase {

        private readonly IExaminationData examinationData;
        private readonly IChoosenQAData choosenQAData;

        public ExaminationController(IExaminationData examinationData, IChoosenQAData choosenQAData) {
            this.examinationData = examinationData;
            this.choosenQAData = choosenQAData;
        }

        [HttpGet]
        public async Task<ActionResult<List<ExaminationEssentialsDto>>> getExaminationsAsync() {

            int userID = JwtHelpers.getGeneralID(HttpContext.Request.Headers["Authorization"]);




            return Ok(new List<ExaminationEssentialsDto>());
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ExaminationEssentialsDto>> getExaminationAsync(int id) {
            

            return Ok(new ExaminationEssentialsDto());
        }
    }
}
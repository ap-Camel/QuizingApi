using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizingApi.Dtos.WebUserDtos;
using QuizingApi.Helpers;
using QuizingApi.Services.LocalDb.Interfaces;

namespace QuizingApi.Controllers {
    [ApiController]
    [Route("webUser")]
    public class WebUserController : ControllerBase {
        private readonly IWebUserData webUserData;

        public WebUserController(IWebUserData webUserData){
            this.webUserData = webUserData;
        }


        [HttpPost]
        public async Task<ActionResult<bool>> SignupAsync(WebUserSignupDto user) {

            //check if username exists
            //check if email is valid
            //hash the password
            //insert the user and return true

            var verifyUsername = await webUserData.verifyUserNameAsync(user.userName);
            if(verifyUsername is not null) {
                return Conflict("username already exists");
            }

            var verifyEmail = await webUserData.verifyEmailAsync(user.email);
            if(verifyEmail is not null) {
                return Conflict("user with this email already exists");
            }

            string passHash = await Hashing.hash(user.userPassword);

            int result = await webUserData.insertWebUserAsync(user, passHash);

            return result > 0 ? Created(nameof(getUserDataAsync), user) : BadRequest("user was not created");
        }

        
        [HttpGet]        
        [Authorize]
        public async Task<ActionResult<WebUserEssentialsDto>> getUserDataAsync() {
            
            // get the userid from the JWT token
            // check the id
            // return user with the id

            int id = JwtHelpers.getGeneralID(HttpContext.Request.Headers["Authorization"]);

            if(id <= 0) {
                return NotFound("no user with this id exists");
            }

            var webUser = await webUserData.getWebUserByIdAsync(id);

            if(webUser is null) {
                return NotFound("no user with this id exists");
            }

            return Ok(Helpers.Converting.toWebUserEssintials(webUser));
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<bool>> udateUserInfoAsync(WebUserUpdateDto updateUser) {

            // check the id of the user for jwt
            // update the user

            int id = JwtHelpers.getGeneralID(HttpContext.Request.Headers["Authorization"]);

            if(id <= 0) {
                return NotFound("no user with this id exists");
            }

            bool result = await webUserData.updateWebUserAsync(updateUser, id);

            return result ? StatusCode(StatusCodes.Status204NoContent) : BadRequest("user was not updated");
        }


        [HttpDelete]
        [Authorize]
        public async Task<ActionResult<bool>> deleteWebUserAsync() {
            
            int id = JwtHelpers.getGeneralID(HttpContext.Request.Headers["Authorization"]);

            if(id <= 0) {
                return NotFound("no user with this id exists");
            }

            bool result = await webUserData.deleteWebUserAsync(id);

            return result ? StatusCode(StatusCodes.Status204NoContent) : BadRequest("user was not deleted"); 
        }


        [HttpPut("/username/")]
        [Authorize]
        public async Task<ActionResult<bool>> updateUsernameAsync(string newUserName) {
            
            int id = JwtHelpers.getGeneralID(HttpContext.Request.Headers["Authorization"]);

            if(id <= 0) {
                return NotFound("no user with this id exists");
            }

            var checkUsername = await webUserData.verifyUserNameAsync(newUserName);

            if(checkUsername is not null) {
                return Conflict("username taken");
            }

            bool result = await webUserData.updateUsernameAsync(newUserName, id);

            return result ? StatusCode(StatusCodes.Status204NoContent) : BadRequest("username was not updated");
        }
    }
}
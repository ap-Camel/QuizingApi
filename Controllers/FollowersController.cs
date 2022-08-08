using Microsoft.AspNetCore.Mvc;
using QuizingApi.Dtos.FollowersDtos;
using QuizingApi.Helpers;
using QuizingApi.Models;
using QuizingApi.Services.LocalDb.Interfaces;

namespace QuizingApi.Controllers {

    [ApiController]
    [Route("followers")]
    public class FollowersController : ControllerBase {
        private readonly IFollowersData followersData;
        private readonly IWebUserData webUserData;

        public FollowersController(IFollowersData followersData, IWebUserData webUserData) {
            this.followersData = followersData;
            this.webUserData = webUserData;
        }


        [HttpGet("/followers/myFollowers")]
        public async Task<ActionResult<List<FollowerEssentialsDto>>> getMyFollowersAsync() {
            int userID = JwtHelpers.getGeneralID(HttpContext.Request.Headers["Authorization"]);

            var temp = await followersData.getFollowersListAsync(userID);

            if(!temp.Any()) {
                return Ok(new List<FollowerEssentialsDto>());
            }

            List<FollowerEssentialsDto> list = new List<FollowerEssentialsDto>();
            WebUser tempW;

            foreach(FollowersModel f in temp) {
                tempW = await webUserData.getWebUserByIdAsync(f.followerID);
                list.Add(new FollowerEssentialsDto {
                    followerUsername = tempW.userName,
                    requestDate = f.requestDate
                });
            }

            return Ok(list);
        }


        [HttpGet("/followers/myFollowing")]
        public async Task<ActionResult<List<FollowerEssentialsDto>>> getMyFolloweingAsync() {

            int userID = JwtHelpers.getGeneralID(HttpContext.Request.Headers["Authorization"]);

            var temp = await followersData.getFollowersListByFollowerIdAsync(userID);

            if(!temp.Any()) {
                return Ok(new List<FollowerEssentialsDto>());
            }

            List<FollowerEssentialsDto> list = new List<FollowerEssentialsDto>();
            WebUser tempW;

            foreach(FollowersModel f in temp) {
                tempW = await webUserData.getWebUserByIdAsync(f.followerID);
                list.Add(new FollowerEssentialsDto {
                    followerUsername = tempW.userName,
                    requestDate = f.requestDate
                });
            }

            return Ok(list);
        }


        [HttpPost]
        public async Task<ActionResult<bool>> insertFollowingAsync(FollowersInsertRequestDto f) {

            int userID = JwtHelpers.getGeneralID(HttpContext.Request.Headers["Authorization"]);
            WebUser tempU = await webUserData.getWebUserByUsernameAsync(f.userName);

            if(tempU is null) {
                return NotFound("can not followe, username does not exist");
            }

            int result = await followersData.insertFollowerAsync(new FollowersInsertDto { userID = tempU.ID, followerID = userID });

            if(result <= 0) {
                return BadRequest("something went wrong, could not follow user");
            }

            return Ok();
        }


        [HttpDelete("{username}")]
        public async Task<ActionResult<bool>> deleteFollowingAsync(string username) {

            int userID = JwtHelpers.getGeneralID(HttpContext.Request.Headers["Authorization"]);
            WebUser tempU = await webUserData.getWebUserByUsernameAsync(username);

            if(tempU is null) {
                return NotFound("can not remove from followeing, username does not exist");
            }

            bool result = await followersData.deleteFollowerAsync(tempU.ID, userID);

            return result ? Ok("removed from following succesfully") : BadRequest("something went wrong, could not remove following");
        }
    }
}
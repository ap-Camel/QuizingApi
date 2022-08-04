using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QuizingApi.Dtos.WebUserDtos;
using QuizingApi.Helpers;
using QuizingApi.Models;
using QuizingApi.Services.LocalDb.Interfaces;

namespace QuizingApi.Controllers {

    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase {
        private readonly IWebUserData webUserData;
        private readonly IConfiguration config;


        public AuthController(IWebUserData webUserData, IConfiguration config) {
            this.webUserData = webUserData;
            this.config = config;
        }

        [HttpPost]
        public async Task<ActionResult<WebUserLoginReturnDto>> login(WebUserLoginDto user) {
            
            WebUser webUserVerify = await webUserData.verifyUserAsync(user.email, user.password);

            if(webUserVerify is null) {
                return NotFound("Invalid credentials");
            }

            try
            {
                return new WebUserLoginReturnDto {
                    webUser = Converting.toWebUserEssintials(webUserVerify),
                    JWT = generateToken(webUserVerify)
                };
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
                throw;
            }
        }

        private string generateToken(WebUser user) {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.userName),
                new Claim("ID", user.ID.ToString()),
                new Claim(ClaimTypes.Email, user.email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["AppSettings:Token"]));

            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                        claims: claims,
                        expires: DateTime.UtcNow.AddDays(60),
                        signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
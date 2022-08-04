using System.IdentityModel.Tokens.Jwt;

namespace QuizingApi.Helpers {
    public class JwtHelpers {
        public static int getGeneralID(string token) {
            string tokenHash = token.Split(" ")[1];

            var newToken = new JwtSecurityToken(tokenHash);
            int userId = int.Parse(newToken.Claims.First(x => x.Type == "ID").Value);

            return userId;
        }
    }
}
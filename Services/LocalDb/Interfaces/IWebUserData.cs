using QuizingApi.Dtos.WebUserDtos;
using QuizingApi.Models;

namespace QuizingApi.Services.LocalDb.Interfaces {
    public interface IWebUserData {
        Task<WebUser> verifyUserAsync(string email, string password);
        Task<WebUser> verifyUserNameAsync(string username);
        Task<WebUser> verifyEmailAsync(string email);
        Task<int> insertWebUserAsync(WebUserSignupDto user, string pass);
        Task<WebUser> getWebUserByIdAsync(int id);
        Task<bool> updateWebUserAsync(WebUserUpdateDto updateUser, int id);
        Task<bool> deleteWebUserAsync(int id);
        Task<bool> updateUsernameAsync(string newUserName, int id);

    }
}
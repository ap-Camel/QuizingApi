using QuizingApi.Dtos.WebUserDtos;
using QuizingApi.Helpers;
using QuizingApi.Models;
using QuizingApi.Services.LocalDb.Interfaces;

namespace QuizingApi.Services.LocalDb.Tables {
    public class WebUserData : IWebUserData {
        private readonly ISqlDataAccess _db;

        public WebUserData(ISqlDataAccess db) {
            _db = db;
        }

        public async Task<WebUser> verifyUserAsync(string email, string password) {
            string hashedPass = await Hashing.hash(password);

            string sql = $"select top 1 * from webUser where email = '{email}' and userPassword = '{hashedPass}'";

            return await _db.LoadSingle<WebUser>(sql);
        }

        public async Task<WebUser> verifyUserNameAsync(string username) {
            string sql = $"select top 1 * from webUser where userName = '{username}'";

            return await _db.LoadSingle<WebUser>(sql);
        } 

        public async Task<WebUser> verifyEmailAsync(string email) {
            string sql = $"select top 1 * from webUser where email = '{email}'";

            return await _db.LoadSingle<WebUser>(sql);
        }

        public async Task<int> insertWebUserAsync(WebUserSignupDto user, string pass) {
            string sql = $"insert into webUser output inserted.id values ('{user.firstName}', '{user.lastName}', '{user.userName}', '{user.email}', '{pass}', default);";

            return await _db.insertDataWithReturn(sql);
        }

        public async Task<WebUser> getWebUserByIdAsync(int id) {
            string sql = $"select top 1 * from webUser where ID = {id}";

            return await _db.LoadSingle<WebUser>(sql);
        }

        public async Task<bool> updateWebUserAsync(WebUserUpdateDto updateUser, int id) {
            string sql = $"update webUser set firstName = '{updateUser.firstName}', lastName = '{updateUser.lastName}' where ID = {id}";

            return await _db.insertData(sql);
        }

        public async Task<bool> deleteWebUserAsync(int id) {
            string sql = $"delete from webUser where ID = {id}";

            return await _db.insertData(sql);
        }

        public async Task<bool> updateUsernameAsync(string newUserName, int id) {
            string sql = $"update webUser set userName = '{newUserName}' where ID = '{id}'";

            return await _db.insertData(sql);
        }

        public async Task<WebUser> getWebUserByUsernameAsync(string username) {
            string sql = $"select top 1 * from webUser where username = '{username}'";

            return await _db.LoadSingle<WebUser>(sql);
        }
    }
}
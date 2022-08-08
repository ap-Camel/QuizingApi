using QuizingApi.Dtos.FollowersDtos;
using QuizingApi.Models;
using QuizingApi.Services.LocalDb.Interfaces;

namespace QuizingApi.Services.LocalDb.Tables {
    public class FollowersData : IFollowersData {
        private readonly ISqlDataAccess _db;
        public FollowersData(ISqlDataAccess db) {
            _db = db;
        }


        public async Task<IEnumerable<FollowersModel>> getFollowersListAsync(int userID) {
            string sql = $"select * from followers where userID = {userID}";

            return await _db.LoadMany<FollowersModel>(sql);
        }

        public async Task<FollowersModel> getFollowerAsync(int userID, int followerID) {
            string sql = $"select top 1 * from followers where userID = {userID} and followerID = {followerID}";

            return await _db.LoadSingle<FollowersModel>(sql);
        }

        public async Task<int> insertFollowerAsync(FollowersInsertDto follower) {
            string sql = $"insert into followers output inserted.id values({follower.userID}, {follower.followerID}, default, default, default);";

            return await _db.insertDataWithReturn(sql);
        }

        public async Task<bool> deleteFollowerAsync(int userID, int followerID) {
            string sql = $"delete from followers where userID = {userID} and followerID = {followerID}";

            return await _db.insertData(sql);
        }

        public async Task<IEnumerable<FollowersModel>> getFollowersListByFollowerIdAsync(int followerID) {
            string sql = $"select * from followers where followerID = {followerID}";

            return await _db.LoadMany<FollowersModel>(sql);
        }
    }
}
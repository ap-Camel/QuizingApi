using QuizingApi.Dtos.FollowersDtos;
using QuizingApi.Models;

namespace QuizingApi.Services.LocalDb.Interfaces {
    public interface IFollowersData {
        Task<IEnumerable<FollowersModel>> getFollowersListAsync(int userID);
        Task<FollowersModel> getFollowerAsync(int userID, int followerID);
        Task<int> insertFollowerAsync(FollowersInsertDto follower);
        Task<bool> deleteFollowerAsync(int userID, int followerID);
        Task<IEnumerable<FollowersModel>> getFollowersListByFollowerIdAsync(int followerID);
    }
}
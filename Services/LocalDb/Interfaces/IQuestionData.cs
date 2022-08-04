using QuizingApi.Dtos.QuestionDtos;
using QuizingApi.Models;

namespace QuizingApi.Services.LocalDb.Interfaces {
    public interface IQuestionData {
        Task<QuestionModel> getQuestionAsync(int questionID, int userID);
        Task<IEnumerable<QuestionModel>> getQuestionsByExamIdAsync(int examID);
        Task<IEnumerable<QuestionModel>> getQuestionsByUserIdAsync(int userID);
        Task<int> insertQuestionAsync(QuestionInsertDto question);
        Task<bool> updateQuestionAsync(QuestionUpdateDto q);
        Task<bool> deleteQuestionAsync(int questionID, int examID);
        Task<QuestionModel> verifyQuestionOwnerAsync(int userID, int questionID);
    }
}
using QuizingApi.Dtos.AnswerDtos;
using QuizingApi.Models;

namespace QuizingApi.Services.LocalDb.Interfaces {
    public interface IAnswerData {
        Task<AnswerModel> getAnswerByUserIdAsync(int answerID, int userID);
        Task<IEnumerable<AnswerModel>> getAnswersByUserIdAsync(int userID);
        Task<IEnumerable<AnswerModel>> getAnswersByQuestionIdAync(int questionID, int userID);
        Task<int> insertAnswerAsync(AnswerInsertDto answer);
        Task<bool> updateAnswerAsync(AnswerUpdateDto answer);
        Task<bool> deleteAnswerAsync(int answerID, int questionID);
        Task<bool> deleteAnswerByQuestionIdAsync(int questionID);

    }
}
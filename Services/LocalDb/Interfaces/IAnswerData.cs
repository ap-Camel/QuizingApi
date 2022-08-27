using QuizingApi.Dtos.AnswerDtos;
using QuizingApi.Models;

namespace QuizingApi.Services.LocalDb.Interfaces {
    public interface IAnswerData {
        Task<AnswerModel> getAnswerByUserIdAsync(int answerID, int userID);
        Task<AnswerModel> getAnswerByIdAsync(int answerID);
        Task<IEnumerable<AnswerModel>> getAnswersByUserIdAsync(int userID);
        Task<IEnumerable<AnswerModel>> getAnswersByQuestionIdAync(int questionID);
        Task<int> insertAnswerAsync(AnswerInsertDto answer);
        Task<bool> updateAnswerAsync(AnswerUpdateDto answer);
        Task<bool> deleteAnswerAsync(int answerID, int questionID);
        Task<bool> deleteAnswerByQuestionIdAsync(int questionID);
        Task<AnswerModel> checkAnswerAsync(string answer, int questionID);
    }
}
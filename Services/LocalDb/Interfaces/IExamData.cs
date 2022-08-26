using QuizingApi.Dtos.ExamDtos;
using QuizingApi.Models;

namespace QuizingApi.Services.LocalDb.Interfaces {
    public interface IExamData {
        Task<ExamModel> getExamAsync(int userID, int examID);
        Task<ExamModel> getExamByIdAsync(int examID);
        Task<IEnumerable<ExamModel>> getExamsAsync(int userID);
        Task<int> insertExamAsync(ExamInsertDto exam, int userID);
        Task<bool> updateExamAsync(ExamUpdateDto updateExam, int userID);
        Task<bool> deleteExamAsync(int examID, int userID);
        Task<ExamModel> verifyExamOwnerAsync(int userID, int examID);
        Task<IQueryable<ExamSearchReturnDto>> getAllExamsAsync();
        Task<IEnumerable<ExamSearchReturnDto>> getTopExams(int number);
        Task<IEnumerable<ExamSearchReturnDto>> getExamsByUsername(string username);
    }
}
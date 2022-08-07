using QuizingApi.Dtos.ExaminationDtos;
using QuizingApi.Models;

namespace QuizingApi.Services.LocalDb.Interfaces {
    public interface IExaminationData
    {
        Task<ExaminationModel> getExaminationByUserIdAsync(int id, int userID);
        Task<IEnumerable<ExaminationModel>> getExaminationsByUserIdAsync(int userID);
        Task<int> insertExaminationAsync(ExaminationInsertDto e);
        Task<bool> updateExaminationResultAsync(int result, int examinationID, int userID);
    }
}
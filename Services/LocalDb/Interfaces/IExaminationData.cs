using QuizingApi.Dtos.ExaminationDtos;
using QuizingApi.Models;

namespace QuizingApi.Services.LocalDb.Interfaces {
    public interface IExaminationData
    {
        Task<ExaminationModel> getExaminationByUserIdAsync(int id, int userID);
        Task<IEnumerable<ExaminationModel>> getExaminationsAsyncByUserIdAsync(int userID);
        Task<int> insertExaminationAsync(ExaminationInsertDto e);
    }
}
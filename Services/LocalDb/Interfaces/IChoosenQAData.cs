using QuizingApi.DtosChoosenQuestionAnswersDtos;
using QuizingApi.Models;

namespace QuizingApi.Services.LocalDb.Interfaces {
    public interface IChoosenQAData
    {
        Task<ChooseQuestionsAnswersModel> getChoosenQAAsync(int id, int userID);
        Task<IEnumerable<ChooseQuestionsAnswersModel>> getCQAbyExaminationIdAsync(int examinationID);
        Task<int> insertCQAasync(ChoosenQAInsertDto insert);
    }
}
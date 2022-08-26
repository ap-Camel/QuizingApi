using QuizingApi.DtosChoosenQuestionAnswersDtos;
using QuizingApi.Models;

namespace QuizingApi.Services.LocalDb.Interfaces {
    public interface IChoosenQAData
    {
        Task<ChooseQuestionsAnswersModel> getChoosenQAAsync(int id, int userID);
        Task<IEnumerable<ChooseQuestionsAnswersModel>> getCQAbyExaminationIdAsync(int examinationID);
        Task<int> insertCQAasync(ChoosenQAInsertDto insert);
        Task<int> insertCQA_WithoutAnswerAsync(ChoosenQAInsertDto insert);
        Task<bool> deleteCQA_ByIdAsync(int CQA_id, int examinationID);
        Task<bool> updateCQA_AnswerIdAsync(int answerID, int questionID, int examinationID, bool result);
        Task<IEnumerable<int>> getQuestionIdsFromExaminationAsync(int examinationID, int userID);
    }
}
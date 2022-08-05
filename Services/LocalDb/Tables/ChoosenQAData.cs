using QuizingApi.DtosChoosenQuestionAnswersDtos;
using QuizingApi.Models;
using QuizingApi.Services.LocalDb.Interfaces;

namespace QuizingApi.Services.LocalDb.Tables {

    public class ChoosenQAData : IChoosenQAData
    {
        private readonly ISqlDataAccess _db;

        public ChoosenQAData(ISqlDataAccess db)
        {
            _db = db;
        }


        public async Task<ChooseQuestionsAnswersModel> getChoosenQAAsync(int id, int userID)
        {
            string sql = $"select top 1 * from choosenQuestionsAnswers where ID = {id}";

            return await _db.LoadSingle<ChooseQuestionsAnswersModel>(sql);
        }

        public async Task<IEnumerable<ChooseQuestionsAnswersModel>> getCQAbyExaminationIdAsync(int examinationID)
        {
            string sql = $"select * from choosenQuestionsAnswers where examinationID = {examinationID}";

            return await _db.LoadMany<ChooseQuestionsAnswersModel>(sql);
        }

        public async Task<int> insertCQAasync(ChoosenQAInsertDto insert)
        {
            string sql = $"insert into choosenQuestionsAnswers output inserted.id values({insert.questionID}, {insert.answerID}, {insert.examinationID})";

            return await _db.insertDataWithReturn(sql);
        }
    }
}
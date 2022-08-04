using QuizingApi.Dtos.QuestionDtos;
using QuizingApi.Models;
using QuizingApi.Services.LocalDb.Interfaces;

namespace QuizingApi.Services.LocalDb.Tables {
    public class QuestionData : IQuestionData {
        private readonly ISqlDataAccess _db;

        public QuestionData(ISqlDataAccess db) {
            _db = db;
        }

        public async Task<QuestionModel> getQuestionAsync(int questionID, int userID) {
            string sql = $"select top 1 question.* from question join exam on question.examID = exam.ID join webUser on exam.userID = webUser.ID where webUser.ID = {userID} and question.ID = {questionID}";

            return await _db.LoadSingle<QuestionModel>(sql);
        }

        public async Task<IEnumerable<QuestionModel>> getQuestionsByExamIdAsync(int examID) {
            string sql = $"select * from question where examID = {examID}";

            return await _db.LoadMany<QuestionModel>(sql);
        }

        public async Task<IEnumerable<QuestionModel>> getQuestionsByUserIdAsync(int userID) {
            string sql = $"select question.* from question join exam on question.examID = exam.ID join webUser on exam.userID = webUser.ID where webUser.ID = {userID}";

            return await _db.LoadMany<QuestionModel>(sql);
        }

        public async Task<int> insertQuestionAsync(QuestionInsertDto question) {
            string sql = $"insert into question output inserted.id values ('{question.question}', {question.difficulty}, default, default, '{question.questionType}', {Convert.ToInt16(question.hasImage)}, '{question.imgUrl}', {question.examID});";

            return await _db.insertDataWithReturn(sql);
        }

        public async Task<bool> updateQuestionAsync(QuestionUpdateDto q) {
            string sql = $"update question set question = '{q.question}', difficulty = {q.difficulty}, dateUpdated = getdate(), questionType = '{q.questionType}', hasImage = {Convert.ToInt16(q.hasImage)}, imgUrl = '{q.imgUrl}' where examID = {q.examID} and ID = {q.ID}";

            return await _db.insertData(sql);
        }

        public async Task<bool> deleteQuestionAsync(int questionID, int examID) {
            string sql = $"delete from question where ID = {questionID} and examID = {examID}";

            return await _db.insertData(sql);
        }

        public async Task<QuestionModel> verifyQuestionOwnerAsync(int userID, int questionID) {
            string sql = $"select top 1 question.* from question join exam on question.examID = exam.ID where exam.userID = {userID} and question.ID = {questionID}";

            return await _db.LoadSingle<QuestionModel>(sql);
        }

        public async Task<bool> deleteQuestionsByExamIdAsync(int examID) {
            string sql = $"delete from question where examID = {examID}";

            return await _db.insertData(sql);
        }
    }
}
using QuizingApi.Dtos.AnswerDtos;
using QuizingApi.Models;
using QuizingApi.Services.LocalDb.Interfaces;

namespace QuizingApi.Services.LocalDb.Tables {
    public class AnswerData : IAnswerData {
        private readonly ISqlDataAccess _db;

        public AnswerData(ISqlDataAccess db) {
            _db = db;
        }


        public async Task<AnswerModel> getAnswerByUserIdAsync(int answerID, int userID) {
            string sql = "select top 1 answer.* from answer " + 
                        "join question on answer.questionID = question.ID " +
                        "join exam on question.examID = exam.ID " +
                        "join webUser on exam.userID = webUser.ID " +
                        $"where answer.ID = {answerID} and webUser.ID = {userID}";

            return await _db.LoadSingle<AnswerModel>(sql);
        }


        public async Task<IEnumerable<AnswerModel>> getAnswersByUserIdAsync(int userID) {
            string sql = "select answer.* from answer " + 
                        "join question on answer.questionID = question.ID " +
                        "join exam on question.examID = exam.ID " +
                        "join webUser on exam.userID = webUser.ID " +
                        $"where webUser.ID = {userID}";

            return await _db.LoadMany<AnswerModel>(sql);
        }


        public async Task<IEnumerable<AnswerModel>> getAnswersByQuestionIdAync(int questionID, int userID) {
            string sql = "select answer.* from answer " + 
                        "join question on answer.questionID = question.ID " +
                        "join exam on question.examID = exam.ID " +
                        "join webUser on exam.userID = webUser.ID " +
                        $"where webUser.ID = {userID} and question.ID = {questionID}";

            return await _db.LoadMany<AnswerModel>(sql);
        }


        public async Task<int> insertAnswerAsync(AnswerInsertDto answer) {
            string sql = $"insert into answer output inserted.id values ({Convert.ToInt16(answer.correct)}, '{answer.answer}', {Convert.ToInt16(answer.active)}, default, default, {Convert.ToInt16(answer.hasImage)}, '{answer.imgUrl}', {answer.questionID});";

            return await _db.insertDataWithReturn(sql);
        }


        public async Task<bool> updateAnswerAsync(AnswerUpdateDto answer) {
            string sql = $"update answer set correct = {Convert.ToInt16(answer.correct)}, answer = '{answer.answer}', active = {Convert.ToInt16(answer.active)}, dateUpdated = getdate(), hasImage = {Convert.ToInt16(answer.hasImage)}, imgURL = '{answer.imgUrl}' where questionID = {answer.questionID}";

            return await _db.insertData(sql);
        }

        public async Task<bool> deleteAnswerAsync(int answerID, int questionID) {
            string sql = $"delete from answer where ID = {answerID} and questionID = {questionID}";

            return await _db.insertData(sql);
        }

        public async Task<bool> deleteAnswerByQuestionIdAsync(int questionID) {
            string sql = $"delete from answer where questionID = {questionID}";

            return await _db.insertData(sql);
        }

        public async Task<AnswerModel> checkAnswerAsync(string answer, int questionID) {
            string sql = $"select top 1 * from answer where answer = '{answer}' and questionID = {questionID} and correct = 1";

            return await _db.LoadSingle<AnswerModel>(sql);
        }
    }
}
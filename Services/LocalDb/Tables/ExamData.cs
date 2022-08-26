using QuizingApi.Dtos.ExamDtos;
using QuizingApi.Models;
using QuizingApi.Services.LocalDb.Interfaces;

namespace QuizingApi.Services.LocalDb.Tables {
    public class ExamData : IExamData {
        private readonly ISqlDataAccess _db;

        public ExamData(ISqlDataAccess db) {
            _db = db;
        }


        public async Task<ExamModel> getExamAsync(int userID, int examID) {
            string sql = $"select top 1 * from exam where ID = {examID} and userID = {userID}";

            return await _db.LoadSingle<ExamModel>(sql);
        }

        public async Task<ExamModel> getExamByIdAsync(int examID) {
            string sql = $"select top 1 * from exam where ID = {examID}";

            return await _db.LoadSingle<ExamModel>(sql);
        }

        public async Task<IEnumerable<ExamModel>> getExamsAsync(int userID) {
            string sql = $"select * from exam where userID = {userID}";

            return await _db.LoadMany<ExamModel>(sql);
        }

        public async Task<int> insertExamAsync(ExamInsertDto exam, int userID) {
            string sql = $"insert into exam output inserted.id values ('{exam.title}', {exam.numOfQuestions}, {exam.duration}, {Convert.ToInt16(exam.active)}, {exam.difficulty}, default, default, {userID}, {(string.IsNullOrEmpty(exam.imgURL) ? "default" : ($"'{exam.imgURL}'"))})";

            return await _db.insertDataWithReturn(sql);
        }

        public async Task<bool> updateExamAsync(ExamUpdateDto updateExam, int userID) {
            string sql = $"update exam set title = '{updateExam.title}', numOfQuestions = {updateExam.numOfQuestions}, duration = {updateExam.duration}, active = {Convert.ToInt16(updateExam.active)}, difficulty = {updateExam.difficulty}, dateUpdated = getdate(), imgURL = '{updateExam.imgURL}' where userID = {userID} and ID = {updateExam.ID}";

            return await _db.insertData(sql);
        }

        public async Task<bool> deleteExamAsync(int examID, int userID) {
            string sql = $"delete from exam where ID = {examID} and userID = {userID}";

            return await _db.insertData(sql);
        }


        public async Task<ExamModel> verifyExamOwnerAsync(int userID, int examID) {
            string sql = $"select top 1 * from exam where userID = {userID} and ID = {examID}";

            return await _db.LoadSingle<ExamModel>(sql);
        }

        public async Task<IQueryable<ExamSearchReturnDto>> getAllExamsAsync() {
            string sql = $"select exam.ID, exam.title, exam.duration, exam.difficulty, exam.imgURL, count(examination.ID) as count from exam left join examination on exam.ID = examination.examID group by exam.ID, exam.title, exam.duration, exam.difficulty, exam.imgURL";

            return (await _db.LoadMany<ExamSearchReturnDto>(sql)).AsQueryable();
        }

        public async Task<IEnumerable<ExamSearchReturnDto>> getTopExams(int number) {
            string sql = $"select top {number} exam.ID, exam.title, exam.duration, exam.difficulty, exam.imgURL, count(examination.ID) as count from exam left join examination on exam.ID = examination.examID group by exam.ID, exam.title, exam.duration, exam.difficulty, exam.imgURL ORDER BY count DESC;";

            return await _db.LoadMany<ExamSearchReturnDto>(sql);
        }

        public async Task<IEnumerable<ExamSearchReturnDto>> getExamsByUsername(string username) {
            string sql = $"select top 15 exam.ID, exam.title, exam.duration, exam.difficulty, exam.imgURL, count(examination.ID) as count from exam left join examination on exam.ID = examination.examID join webUser on exam.userID = webUser.ID where webUser.username = '{username}' group by exam.ID, exam.title, exam.duration, exam.difficulty, exam.imgURL ORDER BY count DESC;";

            return await _db.LoadMany<ExamSearchReturnDto>(sql);
        }

    }
}
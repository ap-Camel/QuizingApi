using QuizingApi.Dtos.ExaminationDtos;
using QuizingApi.Models;
using QuizingApi.Services.LocalDb.Interfaces;

namespace QuizingApi.Services.LocalDb.Tables {

    public class ExaminationData : IExaminationData
    {
        private readonly ISqlDataAccess _db;

        public ExaminationData(ISqlDataAccess db)
        {
            _db = db;
        }


        public async Task<IEnumerable<ExaminationModel>> getExaminationsByUserIdAsync(int userID)
        {
            string sql = $"select * from examination where userID = {userID} order by atDate DESC";

            return await _db.LoadMany<ExaminationModel>(sql);
        }

        public async Task<ExaminationModel> getExaminationByUserIdAsync(int id, int userID)
        {
            string sql = $"select top 1 * from examination where userID = {userID} and ID = {id}";

            return await _db.LoadSingle<ExaminationModel>(sql);
        }


        public async Task<int> insertExaminationAsync(ExaminationInsertDto e)
        {
            string sql = $"insert into examination output inserted.id values ({e.examID}, {e.userID}, default, {e.result}, default);";

            return await _db.insertDataWithReturn(sql);
        }

        public async Task<bool> updateExaminationResultAsync(int result, int examinationID, int userID) {
            string sql = $"update examination set result = {result}, evaluated = 1 where ID = {examinationID} and userID = {userID}";

            return await _db.insertData(sql);
        }
    }
}
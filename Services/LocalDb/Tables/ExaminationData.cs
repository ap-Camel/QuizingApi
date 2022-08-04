using QuizingApi.Models;

namespace QuizingApi.Services.LocalDb.Tables {
    public class ExaminationData{
        private readonly ISqlDataAccess _db;

        public ExaminationData(ISqlDataAccess db) {
            _db = db;
        }


        public async Task<IEnumerable<ExaminationModel>> getExaminationsAsyncByUserIdAsync(int userID) {
            string sql = $"select * from examination where userID = {userID}";

            return await _db.LoadMany<ExaminationModel>(sql);
        }

        public async Task<ExaminationModel> getExaminationByUserIdAsync(int id, int userID) {
            string sql = $"select top 1 * from examination where userID = {userID} and ID = {id}";

            return await _db.LoadSingle<ExaminationModel>(sql);
        }


        public async Task<int> insertExaminationAsync(){
            
        }
    }
}
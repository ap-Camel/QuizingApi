namespace QuizingApi.Services.LocalDb {
    public interface ISqlDataAccess
    {
        string connectionStringName { get; set; }

        Task<bool> insertData(string sql);
        Task<List<T>> LoadMany<T>(string sql);
        Task<T> LoadSingle<T>(string sql);
        Task<int> insertDataWithReturn(string sql);
        Task<T> insertDataWithObjectReturn<T>(string sql);
    }
}
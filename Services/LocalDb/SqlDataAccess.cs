using System.Data;
using System.Data.SqlClient;
using Dapper;


namespace QuizingApi.Services.LocalDb {
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _config;

        public string connectionStringName { get; set; } = "Default";

        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }

        public async Task<List<T>> LoadMany<T>(string sql)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    var list = await connection.QueryAsync<T>(sql);
                    return list.ToList();
                }
                catch (Exception e)
                {
                    return new List<T>();
                }
            }
        }

        public async Task<T> LoadSingle<T>(string sql)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);
            T? data = default(T);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    data = await connection.QuerySingleAsync<T>(sql);
                    return data;
                }
                catch (Exception e)
                {
                    return data;
                }
            }
        }

        public async Task<bool> insertData(string sql)
        {

            string connectionString = _config.GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.ExecuteAsync(sql);
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        public async Task<int> insertDataWithReturn(string sql)
        {

            string connectionString = _config.GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    int id = await connection.ExecuteScalarAsync<int>(sql);
                    return id;
                }
                catch (Exception e)
                {
                    return 0;
                }
            }
        }

        public async Task<T> insertDataWithObjectReturn<T>(string sql)
        {

            string connectionString = _config.GetConnectionString(connectionStringName);
            T? data = default(T);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    data = (T)(await connection.ExecuteScalarAsync(sql));
                    return data;
                }
                catch (Exception e)
                {
                    return data;
                }
            }
        }


    }
}
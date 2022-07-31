using MySqlConnector;
using System.Data.Common;
using Dapper;

namespace Common
{
    public class Db
    {
        private static readonly string _conn = "Server=localhost;port=3306;User ID=root;Password=123456;Database=dtm_barrier";

        public static DbConnection GeConn() => new MySqlConnection(_conn);

        public static async Task<bool> UpdateTask(string taskId, string description)
        {
            try
            {
                var sql = "update task set description = @description  where taskId = @taskId";
                using var conn = GeConn();
                await conn.OpenAsync();
                var affectedRows = await conn.ExecuteAsync(sql, new { taskId, description });
                return affectedRows > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        
    }
}
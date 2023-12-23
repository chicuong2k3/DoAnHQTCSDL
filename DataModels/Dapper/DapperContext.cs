using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DataModels.Config
{
    public class DapperContext
    {
        private readonly string _connectionString;
        public DapperContext()
        {
            _connectionString = "Server=(local);Database=NhaKhoaDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
        }
        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);

    }
}

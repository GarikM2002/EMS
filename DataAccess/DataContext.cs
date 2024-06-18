using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
    public class DataContext(string connectionString)
    {
        private readonly string connectionString = connectionString;

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(connectionString);
        }
    }

}

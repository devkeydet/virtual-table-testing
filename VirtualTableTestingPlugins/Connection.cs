using System;
using System.Data.SqlClient;

namespace VirtualTableTestingPlugins
{
    public static class Connection
    {
        public static SqlConnection GetConnection()
        {
            try
            {
                //sample database to connect to 
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "your-azure-sql.database.windows.net";
                builder.UserID = "your-userid";
                builder.Password = "your-password";
                builder.InitialCatalog = "your-database-name";
                SqlConnection connection = new SqlConnection(builder.ConnectionString);
                return connection;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
        }
    }
}

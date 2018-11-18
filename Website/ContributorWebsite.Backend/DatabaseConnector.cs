using System;
using System.Data.SqlClient;

namespace ContributorWebsite.Backend
{
    internal class DatabaseConnector
    {
        internal static SqlConnection OpenConnection()
        {
            // Get the connection string from app settings and use it to create a connection.
            var connString = Environment.GetEnvironmentVariable("DatabaseConnectionString");

            var connection = new SqlConnection(connString);
            connection.Open();

            return connection;
        }
    }
}

using dotenv.net;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library_management
{
    public class DBConnectionService
    {
        private static string? _connectionString = null;
        private static MySqlConnection? _connection = null;

        private static void CreateConnectionString()
        {
            DotEnv.Load();
            
            string dbUser = Environment.GetEnvironmentVariable("DB_USER");
            string dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
            string dbServer = Environment.GetEnvironmentVariable("DB_SERVER");
            string dbName = Environment.GetEnvironmentVariable("DB_NAME");

            Console.WriteLine("dbUser: " + dbUser);
            Console.WriteLine("dbPassword: " + dbPassword);
            Console.WriteLine("dbServer: " + dbServer);
            Console.WriteLine("dbName: " + dbName);

            _connectionString = $"Server={dbServer};Database={dbName};User={dbUser};Password={dbPassword};";
        }

        public static MySqlConnection GetConnection()
        {
            if (_connection == null)
            {
                CreateConnectionString();
                _connection = new MySqlConnection(_connectionString);
                _connection.Open();
            }
            else if (_connection.State == System.Data.ConnectionState.Closed)
            {
                _connection.Open();
            }
            return _connection;
        }

        public static void CloseConnection()
        {
            if (_connection != null && _connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }
        }

    }
}

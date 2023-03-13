using Microsoft.Data.Sqlite;

namespace CCLibrary.Data
{
    public class Database
    {
        internal static readonly string _dbSource = @"Data Source=CaloriesCalculator.db";
        internal SqliteConnection _connection;

        public Database()
        {
            _connection = new SqliteConnection(_dbSource);
            Initialize();
        }

        /// <summary>
        /// Check and keep connection open.
        /// </summary>
        internal void CheckConnection()
        {
            switch (_connection.State)
            {
                case System.Data.ConnectionState.Closed:
                    _connection.Open();
                    break;
                case System.Data.ConnectionState.Broken:
                    _connection = new SqliteConnection(_dbSource);
                    _connection.Open();
                    break;
            }
        }

        private void Initialize()
        {
            CheckConnection();

            // ... 
        }
    }
}

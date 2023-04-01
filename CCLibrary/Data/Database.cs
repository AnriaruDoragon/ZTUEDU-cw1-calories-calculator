using Microsoft.Data.Sqlite;

namespace CCLibrary.Data
{
    public class Database
    {
        internal static readonly string DbSource = @"Data Source=CaloriesCalculator.db";
        internal SqliteConnection Connection;

        public DataContext Context;

        public Database()
        {
            Connection = new SqliteConnection(DbSource);
            Initialize();

            Context = new DataContext(Connection);
        }

        /// <summary>
        /// Check and keep connection open.
        /// </summary>
        internal static void CheckConnection(SqliteConnection connection)
        {
            switch (connection.State)
            {
                case System.Data.ConnectionState.Closed:
                    connection.Open();
                    break;
                case System.Data.ConnectionState.Broken:
                    connection = new SqliteConnection(DbSource);
                    connection.Open();
                    break;
            }
        }

        private void Initialize()
        {
            CheckConnection(Connection);

             new SqliteCommand(@"
                CREATE TABLE IF NOT EXISTS ProfileConsumedProducts (
                    rowid INTEGER PRIMARY KEY,
                    ProfileID INTEGER NOT NULL,
                    ProductID INTEGER NOT NULL,
                    ProductMass FLOAT NOT NULL,
                    ConsumedDate DATE NOT NULL
                );", Connection).ExecuteNonQuery();
        }
    }
}

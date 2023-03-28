using System;
using Microsoft.Data.Sqlite;
using CCLibrary.User;
using CCLibrary.Products;

namespace CCLibrary.Data
{
    public class Database
    {
        internal static readonly string DbSource = @"Data Source=CaloriesCalculator.db";
        internal SqliteConnection Connection;

        public ProductContext ProductContext = new();
        public ProfileContext ProfileContext = new();

        public Database()
        {
            Connection = new SqliteConnection(DbSource);
            Initialize();
        }

        /// <summary>
        /// Check and keep connection open.
        /// </summary>
        internal void CheckConnection()
        {
            switch (Connection.State)
            {
                case System.Data.ConnectionState.Closed:
                    Connection.Open();
                    break;
                case System.Data.ConnectionState.Broken:
                    Connection = new SqliteConnection(DbSource);
                    Connection.Open();
                    break;
            }
        }

        private void Initialize()
        {
            CheckConnection();

             new SqliteCommand(@"
                CREATE TABLE IF NOT EXISTS ProfileConsumedProducts (
                    rowid INTEGER PRIMARY KEY,
                    ProfileID INTEGER NOT NULL,
                    ProductID INTEGER NOT NULL,
                    ProductMass FLOAT NOT NULL,
                    ConsumedDate DATE NOT NULL
                );", Connection).ExecuteNonQuery();
        }

        /// <summary>
        /// Get daily consumption for a specific profile in a specific day.
        /// </summary>
        public DailyConsumption GetProfileConsumption(Profile profile, DateTime date)
        {
            DailyConsumption consumption = new(profile, date);

            CheckConnection();
            SqliteCommand selectCommand = new(@"
                SELECT ProductID, ProductMass
                FROM ProfileConsumedProducts
                WHERE ProfileID = @Profile AND ConsumedDate = @Date;", Connection);
            selectCommand.Parameters.AddWithValue("@Profile", profile.Id);
            selectCommand.Parameters.AddWithValue("@Date", date.ToString("yyyy-MM-dd"));
            SqliteDataReader reader = selectCommand.ExecuteReader();

            ProductContext productContext = new();
            while (reader.Read())
            {
                Product? product = productContext.GetProduct(reader.GetInt64(0));
                if (product is null)
                    continue;
                Product copy = product.Copy();
                copy.NetMassInGrams = reader.GetInt32(1);
                consumption.Consume(copy);
            }

            return consumption;
        }

        /// <summary>
        /// Insert a record to the db for a profile consumed product.
        /// </summary>
        internal void AddProfileConsumption(Profile profile, Product product, DateTime date)
        {
            CheckConnection();
            SqliteCommand insertCommand = new(@"
                INSERT INTO ProfileConsumedProducts (ProfileID, ProductID, ProductMass, ConsumedDate)
                VALUES (@ProfileID, @ProductID, @ProductMass, @ConsumedDate);", Connection);
            insertCommand.Parameters.AddWithValue("@ProfileID", profile.Id);
            insertCommand.Parameters.AddWithValue("@ProductID", product.Id);
            insertCommand.Parameters.AddWithValue("@ProductMass", product.NetMassInGrams);
            insertCommand.Parameters.AddWithValue("@ConsumedDate", date.ToString("yyyy-MM-dd"));
            insertCommand.ExecuteNonQuery();
        }
    }
}

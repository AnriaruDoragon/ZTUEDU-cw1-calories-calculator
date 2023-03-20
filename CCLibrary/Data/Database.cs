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
                    ID INT NOT NULL AUTO_INCREMENT,
                    ProfileID INT NOT NULL,
                    ProductID INT NOT NULL,
                    ProductMass FLOAT NOT NULL,
                    ConsumedDate DATE NOT NULL,
                    PRIMARY KEY (ID)
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
                SELECT *
                FROM ProfileConsumedProducts
                WHERE ProfileID = @Profile AND ConsumedDate = @Date;", Connection);
            selectCommand.Parameters.AddWithValue("@Profile", profile.Id);
            selectCommand.Parameters.AddWithValue("@Date", date.ToString("yyyy-MM-dd"));
            SqliteDataReader reader = selectCommand.ExecuteReader();

            ProductContext productContext = new();
            while (reader.Read())
            {
                Product? product = productContext.GetProduct(reader.GetInt64(2));
                if (product is null)
                    continue;
                Product copy = product.Copy();
                copy.NetMassInGrams = reader.GetInt32(3);
                consumption.Consume(copy);
            }

            return consumption;
        }
    }
}

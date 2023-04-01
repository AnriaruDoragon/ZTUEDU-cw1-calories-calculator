using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using CCLibrary.User;
using CCLibrary.Products;
using CCLibrary.Exceptions;

namespace CCLibrary.Data
{
    public partial class DataContext : DbContext
    {
        /// <summary>
        /// Get daily consumption for a specific profile in a specific day.
        /// </summary>
        public DailyConsumption GetProfileConsumption(Profile profile, DateTime date)
        {
            DailyConsumption consumption = new(profile, date);

            Data.Database.CheckConnection(_connection);

            SqliteCommand selectCommand = new(@"
                SELECT ProductID, ProductMass
                FROM ProfileConsumedProducts
                WHERE ProfileID = @Profile AND ConsumedDate = @Date;", _connection);
            selectCommand.Parameters.AddWithValue("@Profile", profile.Id);
            selectCommand.Parameters.AddWithValue("@Date", date.ToString("yyyy-MM-dd"));
            SqliteDataReader reader = selectCommand.ExecuteReader();

            while (reader.Read())
            {
                try
                {
                    Product product = GetProduct(reader.GetInt64(0));
                    product.NetMassInGrams = reader.GetInt32(1);
                    consumption.Consume(product);
                }
                catch (ProductNotFoundException)
                {
                    // ignored
                }
            }

            return consumption;
        }

        /// <summary>
        /// Insert a record to the db for a profile consumed product.
        /// </summary>
        internal void AddProfileConsumption(Profile profile, Product product, DateTime date)
        {
            Data.Database.CheckConnection(_connection);

            SqliteCommand insertCommand = new(@"
                INSERT INTO ProfileConsumedProducts (ProfileID, ProductID, ProductMass, ConsumedDate)
                VALUES (@ProfileID, @ProductID, @ProductMass, @ConsumedDate);", _connection);
            insertCommand.Parameters.AddWithValue("@ProfileID", profile.Id);
            insertCommand.Parameters.AddWithValue("@ProductID", product.Id);
            insertCommand.Parameters.AddWithValue("@ProductMass", product.NetMassInGrams);
            insertCommand.Parameters.AddWithValue("@ConsumedDate", date.ToString("yyyy-MM-dd"));
            insertCommand.ExecuteNonQuery();
        }
    }
}

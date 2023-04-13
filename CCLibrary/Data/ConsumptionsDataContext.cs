using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using CCLibrary.User;
using CCLibrary.Products;
using CCLibrary.Exceptions;

#pragma warning disable CS8604

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

            Data.Database.ReopenConnection(_connection);

            SqliteCommand selectCommand = new(@"
                SELECT ProductID, ProductMass
                FROM ProfileConsumedProducts
                INNER JOIN Products ON Products.Id = ProfileConsumedProducts.ProductID
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

            _connection.Close();

            return consumption;
        }

        /// <summary>
        /// Insert a record to the db for a profile consumed product.
        /// </summary>
        public void AddProfileConsumption(Profile profile, Product product, DateTime date)
        {
            Data.Database.ReopenConnection(_connection);

            SqliteCommand insertCommand = new(@"
                INSERT INTO ProfileConsumedProducts (ProfileID, ProductID, ProductMass, ConsumedDate)
                VALUES (@ProfileID, @ProductID, @ProductMass, @ConsumedDate);", _connection);
            insertCommand.Parameters.AddWithValue("@ProfileID", profile.Id);
            insertCommand.Parameters.AddWithValue("@ProductID", product.Id);
            insertCommand.Parameters.AddWithValue("@ProductMass", product.NetMassInGrams);
            insertCommand.Parameters.AddWithValue("@ConsumedDate", date.ToString("yyyy-MM-dd"));
            insertCommand.ExecuteNonQuery();

            _connection.Close();
        }

        /// <summary>
        /// Delete a record from the db for a profile.
        /// </summary>
        public void RemoveProfileConsumption(Profile profile, Product product, DateTime date)
        {
            Data.Database.ReopenConnection(_connection);

            SqliteCommand deleteCommand = new(@"
                DELETE FROM ProfileConsumedProducts
                WHERE ProfileID=@ProfileID AND ProductID=@ProductID
                    AND ProductMass=@ProductMass AND ConsumedDate=@ConsumedDate;", _connection);
            deleteCommand.Parameters.AddWithValue("@ProfileID", profile.Id);
            deleteCommand.Parameters.AddWithValue("@ProductID", product.Id);
            deleteCommand.Parameters.AddWithValue("@ProductMass", product.NetMassInGrams);
            deleteCommand.Parameters.AddWithValue("@ConsumedDate", date.ToString("yyyy-MM-dd"));
            deleteCommand.ExecuteNonQuery();

            _connection.Close();
        }

        /// <summary>
        /// Checks if the products is already in use by any profile.
        /// </summary>
        public bool IsProductUsed(Product product)
        {
            Data.Database.ReopenConnection(_connection);

            SqliteCommand selectCommand = new(@"
                SELECT COUNT(ProductID) FROM ProfileConsumedProducts
                WHERE ProductID=@ProductID;", _connection);
            selectCommand.Parameters.AddWithValue("@ProductID", product.Id);
            int count = Convert.ToInt32(selectCommand.ExecuteScalar());

            _connection.Close();

            return count > 0;
        }
    }
}

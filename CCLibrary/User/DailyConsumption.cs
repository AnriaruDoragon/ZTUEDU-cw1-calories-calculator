using System;
using System.Linq;
using System.Collections.Generic;
using CCLibrary.Products;

namespace CCLibrary.User
{
    public class DailyConsumption
    {
        private List<Product> _consumedProducts;

        public Profile LinkedProfile { get; }
        public DateTime Date { get; }
        
        public DailyConsumption(Profile profile, DateTime date)
        {
            LinkedProfile = profile;
            Date = date;
            _consumedProducts = new List<Product>();
        }

        /// <summary>
        /// Calculate the total amount of consumed calories.
        /// </summary>
        public double CalculateCalories()
        {
            return _consumedProducts.Select(product => product.GetCalories()).Sum();
        }

        /// <summary>
        /// Calculate the total amount of consumed energy.
        /// </summary>
        public double CalculateEnergy()
        {
            return CalculateCalories() * 4.184;
        }

        /// <summary>
        /// Get a copy of a list of consumed products.
        /// </summary>
        public List<Product> GetProducts()
        {
            return new List<Product>(_consumedProducts);
        }

        /// <summary>
        /// Add product to consumed list.
        /// </summary>
        public void Consume(Product product)
        {
            _consumedProducts.Add(product);
        }

        /// <summary>
        /// Set a new list of consumed products.
        /// </summary>
        public void UpdateConsumed(List<Product> products)
        {
            _consumedProducts = new List<Product>(products);
        }
    }
}

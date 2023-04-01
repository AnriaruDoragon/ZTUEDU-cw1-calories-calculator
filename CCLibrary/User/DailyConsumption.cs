using System;
using System.Linq;
using System.Collections.Generic;
using CCLibrary.Products;

namespace CCLibrary.User
{
    public class DailyConsumption
    {
        private List<Product> _products;

        public Profile Profile { get; }
        public DateTime Date { get; }
        
        public DailyConsumption(Profile profile, DateTime date)
        {
            Profile = profile;
            Date = date;
            _products = new List<Product>();
        }

        /// <summary>
        /// Calculate the total amount of consumed calories.
        /// </summary>
        public double CalculateCalories()
        {
            return _products.Select(product => product.GetCalories()).Sum();
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
            return new List<Product>(_products);
        }

        /// <summary>
        /// Add product to consumed list.
        /// </summary>
        public void Consume(Product product)
        {
            _products.Add(product);
        }

        /// <summary>
        /// Set a new list of consumed products.
        /// </summary>
        public void UpdateConsumed(List<Product> products)
        {
            _products = new List<Product>(products);
        }
    }
}

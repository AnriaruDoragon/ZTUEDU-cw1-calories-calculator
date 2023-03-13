using System;
using System.Collections.Generic;
using CCLibrary.Exceptions;

namespace CCLibrary.Products
{
    public class EnergyDrink : Drink
    {
        protected Dictionary<string, double> _vitamins;

        public EnergyDrink(string name, double caloriesPerServing, Dictionary<string, double>? vitamins = null)
            : base(name, caloriesPerServing)
        {
            Type = DrinkTypes.Energy;

            if (vitamins == null)
                _vitamins = new Dictionary<string, double>();
            else
                _vitamins = new Dictionary<string, double>(vitamins);
        }

        /// <summary>
        /// Get dictionary of vitamins for this product.
        /// </summary>
        public Dictionary<string, double> GetVitamins()
        {
            return new Dictionary<string, double>(_vitamins);
        }

        /// <summary>
        /// Get dictionary of total amount of vitamins for this product.
        /// </summary>
        public Dictionary<string, double> GetVitaminsTotal()
        {
            Dictionary<string, double> _total = new(_vitamins);
            foreach (var vitamin in _total)
                _total[vitamin.Key] = vitamin.Value * _netMass / _servingSize;
            return _total;
        }

        /// <summary>
        /// Add vitamins to this product.
        /// </summary>
        /// <param name="name">Vitamin name.</param>
        /// <param name="value">Amount of the vitamin in mg for _servingSize g.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void AddVitamin(string name, double value)
        {
            if (value < 0)
                throw new ValueOutOfRangeException("Значення повинно бути додатнім!");
            if (_vitamins.ContainsKey(name))
                throw new VitaminAlreadyExistsException();
            _vitamins.Add(name, value);
        }

        /// <summary>
        /// Remove vitamins from this product.
        /// </summary>
        /// <param name="name">Vitamin name.</param>
        /// <exception cref="ArgumentException"></exception>
        public void RemoveVitamin(string name)
        {
            if (!_vitamins.ContainsKey(name))
                throw new VitaminNotFoundException();
            _vitamins.Remove(name);
        }
    }
}

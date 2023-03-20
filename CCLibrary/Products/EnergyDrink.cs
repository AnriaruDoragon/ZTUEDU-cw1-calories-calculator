using System;
using System.Collections.Generic;
using CCLibrary.Exceptions;

namespace CCLibrary.Products
{
    public class EnergyDrink : Drink
    {
        protected EnergyDrink(IDictionary<string, double> vitamins)
        {
            _vitamins = new Dictionary<string, double>(vitamins);
        }

        public EnergyDrink(string name, double caloriesPerServing, IDictionary<string, double>? vitamins = null)
            : base(name, caloriesPerServing)
        {
            Type = DrinkTypes.Energy;

            _vitamins = vitamins == null
                ? new Dictionary<string, double>() 
                : new Dictionary<string, double>(vitamins);
        }

        protected Dictionary<string, double> _vitamins;

        public override Product Copy()
        {
            return new EnergyDrink(this._vitamins)
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                NetMassInGrams = this.NetMassInGrams,
                CaloriesPerServing = this.CaloriesPerServing,
                ServingSizeInGrams = this.ServingSizeInGrams,
                Type = this.Type,
                IsCarbonated = this.IsCarbonated
            };
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
            Dictionary<string, double> total = new(_vitamins);
            foreach (var vitamin in total)
                total[vitamin.Key] = vitamin.Value * _netMass / _servingSize;
            return total;
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

using System;

namespace CCLibrary
{
    public class Consumable : Product
    {
        protected double _caloriesPerServing;
        protected double _servingSize = 100;

        public Consumable(string name, double caloriesPerServing) : base(name)
        {
            CaloriesPerServing = caloriesPerServing;
        }

        /// <summary>
        /// Specifies the amount of calories per serving size (g).
        /// </summary>
        public double CaloriesPerServing
        {
            get { return _caloriesPerServing; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(value),
                        "The value must be a positive number.");
                else
                    _caloriesPerServing = value;
            }
        }

        /// <summary>
        /// <para>The declared serving size for calories calculations in grams.</para>
        /// <para>Specifies the mass that contains N amount of calories.</para>
        /// By default set to 100g.
        /// </summary>
        public double ServingSizeInGrams
        {
            get { return _servingSize; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(value),
                        "The value must be a positive number.");
                else
                    _servingSize = value;
            }
        }

        public override double GetCalories()
        {
            return _caloriesPerServing * _netMass / _servingSize;
        }
    }
}

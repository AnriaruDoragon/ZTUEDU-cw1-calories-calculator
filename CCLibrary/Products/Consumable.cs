using CCLibrary.Exceptions;

namespace CCLibrary.Products
{
    public class Consumable : Product
    {
        protected Consumable()
        {
        }

        public Consumable(string name, double caloriesPerServing) : base(name)
        {
            CaloriesPerServing = caloriesPerServing;
        }

        protected double _caloriesPerServing;
        protected double _servingSize = 100;

        /// <summary>
        /// Specifies the amount of calories per serving size (g).
        /// </summary>
        public double CaloriesPerServing
        {
            get { return _caloriesPerServing; }
            set
            {
                if (value <= 0)
                    throw new ValueOutOfRangeException("Значення повинно бути додатнім!");
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
                    throw new ValueOutOfRangeException("Значення повинно бути додатнім!");
                else
                    _servingSize = value;
            }
        }

        public override Product Copy()
        {
            return new Consumable
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                NetMassInGrams = this.NetMassInGrams,
                CaloriesPerServing = this.CaloriesPerServing,
                ServingSizeInGrams = this.ServingSizeInGrams
            };
        }

        public override double GetCalories()
        {
            return _caloriesPerServing * _netMass / _servingSize;
        }
    }
}

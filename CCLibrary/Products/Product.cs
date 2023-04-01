using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CCLibrary.Exceptions;

namespace CCLibrary.Products
{
    [Table("Products")]
    public abstract class Product
    {
        protected double _netMass = 100;

        [Key]
        public long Id { get; protected set; }
        public string Name { get; set; } = "N/D";
        public string Description { get; set; } = string.Empty;

        protected Product() { }

        protected Product(string name)
        {
            Name = name;
        }

        public double NetMassInGrams
        {
            get => _netMass;
            set
            {
                if (value < 0)
                    throw new ValueOutOfRangeException("Значення повинно бути додатнім!");
                _netMass = value;
            }
        }

        public double NetMassInKilos
        {
            get => _netMass / 1000.0;
            set
            {
                value *= 1000.0;
                if (value < 0)
                    throw new ValueOutOfRangeException("Значення повинно бути додатнім!");
                _netMass = value;
            }
        }

        public double NetMassInPounds
        {
            get => _netMass / 453.6;
            set
            {
                value *= 453.6;
                if (value < 0)
                    throw new ValueOutOfRangeException("Значення повинно бути додатнім!");
                _netMass = value;
            }
        }

        /// <summary>
        /// Get a copy of this product.
        /// </summary>
        public abstract Product Copy();

        /// <summary>
        /// Get the amount of calories for this product.
        /// </summary>
        public abstract double GetCalories();

        /// <summary>
        /// Get the amount of energy in joules for this product.
        /// </summary>
        public double GetEnergy()
        {
            return GetCalories() * 4.184;
        }

        public override int GetHashCode()
        {
            return (int)Id;
        }
    }
}

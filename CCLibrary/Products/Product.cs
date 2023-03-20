using System;
using CCLibrary.Exceptions;

namespace CCLibrary.Products
{
    public abstract class Product : ICloneable
    {
        protected Product() 
        {
        }

        protected Product(string name)
        {
            Name = name;
        }

        public long Id { get; set; }
        public string Name { get; set; } = "N/D";
        public string Description { get; set; } = string.Empty;
        protected double _netMass = 100;

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
        public object Clone()
        {
            return MemberwiseClone();
        }

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

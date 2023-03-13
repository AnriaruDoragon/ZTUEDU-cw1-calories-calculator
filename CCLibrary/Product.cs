namespace CCLibrary
{
    public abstract class Product
    {
        public ulong Id { get; internal set; } = 0;

        public string Name { get; set; } = "N/D";
        public string Description { get; set; } = string.Empty;

        protected double _netMass = 100;
        public double NetMassInGrams
        {
            get { return _netMass; }
            protected set
            {
                if (value < 0)
                    throw new Exceptions.ValueOutOfRangeException(
                        "Значення повинно бути додатнім!");
                else
                    _netMass = value;
            }
        }
        public double NetMassInKilos
        {
            get { return _netMass / 1000.0; }
            protected set
            {
                value *= 1000.0;
                if (value < 0)
                    throw new Exceptions.ValueOutOfRangeException(
                        "Значення повинно бути додатнім!");
                else
                    _netMass = value;
            }
        }
        public double NetMassInPounds
        {
            get { return _netMass / 453.6; }
            set
            {
                value *= 453.6;
                if (value < 0)
                    throw new Exceptions.ValueOutOfRangeException(
                        "Значення повинно бути додатнім!");
                else
                    _netMass = value;
            }
        }

        public Product(string name)
        {
            Name = name;
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
            return (int)this.Id;
        }
    }
}

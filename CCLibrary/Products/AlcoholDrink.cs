namespace CCLibrary.Products
{
    public class AlcoholDrink : Drink
    {
        protected double _alcoholContent;

        /// <summary>
        /// The amount of alcohol in this product in percentage.
        /// </summary>
        public double AlcoholContent
        {
            get { return _alcoholContent; }
            set
            {
                if (value >= 0 && value <= 100)
                    _alcoholContent = value;
                else
                    throw new Exceptions.ValueOutOfRangeException(
                        "Значення повинно бути у діапазоні від 0 до 100!");
            }
        }

        public AlcoholDrink(string name, double caloriesPerServing, double alcoholContent)
            : base(name, caloriesPerServing)
        {
            Type = DrinkTypes.Alcohol;
            AlcoholContent = alcoholContent;
        }
    }
}

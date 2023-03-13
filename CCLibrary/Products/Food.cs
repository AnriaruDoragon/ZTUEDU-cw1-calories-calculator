namespace CCLibrary.Products
{
    public class Food : Consumable
    {
        protected FoodTypes _foodType;
        public Nutritions Nutritions { get; protected set; }

        public FoodTypes Type
        {
            get { return _foodType; }
            protected set { _foodType = value; }
        }

        public Food(string name, double caloriesPerServing, Nutritions nutritions = new())
            : base(name, caloriesPerServing)
        {
            Nutritions = nutritions;
        }

        /// <summary>
        /// Set new Nutrition facts for this product.
        /// </summary>
        public void UpdateNutritions(Nutritions nutritions)
        {
            Nutritions = nutritions;
        }
    }
}

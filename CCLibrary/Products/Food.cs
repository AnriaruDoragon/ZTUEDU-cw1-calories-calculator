namespace CCLibrary.Products
{
    public class Food : Consumable
    {
        protected Food()
        {
        }

        public Food(string name, double caloriesPerServing, Nutritions nutritions = new())
            : base(name, caloriesPerServing)
        {
            Nutritions = nutritions;
        }

        protected FoodTypes _foodType;
        public Nutritions Nutritions { get; protected set; }

        public FoodTypes Type
        {
            get { return _foodType; }
            protected set { _foodType = value; }
        }

        public override Product Copy()
        {
            return new Food
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                NetMassInGrams = this.NetMassInGrams,
                CaloriesPerServing = this.CaloriesPerServing,
                ServingSizeInGrams = this.ServingSizeInGrams,
                Type = this.Type,
                Nutritions = this.Nutritions
            };
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

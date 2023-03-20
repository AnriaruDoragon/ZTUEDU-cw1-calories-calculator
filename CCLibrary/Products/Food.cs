namespace CCLibrary.Products
{
    public class Food : Consumable
    {
        protected Food()
        {
        }

        public Food(string name, double caloriesPerServing, Nutrition nutrition = new())
            : base(name, caloriesPerServing)
        {
            Nutrition = nutrition;
        }

        protected FoodTypes _foodType;
        public Nutrition Nutrition { get; protected set; }

        public FoodTypes Type
        {
            get => _foodType;
            protected set => _foodType = value;
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
                Nutrition = this.Nutrition
            };
        }

        /// <summary>
        /// Set new Nutrition facts for this product.
        /// </summary>
        public void UpdateNutrition(Nutrition nutrition)
        {
            Nutrition = nutrition;
        }
    }
}

namespace CCLibrary
{
    internal readonly struct Nutritions
    {
        public readonly double Fat;
        public readonly double Protein;
        public readonly double Carbs;
        public readonly double Fiber;
        public readonly double Sodium;

        public Nutritions(double fat = 0, double protein = 0, double carbs = 0,
            double fiber = 0, double sodium = 0)
        {
            Fat = fat;
            Protein = protein;
            Carbs = carbs;
            Fiber = fiber;
            Sodium = sodium;
        }
    }

    internal class Food : Consumable
    {
        public Nutritions Nutritions { get; protected set; }

        public Food(ulong id, double caloriesPerServing, Nutritions nutritions = new())
            : base(id, caloriesPerServing)
        {
            Nutritions = nutritions;
        }

        public Food(ulong id, string name, double caloriesPerServing, Nutritions nutritions = new())
            : base(id, name, caloriesPerServing)
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

namespace CCLibrary.Products
{
    public readonly struct Nutritions
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
}

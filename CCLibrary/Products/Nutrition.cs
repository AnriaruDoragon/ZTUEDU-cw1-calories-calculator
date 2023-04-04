namespace CCLibrary.Products
{
    public struct Nutrition
    {
        public double Fat { get; private set; }
        public double Protein { get; private set; }
        public double Carbs { get; private set; }
        public double Fiber { get; private set; }
        public double Sodium { get; private set; }

        public Nutrition(double fat = 0, double protein = 0, double carbs = 0,
            double fiber = 0, double sodium = 0)
        {
            Fat = fat;
            Protein = protein;
            Carbs = carbs;
            Fiber = fiber;
            Sodium = sodium;
        }

        public static Nutrition operator +(Nutrition a, Nutrition b)
        {
            return new Nutrition(a.Fat + b.Fat, a.Protein + b.Protein, a.Carbs + b.Carbs, a.Fiber + b.Fiber, a.Sodium + b.Sodium);
        }
    }
}

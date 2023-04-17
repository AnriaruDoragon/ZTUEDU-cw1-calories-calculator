namespace CCLibrary.Products;

public struct Nutrition
{
    public double Fat { get; set; }
    public double Protein { get; set; }
    public double Carbs { get; set; }
    public double Fiber { get; set; }
    public double Sodium { get; set; }

    public Nutrition(double fat = 0, double protein = 0, double carbs = 0, double fiber = 0, double sodium = 0)
    {
        Fat = fat;
        Protein = protein;
        Carbs = carbs;
        Fiber = fiber;
        Sodium = sodium;
    }

    public static Nutrition operator *(Nutrition a, double b)
        => new(a.Fat * b, a.Protein * b, a.Carbs * b, a.Fiber * b, a.Sodium * b);

    public static Nutrition operator /(Nutrition a, double b)
        => new(a.Fat / b, a.Protein / b, a.Carbs / b, a.Fiber / b, a.Sodium / b);

    public static Nutrition operator +(Nutrition a, Nutrition b)
        => new(a.Fat + b.Fat, a.Protein + b.Protein, a.Carbs + b.Carbs, a.Fiber + b.Fiber, a.Sodium + b.Sodium);
}

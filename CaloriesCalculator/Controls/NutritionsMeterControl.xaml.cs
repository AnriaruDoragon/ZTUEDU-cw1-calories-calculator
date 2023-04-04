using System.Windows;
using System.Windows.Controls;
using CCLibrary.Products;

namespace CaloriesCalculator.Controls
{
    public partial class NutritionsMeterControl : UserControl
    {
        public NutritionsMeterControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty NutritionProperty = 
            DependencyProperty.Register(nameof(Nutrition), typeof(Nutrition), typeof(NutritionsMeterControl),
                new PropertyMetadata(new Nutrition()));

        public Nutrition Nutrition
        {
            get => (Nutrition)GetValue(NutritionProperty);
            set => SetValue(NutritionProperty, value);
        }
    }
}

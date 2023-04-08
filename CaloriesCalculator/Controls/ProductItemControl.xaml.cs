using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CCLibrary.Products;

namespace CaloriesCalculator.Controls
{
    public partial class ProductItemControl : UserControl
    {
        private readonly List<string> _icons = new()
        {
            "Fruit", "Vegetable", "Meat", "Fish", "Baking", "Cereals",
            "Energy", "Alcohol", "Juice", "Coffee", "Tea", "Milk"
        };

        public ProductItemControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ProductProperty = 
            DependencyProperty.Register(nameof(Product), typeof(Product), typeof(ProductItemControl));

        public Product Product
        {
            get => (Product)GetValue(ProductProperty);
            set => SetValue(ProductProperty, value);
        }

        private void SetIcon()
        {
            string iconString = "ProductDrawingImage";

            if (Product is Food food)
            {
                iconString = _icons.Contains(food.Type.ToString())
                    ? $"{food.Type}DrawingImage"
                    : "ProductDrawingImage";
            }
            else if (Product is Drink drink)
            {
                iconString = _icons.Contains(drink.Type.ToString())
                    ? $"{drink.Type}DrawingImage"
                    : "DrinkDrawingImage";
            }
            else if (Product is Dish)
                iconString = "DishDrawingImage";

            ProductImage.Source = FindResource(iconString) as DrawingImage;
        }

        private void SetInformation()
        {
            if (Product is null)
                return;

            SetIcon();

            CaloriesAmountTextBlock.Text = $"{Product.GetCalories()} кк";
        }
    }
}

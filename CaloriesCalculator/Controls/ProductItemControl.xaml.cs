using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
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

            SetInformation();
        }

        public static readonly DependencyProperty ProductProperty = 
            DependencyProperty.Register(nameof(Product), typeof(Product), typeof(ProductItemControl),
                new PropertyMetadata(null, ProductChangedCallback));

        private static void ProductChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is null)
                return;

            ((ProductItemControl)d).SetInformation();
        }

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

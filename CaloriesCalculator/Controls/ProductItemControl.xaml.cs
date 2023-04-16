using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using CCLibrary.Products;
using CaloriesCalculator.Pages;

namespace CaloriesCalculator.Controls
{
    public partial class ProductItemControl : UserControl
    {
        private static readonly List<string> _icons = new()
        {
            "Fruit", "Vegetable", "Meat", "Fish", "Baking", "Cereals",
            "Energy", "Alcohol", "Juice", "Coffee", "Tea", "Milk"
        };

        public event EventHandler? LeftClick;
        public event EventHandler? RightClick;

        public static readonly DependencyProperty ProductProperty = 
            DependencyProperty.Register(nameof(Product), typeof(Product), typeof(ProductItemControl),
                new PropertyMetadata(null, ProductChangedCallback));

        public ProductItemControl() => InitializeComponent();

        private void ProductItem_Loaded(object sender, RoutedEventArgs e)
            => SetInformation();

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

        /// <summary>
        /// Returns a DrawingImage of an Icon if it's in resources and specified.
        /// </summary>
        public static DrawingImage GetIconImage(Product product)
        {
            string iconString = "ProductDrawingImage";

            if (product is Food food)
            {
                iconString = _icons.Contains(food.Type.ToString())
                    ? $"{food.Type}DrawingImage"
                    : "ProductDrawingImage";
            }
            else if (product is Drink drink)
            {
                iconString = _icons.Contains(drink.Type.ToString())
                    ? $"{drink.Type}DrawingImage"
                    : "DrinkDrawingImage";
            }

            return Application.Current.Resources[iconString] as DrawingImage;
        }

        public void UpdateValues()
        {
            DefaultSizeTextBlock.Text = $"{Product.NetMassInGrams:0.##} г";
            CaloriesAmountTextBlock.Text = $"{Product.GetCalories():0.##} кк";
        }

        private void SetInformation()
        {
            if (Product is null)
                return;

            ProductImage.Source = GetIconImage(Product);

            UpdateValues();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
            => LeftClick?.Invoke(this, e);

        private void Button_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
            => RightClick?.Invoke(this, e);
    }
}

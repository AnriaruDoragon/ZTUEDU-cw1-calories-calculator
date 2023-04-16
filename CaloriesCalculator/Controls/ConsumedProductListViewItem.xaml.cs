using System;
using System.Windows;
using System.Windows.Controls;
using CCLibrary.Products;

namespace CaloriesCalculator.Controls
{
    public partial class ConsumedProductListViewItem : UserControl
    {
        public event EventHandler? Deleted;

        public static readonly DependencyProperty ProductProperty = 
            DependencyProperty.Register(nameof(Product), typeof(Product), typeof(ConsumedProductListViewItem),
                new PropertyMetadata(null));

        public ConsumedProductListViewItem() => InitializeComponent();

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            IconImage.Source = ProductItemControl.GetIconImage(Product);
            NameTextBlock.Text = Product.Name;
            MassTextBlock.Text = $"{Product.NetMassInGrams:0.##} г";
        }

        public Product Product
        {
            get => (Product)GetValue(ProductProperty);
            set => SetValue(ProductProperty, value);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
            => Deleted?.Invoke(Product, e);
    }
}

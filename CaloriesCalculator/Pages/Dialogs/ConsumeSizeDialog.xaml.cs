using System.Windows;
using System.Windows.Controls;
using CaloriesCalculator.Controls;
using CCLibrary.Products;

namespace CaloriesCalculator.Pages.Dialogs
{
    public partial class ConsumeSizeDialog : Window
    {
        public Product Product { get; set; }

        public ConsumeSizeDialog(Product product)
        {
            Product = product.Copy();

            InitializeComponent();

            SizeSlider.Minimum = Product.NetMassInGrams / 10;
            SizeSlider.Maximum = Product.NetMassInGrams * 2;
        }

        private void ProductControl_Loaded(object sender, RoutedEventArgs e)
        {
            ProductItemControl itemControl = (ProductItemControl)sender;
            itemControl.Product = Product;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
            => DialogResult = true;

        private void CancelButton_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;

        private void SizeTextBox_TextChanged(object sender, TextChangedEventArgs e)
            => SelectedProduct.UpdateValues();
    }
}

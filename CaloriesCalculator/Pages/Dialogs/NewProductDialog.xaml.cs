using System.Windows;
using CCLibrary.Products;

namespace CaloriesCalculator.Pages.Dialogs;

public partial class NewProductDialog : Window
{
    public Product Product { get; set; }

    public NewProductDialog() => InitializeComponent();

    private void CancelButton_Click(object sender, RoutedEventArgs e)
        => DialogResult = false;

    private void FoodButton_Click(object sender, RoutedEventArgs e)
    {
        Product = new Food("Їжа", 0);

        DialogResult = true;
    }

    private void DrinkButton_Click(object sender, RoutedEventArgs e)
    {
        Product = new Drink("Напій", 0);

        DialogResult = true;
    }
}

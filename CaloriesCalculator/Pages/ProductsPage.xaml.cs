using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using CaloriesCalculator.Controls;
using CCLibrary.Data;
using CCLibrary.Products;

#pragma warning disable CS8604, CS8618

namespace CaloriesCalculator.Pages;

public partial class ProductsPage : Page
{
    private IEnumerable<Product> _products;

    public ProductsPage()
    {
        App.Database.Context.Products.Load();
        _products = App.Database.Context.Products.ToList();
        
        InitializeComponent();
    }

    private void ProductItems_Loaded(object sender, RoutedEventArgs e)
    {
        ((ItemsControl)sender).ItemsSource = GetSortedProducts(GetSortingMethod());
        _ = EnsureProfileNotNull();
    }

    private bool EnsureProfileNotNull()
    {
        if (App.CurrentProfile is null)
        {
            ProductItems.IsEnabled = false;
            MessageBox.Show("Для початку увійдіть в ваш профіль!",
                "Помилка", MessageBoxButton.OK, MessageBoxImage.Information);
            return false;
        }
        ProductItems.IsEnabled = true;
        return true;
    }

    private void UpdateProductsList()
    {
        App.Database.Context.Products.Load();
        _products = App.Database.Context.Products.ToList();
        SetSearchedProducts();
    }

    #region Sorting and searching
    private enum SortingMethods
    {
        AlphabetA = 1,
        AlphabetZ = 2,
        Ascending = 3,
        Descending = 4,
        ByTypeA = 5,
        ByTypeZ = 6
    }

    private SortingMethods GetSortingMethod()
    {
        ComboBoxItem item = (ComboBoxItem)SortingComboBox.SelectedItem;
        return (SortingMethods)int.Parse(item.Tag.ToString());
    }

    private IEnumerable<Product> FilterProducts(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return _products;

        Regex regex = new(query, RegexOptions.IgnoreCase);
        return _products.Where(p => regex.IsMatch(p.Name) || regex.IsMatch(p.Description));
    }

    private class ProductTypeComparer : IComparer<Product?>
    {
        public int Compare(Product? a, Product? b)
        {
            if (a is Food && b is Drink) return -1; // Food first
            if (a is Drink && b is Food) return 1;  // Drinks last
            // If same, compare types
            var aType = a is Food ? (int)((Food)a).Type : a is Drink ? (int)((Drink)a).Type : 0;
            var bType = b is Food ? (int)((Food)b).Type : b is Drink ? (int)((Drink)b).Type : 0;
            return aType.CompareTo(bType);
        }
    }

    private IEnumerable<Product> GetSortedProducts(SortingMethods method, IEnumerable<Product>? products = null)
    {
        products ??= _products;
        IEnumerable<Product> sortedProducts = method switch
        {
            SortingMethods.AlphabetZ => products.OrderByDescending(p => p.Name),
            SortingMethods.Ascending => products.OrderBy(p => p.GetCalories()),
            SortingMethods.Descending => products.OrderByDescending(p => p.GetCalories()),
            SortingMethods.ByTypeA => products.OrderBy(p => p, new ProductTypeComparer()),
            SortingMethods.ByTypeZ => products.OrderByDescending(p => p, new ProductTypeComparer()),
            _ => products.OrderBy(p => p.Name)
        };
        return sortedProducts;
    }

    private void SetSearchedProducts()
    {
        if (ProductItems is not null)
            ProductItems.ItemsSource = GetSortedProducts(GetSortingMethod(), FilterProducts(SearchBar.Text));
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        => SetSearchedProducts();

    private void SortingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        => SetSearchedProducts();
    #endregion

    #region Products controls
    private void NewProductButton_Click(object sender, RoutedEventArgs e)
    {
        Dialogs.NewProductDialog newDialog = new();
        if (newDialog.ShowDialog() != true)
            return;

        Dialogs.EditProductDialog editDialog = new(newDialog.Product, copy:false);
        if (editDialog.ShowDialog() != true)
            return;

        App.Database.Context.AddProduct(editDialog.Product);
        UpdateProductsList();

        MessageBox.Show($"Продукт \"{editDialog.Product.Name}\" було успішно створено.",
            "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void Product_LeftClick(object sender, EventArgs e)
    {
        if (!EnsureProfileNotNull())
            return;

        Product clickedProduct = ((ProductItemControl)sender).Product;

        if (Settings.Get("AskForSize", true))
        {
            Dialogs.ConsumeSizeDialog sizeDialog = new(clickedProduct);
            if (sizeDialog.ShowDialog() != true)
                return;
            clickedProduct = sizeDialog.Product;
        }

        if (clickedProduct.NetMassInGrams <= 0)
        {
            MessageBox.Show("Вкажіть значення більше нуля!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        
        App.Database.Context.AddProfileConsumption(App.CurrentProfile, clickedProduct, App.SelectedDate);
        ((MainWindow)Window.GetWindow(this)).UpdateCalorieMeter();
    }

    // Context menu
    private ProductItemControl _targetedProductControl;
    private void Product_RightClick(object sender, EventArgs e)
    {
        if (ProductItems.FindResource("ProductContext") is not ContextMenu productContext)
            return;

        _targetedProductControl = (ProductItemControl)sender;
        productContext.PlacementTarget = _targetedProductControl;
        productContext.IsOpen = true;
    }

    private void ProductContextConsume_Click(object sender, RoutedEventArgs e)
    {
        if (_targetedProductControl is not null)
            Product_LeftClick(_targetedProductControl, e);
    }

    private void ProductContextEdit_Click(object sender, RoutedEventArgs e)
    {
        if (_targetedProductControl is null)
            return;

        Product clickedProduct = _targetedProductControl.Product;
        bool isUsed = App.Database.Context.IsProductUsed(clickedProduct);
        if (isUsed)
            MessageBox.Show("Цей продукт вже використовується!\nЗміна калорійності, порції та інших показників вплине на результат днів, коли споживався цей продукт.",
                "Зверніть увагу!", MessageBoxButton.OK, MessageBoxImage.Exclamation);

        Dialogs.EditProductDialog editDialog = new(clickedProduct);
        if (editDialog.ShowDialog() != true)
            return;

        App.Database.Context.ModifyProduct(editDialog.Product);
        UpdateProductsList();
        if (isUsed)
            ((MainWindow)Window.GetWindow(this)).UpdateCalorieMeter();

        MessageBox.Show($"Продукт \"{clickedProduct.Name}\" було успішно оновлено.{(isUsed ? "\nЦі зміни можуть вплинути на результат деяких днів." : "")}",
            "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void ProductContextDelete_Click(object sender, RoutedEventArgs e)
    {
        if (_targetedProductControl is null)
            return;

        Product clickedProduct = _targetedProductControl.Product;
        bool isUsed = App.Database.Context.IsProductUsed(clickedProduct);
        if (isUsed)
            MessageBox.Show("Цей продукт вже використовується!\nЙого видалення призведе до втрати інформації та неточного результату у дні, коли його споживали.",
                "Зверніть увагу!", MessageBoxButton.OK, MessageBoxImage.Exclamation);

        if (MessageBox.Show($"Ви насправді хочете видалити \"{clickedProduct.Name}\"?", "Увага!", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            return;

        App.Database.Context.DeleteProduct(clickedProduct.Id);
        UpdateProductsList();
        if (isUsed)
            ((MainWindow)Window.GetWindow(this)).UpdateCalorieMeter();

        MessageBox.Show($"Продукт \"{clickedProduct.Name}\" було успішно видалено.{(isUsed ? "\nЦе видалення вплине на результат деяких днів." : "")}",
            "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
    }
    #endregion
}

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CCLibrary.User;
using CCLibrary.Products;

#pragma warning disable CS8604, CS8618

namespace CaloriesCalculator.Pages;

public partial class ConsumptionHistoryPage : Page
{
    private List<Product> _products;
    private DailyConsumption _consumption;

    public ConsumptionHistoryPage() => InitializeComponent();

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        ((MainWindow)Window.GetWindow(this)).DateChanged += Meter_DateChanged;
        UpdateHistoryView();
    }

    private void Meter_DateChanged(object? sender, EventArgs e)
    {
        if (!EnsureProfileNotNull(message:false))
            return;

        UpdateHistoryView();
    }

    private void UpdateDisplayedGrid()
    {
        if (App.CurrentProfile is null)
        {
            ConsumptionViewer.Visibility = Visibility.Collapsed;
            NoProfileTextBlock.Visibility = Visibility.Visible;
        }
        else
        {
            NoProfileTextBlock.Visibility = Visibility.Collapsed;
            ConsumptionViewer.Visibility = Visibility.Visible;
        }
    }

    private bool EnsureProfileNotNull(bool message = true)
    {
        if (App.CurrentProfile is null)
        {
            if (message)
                MessageBox.Show("Для початку увійдіть в ваш профіль!",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Information);
            UpdateDisplayedGrid();
            return false;
        }
        return true;
    }

    private void UpdateHistoryView()
    {
        if (!EnsureProfileNotNull())
            return;

        _consumption = App.Database.Context.GetProfileConsumption(App.CurrentProfile, App.SelectedDate);
        _products = _consumption.GetProducts();
        _products.Reverse();
        ConsumeHistoryListView.ItemsSource = _products;
    }

    private void ConsumedProduct_Deleted(object sender, EventArgs e)
    {
        if (!EnsureProfileNotNull())
            return;

        var listProduct = (Product)sender;

        try
        {
            App.Database.Context.RemoveProfileConsumption(App.CurrentProfile, listProduct, _consumption.Date);
            _products.Remove(listProduct);
        }
        catch
        {
            // ignored
        }

        ConsumeHistoryListView.Items.Refresh();
        ((MainWindow)Window.GetWindow(this)).UpdateCalorieMeter();
    }
}

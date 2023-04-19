using System;
using System.Windows;
using System.Windows.Controls;
using CaloriesCalculator.Resources;
using CCLibrary.Products;

namespace CaloriesCalculator.Controls;

public partial class ProductItemControl : UserControl
{
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

    public void UpdateValues()
    {
        DefaultSizeTextBlock.Text = $"{Product.NetMassInGrams:0.##} г";
        CaloriesAmountTextBlock.Text = $"{Product.GetCalories():0.##} кк";
    }

    private void SetInformation()
    {
        if (Product is null)
            return;

        ProductImage.Source = Icons.GetProductIconImage(Product);

        UpdateValues();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
        => LeftClick?.Invoke(this, e);

    private void Button_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        => RightClick?.Invoke(this, e);
}

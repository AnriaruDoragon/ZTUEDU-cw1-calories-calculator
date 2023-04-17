using System;
using System.Windows;
using System.Windows.Controls;

namespace CaloriesCalculator.Controls;

public partial class CaloriesProgressBarControl : UserControl
{
    public static readonly DependencyProperty ValueProperty = 
        DependencyProperty.Register(nameof(Value), typeof(double), typeof(CaloriesProgressBarControl),
            new PropertyMetadata(0D));

    public static readonly DependencyProperty MaxProperty = 
        DependencyProperty.Register(nameof(Max), typeof(double), typeof(CaloriesProgressBarControl),
            new PropertyMetadata(0D));

    public CaloriesProgressBarControl() => InitializeComponent();

    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, Math.Round(value, 0));
    }

    public double Max
    {
        get => (double)GetValue(MaxProperty);
        set => SetValue(MaxProperty, Math.Round(value, 0));
    }
}

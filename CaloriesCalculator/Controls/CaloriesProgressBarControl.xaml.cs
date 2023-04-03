using System.Windows;
using System.Windows.Controls;

namespace CaloriesCalculator.Controls
{
    public partial class CaloriesProgressBarControl : UserControl
    {
        public CaloriesProgressBarControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ValueProperty = 
            DependencyProperty.Register(nameof(Value), typeof(double), typeof(CaloriesProgressBarControl),
                new PropertyMetadata(0));

        public static readonly DependencyProperty MaxProperty = 
            DependencyProperty.Register(nameof(Max), typeof(double), typeof(CaloriesProgressBarControl),
                new PropertyMetadata(1));

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public double Max
        {
            get => (double)GetValue(MaxProperty);
            set => SetValue(MaxProperty, value);
        }
    }
}

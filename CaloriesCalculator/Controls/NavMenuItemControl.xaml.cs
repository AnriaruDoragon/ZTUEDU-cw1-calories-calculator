using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CaloriesCalculator.Controls
{
    public partial class NavMenuItemControl : UserControl
    {
        public NavMenuItemControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TextProperty = 
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(NavMenuItemControl),
                new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty IconProperty = 
            DependencyProperty.Register(nameof(Icon), typeof(string), typeof(NavMenuItemControl),
                new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty IsActiveProperty = 
            DependencyProperty.Register(nameof(IsActive), typeof(bool), typeof(NavMenuItemControl),
                new PropertyMetadata(false));

        public event EventHandler? Click;

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public string Icon
        {
            get => (string)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public bool IsActive
        {
            get => (bool)GetValue(IsActiveProperty);
            set
            {
                SetValue(IsActiveProperty, value);
                MainButton.BorderThickness = value
                    ? new Thickness(1, 0, 0, 0)
                    : new Thickness(0, 0, 0, 0);
            }
        }

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Image img = ((NavMenuItemControl)d).IconImage;
            try
            {
                img.Source = img.FindResource((string)e.NewValue) as DrawingImage;
            }
            catch
            {
                // ignored
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, EventArgs.Empty);
        }
    }
}

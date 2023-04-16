using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CaloriesCalculator.Controls
{
    public partial class NavMenuItemControl : UserControl
    {
        private static readonly Dictionary<string, string> _icons = new()
        {
            {"Gear", "GearDrawingImage"},
            {"Profile", "ProfileDrawingImage"},
            {"Food", "FoodDrawingImage"},
            {"History", "HistoryDrawingImage"}
        };

        public event EventHandler? Click;

        public static readonly DependencyProperty TextProperty = 
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(NavMenuItemControl),
                new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty IconProperty = 
            DependencyProperty.Register(nameof(Icon), typeof(string), typeof(NavMenuItemControl),
                new PropertyMetadata(string.Empty, IconChangedCallback));

        public static readonly DependencyProperty IsActiveProperty = 
            DependencyProperty.Register(nameof(IsActive), typeof(bool), typeof(NavMenuItemControl),
                new PropertyMetadata(false, ActiveChangedCallback));

        public NavMenuItemControl() => InitializeComponent();

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
            set => SetValue(IsActiveProperty, value);
        }

        private static void IconChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                Image img = ((NavMenuItemControl)d).IconImage;
                img.Source = Application.Current.Resources[_icons[(string)e.NewValue]] as DrawingImage;
            }
            catch
            {
                // ignored
            }
        }

        private static void ActiveChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                NavMenuItemControl itemControl = (NavMenuItemControl)d;
                if (itemControl.IsActive)
                {
                    itemControl.MainButton.BorderThickness = new Thickness(3, 0, 0, 0);
                    itemControl.MainButton.Background = new BrushConverter().ConvertFrom("F3D2C1") as SolidColorBrush;
                }
                else
                {
                    itemControl.MainButton.BorderThickness = new Thickness(0, 0, 0, 0);
                    itemControl.MainButton.Background = new SolidColorBrush(Colors.Transparent);
                }
            }
            catch
            {
                // ignored
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
            => Click?.Invoke(this, EventArgs.Empty);
        
    }
}

using CaloriesCalculator.Controls;
using CaloriesCalculator.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CaloriesCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NavMenuItemControl? _lastSelectedItem;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Navigate("Profile", ProfileMenuItem);
        }

        #region Navigation
        private readonly List<(string Tag, Uri Page)> _pages = new()
        {
            ("Settings", new Uri("Pages/SettingsPage.xaml", UriKind.Relative)),
            ("Profile", new Uri("Pages/ProfilePage.xaml", UriKind.Relative)),
            ("Products", new Uri("Pages/ProductsPage.xaml", UriKind.Relative))
        };

        internal void Navigate(string? itemTag, NavMenuItemControl? menuItemControl = null, bool refresh = false)
        {
            if (itemTag == null)
                return;

            Uri _page = _pages.FirstOrDefault(page => page.Tag.Equals(itemTag)).Page;

            Uri currentPage = ContentFrame.CurrentSource;

            if (_page is not null && (!Equals(_page, currentPage)))
            {
                _ = ContentFrame.Navigate(_page);

                // Change current selected item in UI
                if (menuItemControl != null)
                    menuItemControl.IsActive = true;
                if (_lastSelectedItem != null)
                    _lastSelectedItem.IsActive = false;
                _lastSelectedItem = menuItemControl;
            }
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            // Dispose previous page
            ((Frame)sender).NavigationService.RemoveBackEntry();
        }

        private void NavMenuItemControl_Click(object sender, EventArgs e)
        {
            NavMenuItemControl itemControl = (NavMenuItemControl)sender;
            Navigate(itemControl.Tag.ToString(), itemControl);
        }
        #endregion
    }
}

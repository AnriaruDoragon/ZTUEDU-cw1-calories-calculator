using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using CaloriesCalculator.Controls;
using CCLibrary.User;

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
            SetCurrentProfile(App.CurrentProfile);
        }

        internal void SetCurrentProfile(Profile? profile = null)
        {
            if (profile == null)
                CurrentProfileGrid.Visibility = Visibility.Collapsed;
            else
            {
                CurrentProfileNameTextBlock.Text = profile.Name;
                if (profile.Image != null)
                {
                    BitmapImage avatar = new();
                    using (MemoryStream ms = new(profile.Image))
                    {
                        avatar.StreamSource = ms;
                    }
                    CurrentProfileImageBrush.ImageSource = avatar;
                }
                CurrentProfileGrid.Visibility = Visibility.Visible;
            }
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

            if ((_page is not null && (!Equals(_page, currentPage))) || refresh)
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

            // Set page title
            if (ContentFrame.Content != null)
                PageTitleTextBlock.Text = ((Page)ContentFrame.Content).Title;
        }

        private void NavMenuItemControl_Click(object sender, EventArgs e)
        {
            NavMenuItemControl itemControl = (NavMenuItemControl)sender;
            Navigate(itemControl.Tag.ToString(), itemControl);
        }
        #endregion
    }
}

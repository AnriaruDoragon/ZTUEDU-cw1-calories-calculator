using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Collections.Generic;
using CaloriesCalculator.Pages;
using CaloriesCalculator.Controls;
using CCLibrary.User;

namespace CaloriesCalculator;

public partial class MainWindow : Window
{
    private NavMenuItemControl? _lastSelectedItem;
    private bool _isCongratulated = false;

    public event EventHandler? DateChanged;

    public MainWindow() => InitializeComponent();

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        CaloriesDatePicker.SelectedDate = App.SelectedDate;

        Navigate("Profile", ProfileMenuItem);
        SetCurrentProfile(App.CurrentProfile);
    }

    #region Profile Header

    /// <summary>
    /// Update profile header avatar.
    /// </summary>
    internal void UpdateCurrentProfileAvatar(ImageSource? image = null)
    {
        if (App.CurrentProfile is null)
            return;

        if (App.CurrentProfile.Image is null)
        {
            CurrentProfileImageBrush.ImageSource = null;
            return;
        }

        if (image is not null)
        {
            CurrentProfileImageBrush.ImageSource = image;
            return;
        }

        using MemoryStream ms = new(App.CurrentProfile.Image);
        BitmapImage avatar = new();
        avatar.BeginInit();
        avatar.CacheOption = BitmapCacheOption.OnLoad;
        avatar.StreamSource = ms;
        avatar.EndInit();
        CurrentProfileImageBrush.ImageSource = avatar;
    }

    /// <summary>
    /// Set new Current profile and update info on it.
    /// </summary>
    internal void SetCurrentProfile(Profile? profile)
    {
        App.CurrentProfile = profile;

        if (profile is null)
            CurrentProfileGrid.Visibility = Visibility.Collapsed;
        else
        {
            CurrentProfileNameTextBlock.Text = profile.Name;
            UpdateCurrentProfileAvatar();
            CurrentProfileGrid.Visibility = Visibility.Visible;
        }

        UpdateCalorieMeter();
    }

    private void ProfileHeaderButton_Click(object sender, RoutedEventArgs e)
    {
        if (FindResource("HeaderPopUp") is not ContextMenu headerPopUp)
            return;

        headerPopUp.PlacementTarget = sender as Button;
        headerPopUp.IsOpen = true;
    }

    private void SetupProfilePopUp_Click(object sender, RoutedEventArgs e)
        => Navigate("Profile", ProfileMenuItem);

    private void LogoutPopUp_Click(object sender, RoutedEventArgs e)
    {
        SetCurrentProfile(null); 
        if (ContentFrame.Content is ProfilePage profilePage) 
            profilePage.UpdateDisplayedGrid();
    }
    #endregion

    #region Calories Meter
    /// <summary>
    /// Update calories meter on the page if profile and date is set.
    /// If profile is null - hides the meter. If date is not set - diplays zeros.
    /// </summary>
    internal void UpdateCalorieMeter()
    {
        if (App.CurrentProfile is null)
        {
            CaliriesMeterGrid.Visibility = Visibility.Collapsed;
            return;
        }
        else
            CaliriesMeterGrid.Visibility = Visibility.Visible;

        if (CaloriesDatePicker.SelectedDate is null)
        {
            CaloriesProgress.Max = CaloriesProgress.Value = default;
            NutritionsMeter.Nutrition = default;
            return;
        }

        DateTime selectedDate = (DateTime)CaloriesDatePicker.SelectedDate;
        App.SelectedDate = selectedDate;

        DailyConsumption profileConsumption = App.Database.Context.GetProfileConsumption(App.CurrentProfile, selectedDate);

        CaloriesProgress.Max = App.CurrentProfile.CaloriesGoal;
        CaloriesProgress.Value = profileConsumption.CalculateCalories();
        NutritionsMeter.Nutrition = profileConsumption.CalculateNutrition();

        if (CaloriesProgress.Value >= CaloriesProgress.Max && !_isCongratulated)
        {
            MessageBox.Show($"Обрана дата: {App.SelectedDate:D}\nВи досягли щоденної мети споживання калорій!",
                "Увага", MessageBoxButton.OK, MessageBoxImage.Information);
            _isCongratulated = true;
        }
    }

    private void CaloriesDate_Changed(object sender, SelectionChangedEventArgs e)
    {
        if (CaloriesDatePicker.SelectedDate != App.SelectedDate)
            _isCongratulated = false;
        UpdateCalorieMeter();

        DateChanged?.Invoke(sender, e);
    }
    #endregion

    #region Navigation
    private readonly List<(string Tag, Uri Page)> _pages = new()
    {
        ("Settings", new Uri("Pages/SettingsPage.xaml", UriKind.Relative)),
        ("Profile", new Uri("Pages/ProfilePage.xaml", UriKind.Relative)),
        ("Products", new Uri("Pages/ProductsPage.xaml", UriKind.Relative)),
        ("History", new Uri("Pages/ConsumptionHistoryPage.xaml", UriKind.Relative))
    };

    /// <summary>
    /// Navigate main frame to the specific page.
    /// </summary>
    internal void Navigate(string? itemTag, NavMenuItemControl? menuItemControl = null, bool refresh = false)
    {
        if (itemTag is null)
            return;

        Uri _page = _pages.FirstOrDefault(page => page.Tag.Equals(itemTag)).Page;

        Uri currentPage = ContentFrame.CurrentSource;

        if ((_page is not null && (!Equals(_page, currentPage))) || refresh)
        {
            _ = ContentFrame.Navigate(_page);

            // Change current selected item in UI
            if (menuItemControl is not null)
                menuItemControl.IsActive = true;
            if (_lastSelectedItem is not null)
                _lastSelectedItem.IsActive = false;
            _lastSelectedItem = menuItemControl;
        }
    }

    private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
    {
        // Dispose previous page
        ((Frame)sender).NavigationService.RemoveBackEntry();

        // Set page title
        if (ContentFrame.Content is not null)
            PageTitleTextBlock.Text = ((Page)ContentFrame.Content).Title;
    }

    private void NavMenuItemControl_Click(object sender, EventArgs e)
    {
        NavMenuItemControl itemControl = (NavMenuItemControl)sender;
        Navigate(itemControl.Tag.ToString(), itemControl);
    }
    #endregion
}

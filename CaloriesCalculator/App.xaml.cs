using System;
using System.Windows;
using CCLibrary.Data;
using CCLibrary.User;

#pragma warning disable CA2211

namespace CaloriesCalculator
{
    public partial class App : Application
    {
        public static Database Database = new();

        public static Profile? CurrentProfile;
        public static DateTime SelectedDate = DateTime.Today;

        public App()
        {
            InitializeComponent();

            CurrentProfile = Database.Context.GetRememberedProfile();
        }
    }
}

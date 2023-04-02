using System.Windows;
using CCLibrary.Data;
using CCLibrary.User;

namespace CaloriesCalculator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Database Database;
        public static Profile? CurrentProfile;

        public App()
        {
            InitializeComponent();

            Database = new Database();
            CurrentProfile = Database.Context.GetRememberedProfile();
        }
    }
}

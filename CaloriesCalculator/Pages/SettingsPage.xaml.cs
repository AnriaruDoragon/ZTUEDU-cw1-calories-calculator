using System.Windows;
using System.Windows.Controls;
using CCLibrary.Data;

namespace CaloriesCalculator.Pages
{
    public partial class SettingsPage : Page
    {
        public SettingsPage() => InitializeComponent();

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AskForSize.IsChecked = Settings.Get(AskForSize.Name, true);
        }

        private void Setting_Changed(object sender, RoutedEventArgs e)
        {
            CheckBox setting = (CheckBox)sender;
            Settings.Set(setting.Name, setting.IsChecked);
        }
    }
}

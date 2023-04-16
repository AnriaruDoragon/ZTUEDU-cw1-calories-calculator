using System.Windows;
using System.Windows.Controls;

namespace CaloriesCalculator.Pages.Dialogs
{
    public partial class SetOwnGoalDialog : Window
    {
        public double NewGoal { get; set; }

        public SetOwnGoalDialog() => InitializeComponent();

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
            => DialogResult = true;

        private void CancelButton_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;

        private void NewGoalTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (double.TryParse(((TextBox)sender).Text, out double result))
            {
                if (result >= 1)
                {
                    NewGoal = result;
                    ConfirmButton.IsEnabled = true;
                    return;
                }
            }
            ConfirmButton.IsEnabled = false;
        }
    }
}

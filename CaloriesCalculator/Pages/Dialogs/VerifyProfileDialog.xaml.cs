using System.Windows;
using System.Windows.Controls;

namespace CaloriesCalculator.Pages.Dialogs;

public partial class VerifyProfileDialog : Window
{
    public string Password { get; set; } = string.Empty;
    public string SecretWord { get; set; } = string.Empty;

    public VerifyProfileDialog(string note = "", bool withSecret = true)
    {
        InitializeComponent();
        NoteTextBlock.Text = note;

        if (!withSecret)
            SecretWordTextBlock.Visibility = SecretWordTextBox.Visibility = Visibility.Collapsed;
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        => Password = ((PasswordBox)sender).Password;

    private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        => DialogResult = true;

    private void CancelButton_Click(object sender, RoutedEventArgs e)
        => DialogResult = false;
}

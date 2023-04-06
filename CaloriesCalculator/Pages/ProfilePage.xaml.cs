using CCLibrary.Exceptions;
using CCLibrary.User;
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

namespace CaloriesCalculator.Pages
{
    public partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            InitializeComponent();
            UpdateDisplayedGrid();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.Database.Context.Profiles.Any())
                MessageBox.Show("Схоже, що на цьому пристрої відсутні користувачі.\nВам доведеться створити нового, задавши логін, пароль та секретне слово, яке може знадобитися, якщо ви забудете пароль.",
                    "Відсутні користувачі", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// If profile is set for the App
        /// </summary>
        internal void UpdateDisplayedGrid()
        {
            LoginTextBox.Text = default;
            LoginPasswordBox.Password = default;

            if (App.CurrentProfile is not null)
            {
                LoginGrid.Visibility = Visibility.Collapsed;
                ManageGrid.Visibility = Visibility.Visible;
            }
            else
            {
                LoginGrid.Visibility = Visibility.Visible;
                ManageGrid.Visibility = Visibility.Collapsed;
            }
        }

        private bool CheckLoginFields()
        {
            if (string.IsNullOrWhiteSpace(LoginTextBox.Text) || string.IsNullOrWhiteSpace(LoginPasswordBox.Password))
            {
                MessageBox.Show("Заповніть логін та пароль!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckLoginFields())
                return;

            Profile? profile;
            try
            {
                profile = App.Database.Context.GetProfile(LoginTextBox.Text, LoginPasswordBox.Password);
            }
            catch (Exception ex)
            {
                if (ex is ProfiletNotFoundException || ex is WrongPasswordException)
                    MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("Невідома помилка.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ((MainWindow)Window.GetWindow(this)).SetCurrentProfile(profile);
            UpdateDisplayedGrid();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckLoginFields())
                return;

            Dialogs.VerifyProfileDialog verifyDialog = new("Повторіть ваш пароль, щоб переконатися в його достовірності, та вкажіть тайне слово, яке знадобиться при відновленні паролю.");
            bool? result = verifyDialog.ShowDialog();

            if (result != true)
                return;

            if (!LoginPasswordBox.Password.Equals(verifyDialog.Password, StringComparison.Ordinal))
            {
                MessageBox.Show("Паролі не співпадають!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(verifyDialog.SecretWord))
            {
                MessageBox.Show("Обов'язково вкажіть тайне слово!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                App.Database.Context.CreateProfile(LoginTextBox.Text, LoginPasswordBox.Password, verifyDialog.SecretWord);
                MessageBox.Show("Ви створили профіль користувача.\nТепер ви можете використоати цей логін та пароль для авторизації.\nЯкщо ви забули пароль, використайте ваше тайне слово для його відновлення.",
                    "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (ProfileAlreadyExistsException ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ResetPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginTextBox.Text))
            {
                MessageBox.Show("Введіть логін, за яким буде відбуватися відновлення!",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Dialogs.VerifyProfileDialog verifyDialog = new("Введіть новий пароль та тайне слово для вашого профілю.");
            bool? result = verifyDialog.ShowDialog();

            if (result != true)
                return;

            if (string.IsNullOrWhiteSpace(verifyDialog.Password))
            {
                MessageBox.Show("Ви не вказали новий пароль!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(verifyDialog.SecretWord))
            {
                MessageBox.Show("Ви не вказали тайне слово!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                App.Database.Context.ResetPassword(LoginTextBox.Text, verifyDialog.SecretWord, verifyDialog.Password);
                MessageBox.Show("Ви встановили новий пароль!\nВикористайте його для авторизації.",
                    "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                if (ex is ProfiletNotFoundException || ex is WrongSecretWordException)
                    MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("Невідома помилка.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

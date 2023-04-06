using CCLibrary.Exceptions;
using CCLibrary.User;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateDisplayedGrid();

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
                ManageProfileIDTextBlock.Text = $"# {App.CurrentProfile.Id}";
                ManageProfileLoginTextBlock.Text = App.CurrentProfile.Login;
                ManageProfileNameTextBox.Text = App.CurrentProfile.Name;
                LoginGrid.Visibility = Visibility.Collapsed;
                ManageGrid.Visibility = Visibility.Visible;
                UpdateAvatars();
            }
            else
            {
                LoginGrid.Visibility = Visibility.Visible;
                ManageGrid.Visibility = Visibility.Collapsed;
            }
        }

        #region Login grid
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

        #endregion

        /// <summary>
        /// Ensure that profile is not null before processing with data.
        /// </summary>
        private void ManageProfileEnsureNotNull()
        {
            if (App.CurrentProfile is null)
            {
                MessageBox.Show("Нажаль виникла невідома помилка!\nСпробуйте увійти знову.",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                UpdateDisplayedGrid();
            }
        }

        private void ManageProfileNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ManageProfileEnsureNotNull();

            if (App.CurrentProfile.Name.Equals(ManageProfileNameTextBox.Text, StringComparison.Ordinal) || string.IsNullOrWhiteSpace(ManageProfileNameTextBox.Text))
                ManageProfileNameSaveButton.Visibility = Visibility.Collapsed;
            else
                ManageProfileNameSaveButton.Visibility = Visibility.Visible;
        }

        private void ManageProfileNameSaveButton_Click(object sender, RoutedEventArgs e)
        {
            ManageProfileEnsureNotNull();

            ((Button)sender).Visibility = Visibility.Collapsed;

            App.CurrentProfile.Name = ManageProfileNameTextBox.Text;
            App.Database.Context.SaveChanges();
            ((MainWindow)Window.GetWindow(this)).CurrentProfileNameTextBlock.Text = ManageProfileNameTextBox.Text;
        }

        /// <summary>
        /// Update avatars everywhere.
        /// </summary>
        private void UpdateAvatars()
        {
            ManageProfileEnsureNotNull();

            BitmapImage? avatar = null;
            if (App.CurrentProfile.Image is null)
                ManageProfileImageBrush.ImageSource = null;
            else
            {
                using MemoryStream ms = new(App.CurrentProfile.Image);
                avatar = new();
                avatar.BeginInit();
                avatar.CacheOption = BitmapCacheOption.OnLoad;
                avatar.StreamSource = ms;
                avatar.EndInit();
                ManageProfileImageBrush.ImageSource = avatar;
            }

            ((MainWindow)Window.GetWindow(this)).UpdateCurrentProfileAvatar(avatar);

            if (App.CurrentProfile.Image is null)
                DeleteAvatarButton.Visibility = Visibility.Collapsed;
            else
                DeleteAvatarButton.Visibility = Visibility.Visible;
        }

        private void SelectAvatarButton_Click(object sender, RoutedEventArgs e)
        {
            ManageProfileEnsureNotNull();

            OpenFileDialog fileDialog = new()
            {
                Filter = "Зображення (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg; *.jpeg; *.png; *.bmp"
            };
            bool? result = fileDialog.ShowDialog();

            if (result is true)
            {
                using Stream stream = fileDialog.OpenFile();
                using BinaryReader br = new(stream);
                byte[] bytes = br.ReadBytes((int)stream.Length);
                App.CurrentProfile.Image = bytes;
                App.Database.Context.SaveChanges();
                UpdateAvatars();
            }
        }

        private void DeleteAvatarButton_Click(object sender, RoutedEventArgs e)
        {
            ManageProfileEnsureNotNull();

            App.CurrentProfile.Image = null;
            App.Database.Context.SaveChanges();
            UpdateAvatars();
        }
    }
}

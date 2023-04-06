using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using CCLibrary.User;
using CCLibrary.Exceptions;

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
                HorizontalAlignment = HorizontalAlignment.Left;

                // Restore profile settings info
                ManageProfileIDTextBlock.Text = $"# {App.CurrentProfile.Id}";
                ManageProfileLoginTextBlock.Text = App.CurrentProfile.Login;
                ManageProfileNameTextBox.Text = App.CurrentProfile.Name;
                LoginGrid.Visibility = Visibility.Collapsed;
                ManageGrid.Visibility = Visibility.Visible;
                UpdateAvatars();

                // Restore profile preferences
                switch (App.CurrentProfile.Sex)
                {
                    case Sexes.Male:
                        ProfileMaleSexRB.IsChecked = true;
                        break;
                    case Sexes.Female:
                        ProfileFemaleSexRB.IsChecked = true;
                        break;
                }
                switch (App.CurrentProfile.Goal)
                {
                    case Goals.Lose:
                        ProfileLoseGoalRB.IsChecked = true;
                        break;
                    case Goals.Maintaint:
                        ProfileMaintaintGoalRB.IsChecked = true;
                        break;
                    case Goals.Gain:
                        ProfileGainGoalRB.IsChecked = true;
                        break;
                }
                ProfileBirthdatDatePicker.SelectedDate = App.CurrentProfile.BirthDay;
                ProfileHeightTextBox.Text = App.CurrentProfile.HeightInCm.ToString();
                ProfileWeightTextBox.Text = App.CurrentProfile.WeightInKg.ToString();
            }
            else
            {

                LoginGrid.Visibility = Visibility.Visible;
                ManageGrid.Visibility = Visibility.Collapsed;
                HorizontalAlignment = HorizontalAlignment.Center;
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

        #region Manage profile tools
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

        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            ManageProfileEnsureNotNull();

            if (string.IsNullOrWhiteSpace(ManageOldPasswordBox.Password) || string.IsNullOrWhiteSpace(ManageNewPasswordBox.Password) || string.IsNullOrWhiteSpace(ManageRepPasswordBox.Password))
            {
                MessageBox.Show("Ви пропустили поля!\nБудь ласка, заповніть всі поля для встановлення нового паролю.",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!ManageNewPasswordBox.Password.Equals(ManageRepPasswordBox.Password, StringComparison.Ordinal))
            {
                MessageBox.Show("Повторення паролю не співпадає!",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                App.Database.Context.UpdatePassword(App.CurrentProfile.Id, ManageOldPasswordBox.Password, ManageNewPasswordBox.Password);
                MessageBox.Show("Ви успішно змінили пароль на новий!",
                    "Змані паролю", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (WrongPasswordException ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Нажаль виникла невідома помилка!",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            ManageOldPasswordBox.Password = ManageNewPasswordBox.Password = ManageRepPasswordBox.Password = default;
        }

        #endregion

        #region Profile preferences
        /// <summary>
        /// Check if all preferences are set.
        /// </summary>
        private bool EnsurePreferencesSet()
        {
            if (!(ProfileMaleSexRB.IsChecked == true || ProfileFemaleSexRB.IsChecked == true)) 
                return false;

            if (!(ProfileLoseGoalRB.IsChecked == true || ProfileMaintaintGoalRB.IsChecked == true || ProfileGainGoalRB.IsChecked == true)) 
                return false;

            if (ProfileBirthdatDatePicker.SelectedDate is null)
                return false;

            if (ProfileBirthdatDatePicker.SelectedDate >= DateTime.Today)
                return false;

            if (!(float.TryParse(ProfileHeightTextBox.Text, out _) && float.TryParse(ProfileWeightTextBox.Text, out _)))
                return false;

            return true;
        }

        private void ProfilePreferences_Updated(object sender, RoutedEventArgs e)
        {
            if (!EnsurePreferencesSet())
                CalculateGoalButton.Visibility = Visibility.Collapsed;
            else
                CalculateGoalButton.Visibility = Visibility.Visible;
        }

        private void CalculateGoalButton_Click(object sender, RoutedEventArgs e)
        {
            ManageProfileEnsureNotNull();
            
            App.CurrentProfile.Sex = ProfileMaleSexRB.IsChecked == true ? Sexes.Male : Sexes.Female;

            App.CurrentProfile.Goal = ProfileMaintaintGoalRB.IsChecked == true ? Goals.Maintaint
                : ProfileLoseGoalRB.IsChecked == true ? Goals.Lose : Goals.Gain;
            App.CurrentProfile.BirthDay = ProfileBirthdatDatePicker.SelectedDate;

            try
            {
                App.CurrentProfile.HeightInCm = float.Parse(ProfileHeightTextBox.Text);
                App.CurrentProfile.WeightInKg = float.Parse(ProfileWeightTextBox.Text);
            }
            catch (Exception ex)
            {
                if (ex is ValueOutOfRangeException)
                    MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("Невідома помилка.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            App.CurrentProfile.CalculateCaloriesNorm();
            App.Database.Context.SaveChanges();

            ((MainWindow)Window.GetWindow(this)).UpdateCalorieMeter();
        }

        private void SetOwnGoalButton_Click(object sender, RoutedEventArgs e)
        {
            ManageProfileEnsureNotNull();

            Dialogs.SetOwnGoalDialog ownGoalDialog = new();
            bool? result = ownGoalDialog.ShowDialog();

            if (result != true)
                return;

            App.CurrentProfile.CaloriesGoal = ownGoalDialog.NewGoal;
            App.Database.Context.SaveChanges();
            ((MainWindow)Window.GetWindow(this)).UpdateCalorieMeter();
        }
        #endregion
    }
}

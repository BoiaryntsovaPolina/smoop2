using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using StoreApp.Models;
using StoreApp.Services;

namespace StoreApp.Views
{
    public partial class AddUserWindow : Window
    {
        public AddUserWindow()
        {
            InitializeComponent();
        }

        private void BtnEye_Click(object sender, RoutedEventArgs e)
        {
            if (TxtPass.Visibility == Visibility.Visible)
            {
                TxtPassVisible.Text = TxtPass.Password;
                TxtPass.Visibility = Visibility.Collapsed;
                TxtPassVisible.Visibility = Visibility.Visible;
            }
            else
            {
                TxtPass.Password = TxtPassVisible.Text;
                TxtPassVisible.Visibility = Visibility.Collapsed;
                TxtPass.Visibility = Visibility.Visible;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var login = TxtLogin.Text?.Trim() ?? "";
            var pass = (TxtPass.Visibility == Visibility.Visible) ? TxtPass.Password : TxtPassVisible.Text;

            if (string.IsNullOrWhiteSpace(login) || login.Length < 3 || !Regex.IsMatch(login, @"^[A-Za-z0-9]+$"))
            {
                MessageBox.Show("Логін має містити лише латинські літери та цифри, мінімум 3 символи.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(pass) || pass.Length < 4)
            {
                MessageBox.Show("Пароль має бути щонайменше 4 символи.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var sel = (CbRole.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Loader";
            var role = UserRole.Loader;
            if (sel == "Admin") role = UserRole.Admin;
            else if (sel == "Director") role = UserRole.Director;

            var user = new User { Username = login, Password = pass, Role = role };

            if (!UserService.Add(user))
            {
                MessageBox.Show("Користувач з таким логіном вже існує.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
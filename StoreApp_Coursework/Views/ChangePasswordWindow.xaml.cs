using System.Windows;
using StoreApp.Services;

namespace StoreApp.Views
{
    public partial class ChangePasswordWindow : Window
    {
        private readonly Models.User _adminUser;

        public ChangePasswordWindow(Models.User admin)
        {
            InitializeComponent();
            _adminUser = admin;
        }

        private void BtnEye_Click(object sender, RoutedEventArgs e)
        {
            if (PbNewPass.Visibility == Visibility.Visible)
            {
                TxtNewPassVisible.Text = PbNewPass.Password;
                PbNewPass.Visibility = Visibility.Collapsed;
                TxtNewPassVisible.Visibility = Visibility.Visible;
            }
            else
            {
                PbNewPass.Password = TxtNewPassVisible.Text;
                TxtNewPassVisible.Visibility = Visibility.Collapsed;
                PbNewPass.Visibility = Visibility.Visible;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string newPass = (PbNewPass.Visibility == Visibility.Visible) ? PbNewPass.Password : TxtNewPassVisible.Text;
            string confirmPass = PbConfirmPass.Password;

            if (string.IsNullOrWhiteSpace(newPass) || newPass.Length < 4)
            {
                MessageBox.Show("Пароль має бути не менше 4 символів.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (newPass != confirmPass)
            {
                MessageBox.Show("Паролі не співпадають.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (newPass == "admin")
            {
                MessageBox.Show("Новий пароль не може бути 'admin'.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Оновлення
            _adminUser.Password = newPass;
            UserService.Remove(_adminUser.Username);
            UserService.Add(_adminUser);

            MessageBox.Show("Пароль успішно змінено!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
            DialogResult = true;
            Close();
        }
    }
}
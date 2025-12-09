using System.Windows;
using StoreApp.Services;
using StoreApp.Models;
using System.Linq;

namespace StoreApp.Views
{
    public partial class AuthWindow : Window
    {
        private readonly ViewModels.AuthViewModel _vm;

        public AuthWindow()
        {
            InitializeComponent();
            App.ApplyTheme(this); // Застосовуємо тему

            TxtModeHint.Text = $"Режим: {SettingsService.Settings.Mode}";

            if (SettingsService.Settings.Mode == AppMode.Business)
            {
                BtnRegister.Visibility = Visibility.Collapsed;
                UserService.EnsureAdminExistsForBusiness();

                // Перевіряємо: чи існує admin з паролем admin?
                var admin = UserService.Find("admin", "admin");
                if (admin != null)
                {
                    MessageBox.Show("Увага! Використовуються стандартні дані адміністратора.\nЛогін: admin\nПароль: admin\n\nБудь ласка, змініть пароль після входу!",
                                   "Безпека", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            _vm = new ViewModels.AuthViewModel();
            _vm.AuthSucceeded += Vm_AuthSucceeded;
            DataContext = _vm;
        }

        private void Vm_AuthSucceeded(object? sender, User user)
        {
            // Якщо адмін зайшов з паролем "admin", змушуємо змінити
            if (user.Role == UserRole.Admin && user.Password == "admin" && SettingsService.Settings.Mode == AppMode.Business)
            {
                var changePassWin = new ChangePasswordWindow(user);
                App.ApplyTheme(changePassWin);
                if (changePassWin.ShowDialog() != true)
                {
                    TxtGlobalError.Text = "Необхідно змінити стандартний пароль!";
                    return;
                }
            }

            var main = new MainWindow(user);
            main.Show();
            Close();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            ErrLogin.Text = ErrPass.Text = TxtGlobalError.Text = "";
            string login = TxtUsername.Text.Trim();
            string pass = (PwdBox.Visibility == Visibility.Visible) ? PwdBox.Password : TxtPasswordVisible.Text;

            if (string.IsNullOrWhiteSpace(login)) { ErrLogin.Text = "Введіть логін"; return; }
            if (string.IsNullOrWhiteSpace(pass)) { ErrPass.Text = "Введіть пароль"; return; }

            _vm.Username = login;
            _vm.Password = pass;
            _vm.LoginCommand.Execute(null);

            if (!string.IsNullOrEmpty(_vm.Status)) TxtGlobalError.Text = _vm.Status;
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            _vm.Username = TxtUsername.Text.Trim();
            _vm.Password = PwdBox.Password;
            _vm.RegisterCommand.Execute(null);
            if (_vm.Status.Contains("успішно")) MessageBox.Show(_vm.Status);
            else TxtGlobalError.Text = _vm.Status;
        }

        private void BtnEye_Click(object sender, RoutedEventArgs e)
        {
            if (PwdBox.Visibility == Visibility.Visible)
            {
                TxtPasswordVisible.Text = PwdBox.Password;
                PwdBox.Visibility = Visibility.Collapsed; TxtPasswordVisible.Visibility = Visibility.Visible;
            }
            else
            {
                PwdBox.Password = TxtPasswordVisible.Text;
                TxtPasswordVisible.Visibility = Visibility.Collapsed; PwdBox.Visibility = Visibility.Visible;
            }
        }
    }
}
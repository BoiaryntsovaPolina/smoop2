using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Lab5
{
    /// <summary>
    /// Логика взаимодействия для RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            TxtStatus.Text = "";
            var username = TxtUsername.Text.Trim();
            var email = TxtEmail.Text.Trim();
            var pass = TxtPass.Password;
            var pass2 = TxtPassConfirm.Password;

            var errors = ValidateRegistration(username, email, pass, pass2);
            if (errors.Count > 0)
            {
                TxtStatus.Text = string.Join("\n", errors);
                return;
            }

            var existingByLogin = UserService.FindByUsernameOrEmail(username);
            if (existingByLogin != null)
            {
                TxtStatus.Text = "Користувач з таким ім'ям вже існує.";
                return;
            }
            var existingByEmail = UserService.FindByUsernameOrEmail(email);
            if (existingByEmail != null)
            {
                TxtStatus.Text = "Користувач з таким email вже існує.";
                return;
            }

            var user = new User { Username = username, Email = email, Password = pass };
            UserService.AddUser(user, toSocial: false);

            MessageBox.Show("Реєстрація успішна. Тепер можна увійти.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private List<string> ValidateRegistration(string username, string email, string pass, string pass2)
        {
            var errors = new List<string>();
            if (string.IsNullOrEmpty(username)) errors.Add("Ім'я користувача не може бути порожнім.");
            if (string.IsNullOrEmpty(email)) errors.Add("Email не може бути порожнім.");
            else
            {
                var rx = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                if (!rx.IsMatch(email)) errors.Add("Email має невірний формат.");
            }
            if (string.IsNullOrEmpty(pass)) errors.Add("Пароль не може бути порожнім.");
            else if (pass.Length < 4) errors.Add("Пароль має бути щонайменше 4 символи.");
            if (pass != pass2) errors.Add("Паролі не співпадають.");
            return errors;
        }
    }
}
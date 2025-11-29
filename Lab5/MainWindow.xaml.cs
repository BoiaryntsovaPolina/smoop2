using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            TxtLogin.Text = "";
            TxtPassword.Password = "";
            TxtStatus.Text = "";
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            TxtStatus.Text = "";
            var login = TxtLogin.Text.Trim();
            var pass = TxtPassword.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(pass))
            {
                TxtStatus.Text = "Заповніть усі поля.";
                return;
            }

            var user = UserService.FindByUsernameOrEmail(login);

            if (user == null)
            {
                TxtStatus.Text = "Користувача з таким логіном або email не існує.";
                return;
            }

            if (user.Password != pass)
            {
                TxtStatus.Text = "Неправильний пароль. Забули пароль?";
                return;
            }

            MessageBox.Show($"Успішно увійшли як {user.Username}", "Вітаємо", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void OpenRegister_Click(object sender, RoutedEventArgs e)
        {
            var reg = new RegisterWindow();
            reg.Owner = this;
            reg.ShowDialog();
        }

        private void SocialGoogle_Click(object sender, RoutedEventArgs e) => SimulateSocialLogin("Google");
        private void SocialFacebook_Click(object sender, RoutedEventArgs e) => SimulateSocialLogin("Facebook");
        private void SocialTwitter_Click(object sender, RoutedEventArgs e) => SimulateSocialLogin("Twitter");

        private void SimulateSocialLogin(string provider)
        {
            var res = MessageBox.Show($"Симуляція входу через {provider}.\nНатисніть OK, щоб увійти як 'social_{provider.ToLower()}'", $"Вхід через {provider}", MessageBoxButton.OKCancel);
            if (res != MessageBoxResult.OK) return;

            string username = $"social_{provider.ToLower()}";
            string email = $"{username}@{provider.ToLower()}.local";

            var existing = UserService.FindByUsernameOrEmail(username);
            if (existing == null)
            {
                var u = new User { Username = username, Email = email, Password = "social" };
                UserService.AddUser(u, toSocial: true);
            }

            MessageBox.Show($"Успішно увійшли через {provider} як {username}", "Соціальна авторизація", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
    }
}
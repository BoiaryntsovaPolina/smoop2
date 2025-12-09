using System.Linq;
using System.Windows;
using StoreApp.Models;
using StoreApp.Services;
using StoreApp.ViewModels;

namespace StoreApp.Views
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _vm;

        public MainWindow(User user)
        {
            InitializeComponent();
            App.ApplyTheme(this); // Застосування стилю (кольорів)

            // --- БЛОК АВТОМАТИЧНОЇ ГЕНЕРАЦІЇ (DEMO) ---
            // Перевіряємо, чи є товари в базі
            if (!ProductService.GetAll().Any())
            {
                // Якщо база порожня — генеруємо 20 товарів через FakerService
                var fakes = FakerService.GenerateProducts(20);

                // Зберігаємо їх у програму
                foreach (var p in fakes)
                {
                    ProductService.Add(p);
                }
            }
            // -------------------------------------------

            _vm = new MainViewModel();

            DataContext = _vm;

            // Передаємо користувача у ViewModel
            _vm.SetCurrentUser(user);

            TxtUser.Text = _vm.CurrentUserDisplay;

            // Встановлюємо колір верхньої панелі залежно від режиму
            if (Resources["TopBarColor"] is System.Windows.Media.Brush brush)
            {
                TopBorder.Background = brush;
            }

            // Налаштування видимості кнопок. Кнопка "Користувачі" тільки для Адміна в Business режимі
            BtnManageUsers.Visibility = (user.Role == UserRole.Admin && SettingsService.Settings.Mode == AppMode.Business)
                                        ? Visibility.Visible : Visibility.Collapsed;

            // Вкладка "Накладні" прихована в Home режимі
            TabInvoices.Visibility = SettingsService.Settings.Mode == AppMode.Home ? Visibility.Collapsed : Visibility.Visible;
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вийти з облікового запису?", "Вихід", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                SettingsService.Settings.LastUser = "";
                SettingsService.Save();

                var auth = new AuthWindow();
                auth.Show();
                this.Close();
            }
        }

        private void BtnChangeMode_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Змінити режим роботи?", "Зміна режиму", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                SettingsService.SetMode(AppMode.Unset);
                var modeWin = new ModeSelectionWindow();
                modeWin.Show();
                this.Close();
            }
        }

        private void BtnManageUsers_Click(object sender, RoutedEventArgs e)
        {
            var adm = new AdminWindow { Owner = this };
            App.ApplyTheme(adm);
            adm.ShowDialog();
        }
    }
}
using System.Windows;
using StoreApp.Services;

namespace StoreApp.Views
{
    public partial class ModeSelectionWindow : Window
    {
        public ModeSelectionWindow()
        {
            InitializeComponent();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            SettingsService.SetMode(AppMode.Home);
            OpenAuth();
        }

        private void Business_Click(object sender, RoutedEventArgs e)
        {
            SettingsService.SetMode(AppMode.Business);
            // Гарантуємо існування admin
            UserService.EnsureAdminExistsForBusiness();
            OpenAuth();
        }

        private void OpenAuth()
        {
            var win = new AuthWindow();
            win.Show();
            this.Close();
        }
    }
}
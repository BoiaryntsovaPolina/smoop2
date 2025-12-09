using System.Windows;
using StoreApp.Services;
using StoreApp.Views;

namespace StoreApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Логіка вибору стартового вікна
            if (SettingsService.Settings.Mode == AppMode.Unset)
            {
                new ModeSelectionWindow().Show();
            }
            else
            {
                new AuthWindow().Show();
            }
        }

        // Допоміжний метод для стилізації вікон
        public static void ApplyTheme(Window window)
        {
            if (SettingsService.Settings.Mode == AppMode.Business)
            {
                // Business: Стриманий сірий стиль
                window.Background = System.Windows.Media.Brushes.WhiteSmoke;
                if (window.Resources["TopBarColor"] == null)
                    window.Resources.Add("TopBarColor", System.Windows.Media.Brushes.LightSlateGray);
            }
            else
            {
                // Home: Затишний теплий стиль (світло-блакитний/бежевий)
                window.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(250, 245, 235)); // Linen color
                if (window.Resources["TopBarColor"] == null)
                    window.Resources.Add("TopBarColor", System.Windows.Media.Brushes.CornflowerBlue);
            }
        }
    }
}
using System.Linq;
using System.Windows;
using System.Collections.Generic;
using StoreApp.Models;
using StoreApp.Services;

namespace StoreApp.Views
{
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            TxtMode.Text = $"Режим: {SettingsService.Settings.Mode}";
            Refresh();
        }

        // Оновлення таблиці даними з сервісу
        private void Refresh()
        {
            var users = UserService.GetAll().Select(u => new { u.Username, Role = u.Role.ToString() }).ToList();
            LvUsers.ItemsSource = users;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new AddUserWindow { Owner = this };
            App.ApplyTheme(dlg);
            if (dlg.ShowDialog() == true)
            {
                Refresh();
                MessageBox.Show("Користувача додано.", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = LvUsers.SelectedItems;

            if (selectedItems == null || selectedItems.Count == 0)
            {
                MessageBox.Show("Оберіть користувачів.", "Інфо", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (MessageBox.Show($"Видалити обраних ({selectedItems.Count} шт.)?", "Підтвердження", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var usersToDelete = new List<string>();

                foreach (var item in selectedItems)
                {
                    // Отримуємо значення властивості "Username" з вибраного рядка таблиці
                    var prop = item.GetType().GetProperty("Username");
                    var username = prop?.GetValue(item)?.ToString();

                    if (!string.IsNullOrEmpty(username))
                    {
                        // Захист від видалення головного адміна або самого себе
                        if (username == "admin" || username == SettingsService.Settings.LastUser)
                        {
                            MessageBox.Show($"Не можна видалити '{username}'.", "Заборонено", MessageBoxButton.OK, MessageBoxImage.Warning);
                            continue;
                        }
                        usersToDelete.Add(username);
                    }
                }

                // Видаляємо всіх зібраних користувачів
                foreach (var login in usersToDelete)
                {
                    UserService.Remove(login);
                }

                MessageBox.Show("Видалено.", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                Refresh();
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e) => Close();
    }
}
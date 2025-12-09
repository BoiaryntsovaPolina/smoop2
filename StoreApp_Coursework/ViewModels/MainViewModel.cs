using System;
using System.Windows;
using StoreApp.Helpers;
using StoreApp.Models;
using StoreApp.Services;

namespace StoreApp.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        public ProductsViewModel ProductsVM { get; }
        public InvoicesViewModel InvoicesVM { get; }

        private User? _currentUser;
        public string CurrentUserDisplay => _currentUser != null ? $"Користувач: {_currentUser.Username} ({_currentUser.Role})" : "Гість";

        private bool _isAdminVisible;
        public bool IsAdminVisible { get => _isAdminVisible; set => Set(ref _isAdminVisible, value); }

        private bool _isDirectorVisible;
        public bool IsDirectorVisible { get => _isDirectorVisible; set => Set(ref _isDirectorVisible, value); }

        private bool _isLoaderVisible;
        public bool IsLoaderVisible { get => _isLoaderVisible; set => Set(ref _isLoaderVisible, value); }

        // Якщо режим Home — приховуємо накладні
        private bool _showInvoices;
        public bool ShowInvoices { get => _showInvoices; set => Set(ref _showInvoices, value); }

        public RelayCommand ChangeModeCommand { get; }
        public RelayCommand LogoutCommand { get; }

        public MainViewModel()
        {
            ProductsVM = new ProductsViewModel();
            InvoicesVM = new InvoicesViewModel();

            ChangeModeCommand = new RelayCommand(_ => ChangeMode());
            LogoutCommand = new RelayCommand(_ => Logout());

            // за замовчуванням визначимо видимість вкладок за режимом
            UpdateVisibilityByMode(null);
        }

        public void SetCurrentUser(User user)
        {
            _currentUser = user;
            UpdateVisibilityByMode(user);
            Raise(nameof(CurrentUserDisplay));

            // Оновлюємо права в ProductsVM
            ProductsVM.UpdatePermissions(user);

            // Оновлюємо права в InvoicesVM (створимо метод там теж)
            InvoicesVM.UpdatePermissions(user);
        }

        private void UpdateVisibilityByMode(User? user)
        {
            var mode = SettingsService.Settings.Mode;
            if (mode == AppMode.Home)
            {
                // Home: немає накладних, ролі HomeUser
                ShowInvoices = false;

                IsAdminVisible = false;
                IsDirectorVisible = false;
                IsLoaderVisible = false;
            }
            else
            {
                // Business: показати або приховати елементи залежно від ролі
                ShowInvoices = true;
                if (user == null)
                {
                    IsAdminVisible = false;
                    IsDirectorVisible = false;
                    IsLoaderVisible = false;
                }
                else
                {
                    IsAdminVisible = user.Role == UserRole.Admin;
                    IsDirectorVisible = user.Role == UserRole.Director;
                    IsLoaderVisible = user.Role == UserRole.Loader;
                }
            }
        }

        private void ChangeMode()
        {
            var res = MessageBox.Show("Змінити режим? (Після цього відкриється вибір режиму)", "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                SettingsService.SetMode(AppMode.Unset);
                var mode = new Views.ModeSelectionWindow();
                mode.Show();
                Application.Current.MainWindow?.Close();
            }
        }

        private void Logout()
        {
            if (MessageBox.Show("Вийти з облікового запису?", "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                SettingsService.Settings.LastUser = "";
                SettingsService.Save();
                var auth = new Views.AuthWindow();
                auth.Show();
                Application.Current.MainWindow?.Close();
            }
        }
    }
}

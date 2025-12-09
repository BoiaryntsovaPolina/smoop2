using System;
using System.Windows.Input;
using StoreApp.Helpers;
using StoreApp.Models;
using StoreApp.Services;

namespace StoreApp.ViewModels
{
    public class AuthViewModel : ObservableObject
    {
        private string _username = "";
        private string _password = "";
        private string _status = ""; // Повідомлення про помилку або успіх

        public string Username { get => _username; set => Set(ref _username, value); }
        public string Password { get => _password; set => Set(ref _password, value); }
        public string Status { get => _status; set => Set(ref _status, value); }

        public RelayCommand LoginCommand { get; }
        public RelayCommand RegisterCommand { get; }

        // Подія, яка повідомляє вікно, що вхід успішний
        public event EventHandler<User>? AuthSucceeded;

        public AuthViewModel()
        {
            LoginCommand = new RelayCommand(_ => Login());
            RegisterCommand = new RelayCommand(_ => Register());
        }

        private void Login()
        {
            Status = "";
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                Status = "Введіть логін і пароль.";
                return;
            }

            // У бізнес-режимі перевіряємо наявність адміна
            if (SettingsService.Settings.Mode == AppMode.Business)
                UserService.EnsureAdminExistsForBusiness();

            // Шукаємо користувача в базі
            var user = UserService.Find(Username.Trim(), Password);
            if (user == null)
            {
                Status = "Невірний логін або пароль.";
                return;
            }

            // Запам'ятовуємо логін для зручності
            SettingsService.Settings.LastUser = user.Username;
            SettingsService.Save();

            // Запускаємо подію успішного входу
            AuthSucceeded?.Invoke(this, user);
        }

        private void Register()
        {
            Status = "";

            // Заборона реєстрації в Business режимі (тільки Адмін може додавати)
            if (SettingsService.Settings.Mode == AppMode.Business)
            {
                Status = "Реєстрація доступна тільки адміністратору.";
                return;
            }

            // Валідація даних
            if (string.IsNullOrWhiteSpace(Username) || Username.Trim().Length < 3)
            {
                Status = "Логін — мінімум 3 символи.";
                return;
            }
            if (string.IsNullOrWhiteSpace(Password) || Password.Length < 3)
            {
                Status = "Пароль — мінімум 3 символи.";
                return;
            }

            // Створення нового користувача
            var user = new User { Username = Username.Trim(), Password = Password, Role = UserRole.HomeUser };

            if (!UserService.Add(user))
            {
                Status = "Користувач з таким логіном вже існує.";
                return;
            }

            Status = "Реєстрація пройшла успішно.";
        }
    }
}
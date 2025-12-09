using System;
using System.Collections.Generic;
using System.Linq;
using StoreApp.Helpers; // Підключили наш хелпер
using StoreApp.Models;

namespace StoreApp.Services
{
    public static class UserService
    {
        private static JsonDataStore<User> StoreForMode()
        {
            var filename = SettingsService.Settings.Mode == AppMode.Business ? "users_business.json" : "users_home.json";
            return new JsonDataStore<User>(filename);
        }

        // Читаємо з файлу (зашифровані), дешифруємо для програми
        public static List<User> GetAll()
        {
            try
            {
                var users = StoreForMode().GetAll().ToList();

                // Проходимо по кожному користувачу і дешифруємо пароль
                foreach (var user in users)
                {
                    user.Password = SecurityHelper.Decrypt(user.Password);
                }

                return users;
            }
            catch
            {
                return new List<User>();
            }
        }

        public static User? Find(string username, string password)
        {
            // Тут ми працюємо з розшифрованим списком, тому порівнюємо напряму
            var list = GetAll();
            return list.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && u.Password == password);
        }

        // Отримуємо нормальний пароль, зберігаємо його
        public static bool Add(User user)
        {
            var list = GetAll(); // Отримуємо розшифрований список
            if (list.Any(u => u.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase))) return false;

            list.Add(user);
            SaveUsersWithEncryption(list); // Зберігаємо з шифруванням
            return true;
        }

        public static bool Remove(string username)
        {
            var list = GetAll();
            var removed = list.RemoveAll(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)) > 0;
            if (removed) SaveUsersWithEncryption(list);
            return removed;
        }

        public static void EnsureAdminExistsForBusiness()
        {
            if (SettingsService.Settings.Mode != AppMode.Business) return;

            var list = GetAll();
            if (!list.Any(u => u.Role == UserRole.Admin))
            {
                list.Add(new User { Username = "admin", Password = "admin", Role = UserRole.Admin });
                SaveUsersWithEncryption(list);
            }
        }

        // Метод для збереження 
        private static void SaveUsersWithEncryption(List<User> users)
        {
            // Створюємо копію списку
            var usersToSave = users.Select(u => new User
            {
                Username = u.Username,
                Role = u.Role,
                // Шифруємо пароль перед записом!
                Password = SecurityHelper.Encrypt(u.Password)
            }).ToList();

            // Зберігаємо зашифровану копію у файл
            StoreForMode().SaveAll(usersToSave);
        }

        public static string DiagnosticPath() => StoreForMode().FilePath;
    }
}
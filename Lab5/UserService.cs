using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lab5
{
    public static class UserService
    {
        private const string ManualFileName = "users_manual.txt";
        private const string SocialFileName = "users_social.txt";

        // Повертає шлях поруч із виконуваним файлом; файл може бути створений при першому записі
        private static string GetPath(string fileName)
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(baseDir, fileName);
        }

        // Забезпечує існування файлу з початковим прикладом (якщо його немає)
        private static void EnsureFileExistsWithSample(string fileName, List<User> sample)
        {
            var path = GetPath(fileName);
            if (!File.Exists(path))
            {
                try
                {
                    var json = JsonSerializer.Serialize(sample, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(path, json, Encoding.UTF8);
                }
                catch
                {
                    // ігноруємо помилку — метод використовується для зручності
                }
            }
        }

        // Завантажити список користувачів з файлу JSON (.txt який містить JSON)
        private static List<User> LoadFromJsonFile(string fileName, List<User>? createSampleIfMissing = null)
        {
            var path = GetPath(fileName);

            if (!File.Exists(path))
            {
                if (createSampleIfMissing != null)
                {
                    EnsureFileExistsWithSample(fileName, createSampleIfMissing);
                    return new List<User>(createSampleIfMissing);
                }
                return new List<User>();
            }

            try
            {
                var content = File.ReadAllText(path, Encoding.UTF8).Trim();
                if (string.IsNullOrEmpty(content))
                    return new List<User>();

                var list = JsonSerializer.Deserialize<List<User>>(content);
                return list ?? new List<User>();
            }
            catch
            {
                // Якщо файл існує, але містить некоректний JSON — повертаємо порожній список
                return new List<User>();
            }
        }

        // Зберегти список у файл (перезапис)
        private static void SaveToJsonFile(string fileName, List<User> users)
        {
            var path = GetPath(fileName);
            try
            {
                var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, json, Encoding.UTF8);
            }
            catch
            {
                // Ігноруємо помилки запису (у навчальному прикладі)
            }
        }

        // Публічні методи:

        // Завантажити усіх користувачів (manual + social)
        public static List<User> LoadUsers()
        {
            // За замовчуванням створимо приклади, якщо файлів немає
            var manualSample = new List<User> { new User { Username = "demo", Email = "demo@example.local", Password = "1234" } };
            var socialSample = new List<User>
            {
                new User { Username = "social_google", Email = "social_google@google.local", Password = "social" },
                new User { Username = "social_facebook", Email = "social_facebook@facebook.local", Password = "social" },
                new User { Username = "social_twitter", Email = "social_twitter@twitter.local", Password = "social" }
            };

            var manual = LoadFromJsonFile(ManualFileName, manualSample);
            var social = LoadFromJsonFile(SocialFileName, socialSample);

            var all = new List<User>();
            all.AddRange(manual);
            all.AddRange(social);
            return all;
        }

        // Знайти користувача по username або email (спочатку manual, потім social)
        public static User? FindByUsernameOrEmail(string login)
        {
            if (string.IsNullOrWhiteSpace(login)) return null;
            var trimmed = login.Trim();

            var manual = LoadFromJsonFile(ManualFileName);
            foreach (var u in manual)
            {
                if (string.Equals(u.Username?.Trim(), trimmed, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(u.Email?.Trim(), trimmed, StringComparison.OrdinalIgnoreCase))
                    return u;
            }

            var social = LoadFromJsonFile(SocialFileName);
            foreach (var u in social)
            {
                if (string.Equals(u.Username?.Trim(), trimmed, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(u.Email?.Trim(), trimmed, StringComparison.OrdinalIgnoreCase))
                    return u;
            }

            return null;
        }

        // Додати нового користувача у manual або social (перезаписує файл як JSON)
        public static void AddUser(User user, bool toSocial = false)
        {
            var file = toSocial ? SocialFileName : ManualFileName;
            var list = LoadFromJsonFile(file) ?? new List<User>();
            list.Add(user);
            SaveToJsonFile(file, list);
        }

        // Перезаписати manual-файл повністю (якщо потрібно)
        public static void SaveManualUsers(List<User> users)
        {
            SaveToJsonFile(ManualFileName, users ?? new List<User>());
        }

        // Перезаписати social-файл повністю (якщо потрібно)
        public static void SaveSocialUsers(List<User> users)
        {
            SaveToJsonFile(SocialFileName, users ?? new List<User>());
        }
    }
}
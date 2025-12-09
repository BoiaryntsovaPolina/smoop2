using System;
using System.IO;
using System.Text.Json;

namespace StoreApp.Services
{
    public enum AppMode { Unset, Home, Business }

    // Клас даних налаштувань
    public class AppSettings
    {
        public AppMode Mode { get; set; } = AppMode.Unset;
        public string LastUser { get; set; } = "";

        // Прапорець для першого запуску Business режиму
        public bool IsBusinessFirstRun { get; set; } = true;
    }

    // Сервіс для збереження налаштувань у файл
    public static class SettingsService
    {
        private static readonly string _dataDir;
        private static readonly string _path;

        // Статичний конструктор: запускається один раз при старті
        static SettingsService()
        {
            var baseDir = AppContext.BaseDirectory ?? Environment.CurrentDirectory;
            _dataDir = Path.Combine(baseDir, "Data");
            if (!Directory.Exists(_dataDir)) Directory.CreateDirectory(_dataDir);

            _path = Path.Combine(_dataDir, "config.json");

            // Пробуємо завантажити налаштування, якщо файл існує
            if (File.Exists(_path))
            {
                try
                {
                    var json = File.ReadAllText(_path);
                    Settings = JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
                }
                catch
                {
                    Settings = new AppSettings(); // Створюємо нові при помилці
                    Save();
                }
            }
            else
            {
                Settings = new AppSettings(); // Створюємо нові, якщо файлу немає
                Save();
            }
        }

        public static AppSettings Settings { get; private set; }

        // Збереження у файл
        public static void Save()
        {
            var json = JsonSerializer.Serialize(Settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_path, json);
        }

        // Зміна режиму
        public static void SetMode(AppMode mode)
        {
            Settings.Mode = mode;
            Save();
        }

        public static string DataDirectory => _dataDir;
    }
}
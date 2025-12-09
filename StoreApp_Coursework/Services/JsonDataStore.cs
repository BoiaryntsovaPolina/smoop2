using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace StoreApp.Services
{
    // Клас для читання/запису даних у JSON файли
    public class JsonDataStore<T>
    {
        private readonly string _path;

        public JsonDataStore(string filename)
        {
            // Знаходимо папку поруч із .exe файлом
            var baseDir = AppContext.BaseDirectory ?? Environment.CurrentDirectory;
            var dataDir = Path.Combine(baseDir, "Data");

            // Створюємо папку Data, якщо її немає
            if (!Directory.Exists(dataDir)) Directory.CreateDirectory(dataDir);

            _path = Path.Combine(dataDir, filename);
        }

        // Отримати всі об'єкти з файлу
        public IEnumerable<T> GetAll()
        {
            try
            {
                if (!File.Exists(_path)) return new List<T>();
                var json = File.ReadAllText(_path);
                return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            }
            catch
            {
                return new List<T>();
            }
        }

        // Зберегти список у файл
        public void SaveAll(IEnumerable<T> items)
        {
            var json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_path, json);
        }

        public string FilePath => _path;
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Modultask2
{
    public static class JsonDataService
    {
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        // Load from .txt file (which contains JSON array) using StreamReader
        public static List<T> LoadFromTxt<T>(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"Файл не знайдено: {path}");

            using (var sr = new StreamReader(path))
            {
                string json = sr.ReadToEnd();
                var list = JsonSerializer.Deserialize<List<T>>(json, _jsonOptions);
                return list ?? new List<T>();
            }
        }

        // Save list to .txt (JSON inside) using StreamWriter
        public static void SaveToTxt<T>(string path, List<T> list)
        {
            var dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrWhiteSpace(dir))
                Directory.CreateDirectory(dir);

            using (var sw = new StreamWriter(path, false))
            {
                string json = JsonSerializer.Serialize(list, _jsonOptions);
                sw.Write(json);
            }
        }

        // Generate sample data using constructors (returns in-memory lists)
        public static (List<Worker> workers, List<SalaryRecord> salaries) GenerateSampleData()
        {
            var workers = new List<Worker>
            {
                new Worker("W001", "Іваненко І.М.", "Вища", "Програміст", 1985),
                new Worker("W002", "Петренко О.С.", "Середня", "Економіст", 1992),
                new Worker("W003", "Сидорук Н.В.", "Вища", "Менеджер", 1979),
                new Worker("W004", "Коваленко П.П.", "Неповна вища", "Технік", 1990),
                new Worker("W005", "Гончаренко А.Б.", "Вища", "Аналітик", 1988)
            };

            var salaries = new List<SalaryRecord>
            {
                new SalaryRecord("W001", 12000.50m, 12500.75m),
                new SalaryRecord("W002", 8000m, 8200m),
                new SalaryRecord("W003", 15000m, 15500m),
                new SalaryRecord("W004", 7000m, 7100m),
                new SalaryRecord("W005", 11000m, 11500m)
            };

            return (workers, salaries);
        }
    }
}

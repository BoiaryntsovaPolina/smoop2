using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1._2
{
    internal class TextAnalyzer
    {
        private FileManager fileManager;
        private WordCounter wordCounter;
        private const string FirstFileName = "firstFile.txt";

        public TextAnalyzer()
        {
            fileManager = new FileManager();
            wordCounter = new WordCounter();
        }

        public void StartAnalysis()
        {
            // Перевіряємо наявність файлу з переліком текстових файлів
            if (!File.Exists(FirstFileName))
            {
                Console.WriteLine($"Помилка: Файл {FirstFileName} не знайдено!");
                Console.WriteLine("Створіть файл firstFile.txt з переліком текстових файлів для аналізу.");
                return;
            }

            // Зчитуємо список файлів
            List<string> availableFiles = fileManager.ReadFileList(FirstFileName);

            if (availableFiles.Count == 0)
            {
                Console.WriteLine("У файлі firstFile.txt немає переліку файлів для аналізу.");
                return;
            }

            bool continueAnalysis = true;

            while (continueAnalysis)
            {
                // Показуємо доступні файли
                DisplayAvailableFiles(availableFiles);

                // Отримуємо вибір користувача
                int fileChoice = GetUserFileChoice(availableFiles.Count);

                if (fileChoice == -1)
                {
                    continueAnalysis = false;
                    continue;
                }

                string selectedFile = availableFiles[fileChoice];

                // Аналізуємо вибраний файл
                AnalyzeFile(selectedFile);

                // Запитуємо, чи продовжувати аналіз
                continueAnalysis = AskContinue();
            }
        }

        private void DisplayAvailableFiles(List<string> files)
        {
            Console.WriteLine("\n=== Доступні файли для аналізу ===");
            for (int i = 0; i < files.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {files[i]}");
            }
            Console.WriteLine("0. Вихід з програми");
        }

        private int GetUserFileChoice(int maxChoice)
        {
            while (true)
            {
                Console.Write($"\nОберіть файл для аналізу (1-{maxChoice}) або 0 для виходу: ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int choice))
                {
                    if (choice == 0)
                        return -1; // Сигнал для виходу

                    if (choice >= 1 && choice <= maxChoice)
                        return choice - 1; // Повертаємо індекс масиву (0-based)
                }

                Console.WriteLine("Некоректний вибір. Спробуйте ще раз.");
            }
        }

        private void AnalyzeFile(string fileName)
        {
            Console.WriteLine($"\n=== Аналіз файлу: {fileName} ===");

            // Перевіряємо наявність файлу
            if (!File.Exists(fileName))
            {
                Console.WriteLine($"Помилка: Файл {fileName} не існує!");
                return;
            }

            // Зчитуємо текст з файлу
            string fileContent = fileManager.ReadTextFile(fileName);

            if (string.IsNullOrEmpty(fileContent))
            {
                Console.WriteLine("Файл порожній або не може бути прочитаний.");
                return;
            }

            // Підраховуємо слова
            Dictionary<string, int> wordStatistics = wordCounter.CountWords(fileContent);

            // Виводимо статистику на екран
            DisplayStatistics(wordStatistics, fileName);

            // Запитуємо, чи зберегти результат у файл
            if (AskSaveToFile())
            {
                string outputFileName = GenerateOutputFileName(fileName);
                SaveStatisticsToFile(wordStatistics, outputFileName, fileName);
                Console.WriteLine($"Статистику збережено у файл: {outputFileName}");
            }
        }

        private void DisplayStatistics(Dictionary<string, int> statistics, string fileName)
        {
            Console.WriteLine($"\nСтатистика використання слів у файлі {fileName}:");
            Console.WriteLine(new string('=', 60));
            Console.WriteLine($"{"Слово",-25} {"Кількість",10}");
            Console.WriteLine(new string('-', 60));

            // Копіюємо у список і сортуємо ефективно (O(m log m))
            var sortedWords = new List<KeyValuePair<string, int>>(statistics);
            sortedWords.Sort((a, b) => string.Compare(a.Key, b.Key, StringComparison.CurrentCulture));

            // Виводимо відсортовані результати
            foreach (var word in sortedWords)
            {
                Console.WriteLine($"{word.Key,-25} {word.Value,10}");
            }

            Console.WriteLine("-" + new string('-', 50));
            Console.WriteLine($"Загальна кількість унікальних слів: {statistics.Count}");
        }

        private bool AskSaveToFile()
        {
            Console.Write("\nЗберегти статистику у файл? (y/n): ");
            string response = Console.ReadLine()?.ToLower();
            return response == "y" || response == "yes" || response == "так" || response == "т";
        }

        private string GenerateOutputFileName(string originalFileName)
        {
            string nameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);
            return $"{nameWithoutExtension}_statistics.txt";
        }

        private void SaveStatisticsToFile(Dictionary<string, int> statistics, string outputFileName, string originalFileName)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(outputFileName, false, System.Text.Encoding.UTF8))
                {
                    writer.WriteLine($"Статистика використання слів у файлі: {originalFileName}");
                    writer.WriteLine($"Дата аналізу: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    writer.WriteLine(new string('=', 60));
                    writer.WriteLine($"{"Слово",-25} {"Кількість",10}");
                    writer.WriteLine(new string('-', 60));

                    // Копіюємо у список і сортуємо ефективно (O(m log m))
                    var sortedWords = new List<KeyValuePair<string, int>>(statistics);
                    sortedWords.Sort((a, b) => string.Compare(a.Key, b.Key, StringComparison.CurrentCulture));

                    foreach (var word in sortedWords)
                    {
                        writer.WriteLine($"{word.Key,-25} {word.Value,10}");
                    }

                    writer.WriteLine(new string('-', 60));
                    writer.WriteLine($"Загальна кількість унікальних слів: {statistics.Count}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при збереженні файлу: {ex.Message}");
            }
        }

        private bool AskContinue()
        {
            Console.Write("\nПродовжити аналіз інших файлів? (y/n): ");
            string response = Console.ReadLine()?.ToLower();
            return response == "y" || response == "yes" || response == "так" || response == "т";
        }
    }
}

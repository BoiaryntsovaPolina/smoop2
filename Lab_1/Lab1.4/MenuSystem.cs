using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1._4
{
    internal class MenuSystem
    {
        private DictionaryManager manager;

        public MenuSystem()
        {
            manager = new DictionaryManager();
        }

        public async Task RunAsync()
        {
            while (true)
            {
                ShowMainMenu();
                string choice = GetUserInput("Виберіть опцію: ");

                switch (choice)
                {
                    case "1":
                        await CreateDictionaryMenuAsync();
                        break;
                    case "2":
                        await SelectDictionaryMenuAsync();
                        break;
                    case "3":
                        await manager.DisplayAllDictionariesAsync();
                        PauseForUser();
                        break;
                    case "4":
                        SearchInAllDictionariesMenu();
                        break;
                    case "5":
                        await RemoveDictionaryMenuAsync();
                        break;
                    case "6":
                        await ImportDictionaryMenuAsync();
                        break;
                    case "7":
                        manager.DisplayStatistics();
                        PauseForUser();
                        break;
                    case "8":
                        await manager.SaveAllDictionariesAsync();
                        PauseForUser();
                        break;
                    case "0":
                        Console.WriteLine("До побачення!");
                        await manager.SaveAllDictionariesAsync();
                        return;
                    default:
                        Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                        PauseForUser();
                        break;
                }
            }
        }


        private void ShowMainMenu()
        {
            ClearScreen();
            Console.WriteLine("=== СИСТЕМА СЛОВНИКІВ ===");
            Console.WriteLine("1. Створити новий словник");
            Console.WriteLine("2. Працювати зі словником");
            Console.WriteLine("3. Переглянути всі словники");
            Console.WriteLine("4. Шукати в усіх словниках");
            Console.WriteLine("5. Видалити словник");
            Console.WriteLine("6. Імпортувати словник з файлу");
            Console.WriteLine("7. Переглянути статистику");
            Console.WriteLine("8. Зберегти всі зміни");
            Console.WriteLine("0. Вихід");
            Console.WriteLine("========================");
        }


        private async Task CreateDictionaryMenuAsync()
        {
            ClearScreen();
            Console.WriteLine("=== СТВОРЕННЯ НОВОГО СЛОВНИКА ===");

            string name = GetUserInput("Введіть назву словника: ");
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Назва не може бути порожньою!");
                PauseForUser();
                return;
            }

            string sourceLanguage = GetUserInput("Введіть мову оригіналу (наприклад, українська): ");
            if (string.IsNullOrWhiteSpace(sourceLanguage))
            {
                Console.WriteLine("Мова оригіналу не може бути порожньою!");
                PauseForUser();
                return;
            }

            string targetLanguage = GetUserInput("Введіть мову перекладу (наприклад, англійська): ");
            if (string.IsNullOrWhiteSpace(targetLanguage))
            {
                Console.WriteLine("Мова перекладу не може бути порожньою!");
                PauseForUser();
                return;
            }

            if (await manager.CreateDictionaryAsync(name, sourceLanguage, targetLanguage))
            {
                Console.WriteLine("Словник успішно створено!");
            }

            PauseForUser();
        }


        private async Task SelectDictionaryMenuAsync()
        {
            ClearScreen();
            Console.WriteLine("=== ВИБІР СЛОВНИКА ===");

            List<Dictionary> dictionaries = await manager.GetAllDictionariesAsync();
            if (dictionaries.Count == 0)
            {
                Console.WriteLine("Немає доступних словників. Спочатку створіть словник.");
                PauseForUser();
                return;
            }

            Console.WriteLine("Доступні словники:");
            for (int i = 0; i < dictionaries.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {dictionaries[i].Name}");
                Console.WriteLine($"   Тип: {dictionaries[i].SourceLanguage} → {dictionaries[i].TargetLanguage}");
                Console.WriteLine($"   Файл: {dictionaries[i].FileName}");
                Console.WriteLine($"   Слів: {dictionaries[i].GetWordCount()}");
                Console.WriteLine();
            }

            string choice = GetUserInput("Введіть номер словника: ");

            Dictionary selectedDictionary = null;

            if (int.TryParse(choice, out int index))
            {
                if (index >= 1 && index <= dictionaries.Count)
                {
                    selectedDictionary = dictionaries[index - 1];
                }
            }

            if (selectedDictionary == null)
            {
                Console.WriteLine("Невірний номер словника!");
                PauseForUser();
                return;
            }

            await WorkWithDictionaryMenuAsync(selectedDictionary);
        }


        private async Task WorkWithDictionaryMenuAsync(Dictionary dictionary)
        {
            while (true)
            {
                ClearScreen();
                Console.WriteLine($"=== РОБОТА ЗІ СЛОВНИКОМ: {dictionary.Name} ===");
                Console.WriteLine($"Тип: {dictionary.SourceLanguage} → {dictionary.TargetLanguage}");
                Console.WriteLine($"Файл: {dictionary.FileName}");
                Console.WriteLine($"Слів у словнику: {dictionary.GetWordCount()}");
                Console.WriteLine();
                Console.WriteLine("1. Додати слово");
                Console.WriteLine("2. Знайти переклад");
                Console.WriteLine("3. Замінити слово");
                Console.WriteLine("4. Замінити переклад");
                Console.WriteLine("5. Видалити слово");
                Console.WriteLine("6. Видалити переклад");
                Console.WriteLine("7. Показати всі слова");
                Console.WriteLine("8. Експортувати слово");
                Console.WriteLine("9. Зберегти словник");
                Console.WriteLine("0. Повернутися до головного меню");
                Console.WriteLine("========================================");

                string choice = GetUserInput("Виберіть опцію: ");

                switch (choice)
                {
                    case "1":
                        AddWordMenu(dictionary);
                        break;
                    case "2":
                        FindTranslationMenu(dictionary);
                        break;
                    case "3":
                        ReplaceWordMenu(dictionary);
                        break;
                    case "4":
                        ReplaceTranslationMenu(dictionary);
                        break;
                    case "5":
                        RemoveWordMenu(dictionary);
                        break;
                    case "6":
                        RemoveTranslationMenu(dictionary);
                        break;
                    case "7":
                        ShowAllWordsMenu(dictionary);
                        break;
                    case "8":
                        ExportWordMenu(dictionary);
                        break;
                    case "9":
                        await dictionary.SaveToFileAsync();
                        PauseForUser();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                        PauseForUser();
                        break;
                }
            }
        }

        private void AddWordMenu(Dictionary dictionary)
        {
            ClearScreen();
            Console.WriteLine("=== ДОДАВАННЯ СЛОВА ===");

            string word = GetUserInput("Введіть слово: ");
            if (string.IsNullOrWhiteSpace(word))
            {
                Console.WriteLine("Слово не може бути порожнім!");
                PauseForUser();
                return;
            }

            string translation = GetUserInput("Введіть переклад: ");
            if (string.IsNullOrWhiteSpace(translation))
            {
                Console.WriteLine("Переклад не може бути порожнім!");
                PauseForUser();
                return;
            }

            try
            {
                dictionary.AddWord(word, translation);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }

            PauseForUser();
        }

        private void FindTranslationMenu(Dictionary dictionary)
        {
            ClearScreen();
            Console.WriteLine("=== ПОШУК ПЕРЕКЛАДУ ===");

            string word = GetUserInput("Введіть слово для пошуку: ");
            if (string.IsNullOrWhiteSpace(word))
            {
                Console.WriteLine("Слово не може бути порожнім!");
                PauseForUser();
                return;
            }

            List<string> translations = dictionary.FindTranslations(word);
            if (translations.Count == 0)
            {
                Console.WriteLine($"Слово '{word}' не знайдено в словнику.");
            }
            else
            {
                Console.WriteLine($"Переклади для слова '{word}':");
                for (int i = 0; i < translations.Count; i++)
                {
                    Console.WriteLine($"  {i + 1}. {translations[i]}");
                }
            }

            PauseForUser();
        }


        private void ReplaceWordMenu(Dictionary dictionary)
        {
            ClearScreen();
            Console.WriteLine("=== ЗАМІНА СЛОВА ===");

            string oldWord = GetUserInput("Введіть слово для заміни: ");
            if (string.IsNullOrWhiteSpace(oldWord))
            {
                Console.WriteLine("Слово не може бути порожнім!");
                PauseForUser();
                return;
            }

            if (!dictionary.ContainsWord(oldWord))
            {
                Console.WriteLine($"Слово '{oldWord}' не знайдено в словнику.");
                PauseForUser();
                return;
            }

            // Показуємо поточні переклади
            List<string> translations = dictionary.FindTranslations(oldWord);
            Console.WriteLine($"Поточні переклади слова '{oldWord}':");
            for (int i = 0; i < translations.Count; i++)
            {
                Console.WriteLine($"  {i + 1}. {translations[i]}");
            }
            Console.WriteLine();

            string newWord = GetUserInput("Введіть нове слово: ");
            if (string.IsNullOrWhiteSpace(newWord))
            {
                Console.WriteLine("Нове слово не може бути порожнім!");
                PauseForUser();
                return;
            }

            if (dictionary.ReplaceWord(oldWord, newWord))
            {
                Console.WriteLine($"Слово '{oldWord}' успішно замінено на '{newWord}'");
            }
            else
            {
                Console.WriteLine("Не вдалося замінити слово");
            }

            PauseForUser();
        }


        private void ReplaceTranslationMenu(Dictionary dictionary)
        {
            ClearScreen();
            Console.WriteLine("=== ЗАМІНА ПЕРЕКЛАДУ ===");

            string word = GetUserInput("Введіть слово: ");
            if (string.IsNullOrWhiteSpace(word))
            {
                Console.WriteLine("Слово не може бути порожнім!");
                PauseForUser();
                return;
            }

            List<string> translations = dictionary.FindTranslations(word);
            if (translations.Count == 0)
            {
                Console.WriteLine($"Слово '{word}' не знайдено в словнику.");
                PauseForUser();
                return;
            }

            Console.WriteLine($"Поточні переклади слова '{word}':");
            for (int i = 0; i < translations.Count; i++)
            {
                Console.WriteLine($"  {i + 1}. {translations[i]}");
            }

            string oldTranslation = GetUserInput("Введіть переклад для заміни: ");
            if (string.IsNullOrWhiteSpace(oldTranslation))
            {
                Console.WriteLine("Переклад не може бути порожнім!");
                PauseForUser();
                return;
            }

            string newTranslation = GetUserInput("Введіть новий переклад: ");
            if (string.IsNullOrWhiteSpace(newTranslation))
            {
                Console.WriteLine("Новий переклад не може бути порожнім!");
                PauseForUser();
                return;
            }

            if (dictionary.ReplaceTranslation(word, oldTranslation, newTranslation))
            {
                Console.WriteLine($"Переклад замінено: '{oldTranslation}' → '{newTranslation}'");
            }
            else
            {
                Console.WriteLine($"Переклад '{oldTranslation}' не знайдено");
            }

            PauseForUser();
        }

        private void RemoveWordMenu(Dictionary dictionary)
        {
            ClearScreen();
            Console.WriteLine("=== ВИДАЛЕННЯ СЛОВА ===");

            string word = GetUserInput("Введіть слово для видалення: ");
            if (string.IsNullOrWhiteSpace(word))
            {
                Console.WriteLine("Слово не може бути порожнім!");
                PauseForUser();
                return;
            }

            List<string> translations = dictionary.FindTranslations(word);
            if (translations.Count == 0)
            {
                Console.WriteLine($"Слово '{word}' не знайдено в словнику.");
                PauseForUser();
                return;
            }

            Console.WriteLine($"Слово '{word}' має такі переклади:");
            for (int i = 0; i < translations.Count; i++)
            {
                Console.WriteLine($"  {i + 1}. {translations[i]}");
            }

            string confirm = GetUserInput($"\nВи впевнені, що хочете видалити слово '{word}' з усіма перекладами? (y/n): ");
            if (confirm.ToLower() == "y" || confirm.ToLower() == "yes" || confirm.ToLower() == "так")
            {
                if (dictionary.RemoveWord(word))
                {
                    Console.WriteLine($"Слово '{word}' видалено");
                }
                else
                {
                    Console.WriteLine("Не вдалося видалити слово");
                }
            }
            else
            {
                Console.WriteLine("Видалення скасовано");
            }

            PauseForUser();
        }


        private void RemoveTranslationMenu(Dictionary dictionary)
        {
            ClearScreen();
            Console.WriteLine("=== ВИДАЛЕННЯ ПЕРЕКЛАДУ ===");

            string word = GetUserInput("Введіть слово: ");
            if (string.IsNullOrWhiteSpace(word))
            {
                Console.WriteLine("Слово не може бути порожнім!");
                PauseForUser();
                return;
            }

            List<string> translations = dictionary.FindTranslations(word);
            if (translations.Count == 0)
            {
                Console.WriteLine($"Слово '{word}' не знайдено в словнику.");
                PauseForUser();
                return;
            }

            Console.WriteLine($"Переклади слова '{word}':");
            for (int i = 0; i < translations.Count; i++)
            {
                Console.WriteLine($"  {i + 1}. {translations[i]}");
            }

            string translation = GetUserInput("Введіть переклад для видалення: ");
            if (string.IsNullOrWhiteSpace(translation))
            {
                Console.WriteLine("Переклад не може бути порожнім!");
                PauseForUser();
                return;
            }

            if (dictionary.RemoveTranslation(word, translation))
            {
                Console.WriteLine($"Переклад '{translation}' видалено");
            }
            else
            {
                Console.WriteLine($"Не вдалося видалити переклад '{translation}'");
            }

            PauseForUser();
        }


        private void ShowAllWordsMenu(Dictionary dictionary)
        {
            ClearScreen();
            Console.WriteLine("=== ВСІ СЛОВА СЛОВНИКА ===");
            dictionary.DisplayAllWords();
            PauseForUser();
        }


        private void ExportWordMenu(Dictionary dictionary)
        {
            ClearScreen();
            Console.WriteLine("=== ЕКСПОРТ СЛОВА ===");

            string word = GetUserInput("Введіть слово для експорту: ");
            if (string.IsNullOrWhiteSpace(word))
            {
                Console.WriteLine("Слово не може бути порожнім!");
                PauseForUser();
                return;
            }

            if (!dictionary.ContainsWord(word))
            {
                Console.WriteLine($"Слово '{word}' не знайдено в словнику.");
                PauseForUser();
                return;
            }

            // Генеруємо ім'я файлу
            string exportFileName = $"export_{word}_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

            string customFileName = GetUserInput($"Ім'я файлу [{exportFileName}]: ");
            if (!string.IsNullOrWhiteSpace(customFileName))
            {
                exportFileName = customFileName;
            }

            dictionary.ExportWord(word, exportFileName);
            PauseForUser();
        }


        private void SearchInAllDictionariesMenu()
        {
            ClearScreen();
            Console.WriteLine("=== ПОШУК В УСІХ СЛОВНИКАХ ===");

            string word = GetUserInput("Введіть слово для пошуку: ");
            if (string.IsNullOrWhiteSpace(word))
            {
                Console.WriteLine("Слово не може бути порожнім!");
                PauseForUser();
                return;
            }

            manager.SearchInAllDictionaries(word);
            PauseForUser();
        }


        private async Task RemoveDictionaryMenuAsync()
        {
            ClearScreen();
            Console.WriteLine("=== ВИДАЛЕННЯ СЛОВНИКА ===");

            List<Dictionary> dictionaries = await manager.GetAllDictionariesAsync();
            if (dictionaries.Count == 0)
            {
                Console.WriteLine("Немає доступних словників.");
                PauseForUser();
                return;
            }

            Console.WriteLine("Доступні словники:");
            for (int i = 0; i < dictionaries.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {dictionaries[i].Name}");
                Console.WriteLine($"   Тип: {dictionaries[i].SourceLanguage} → {dictionaries[i].TargetLanguage}");
                Console.WriteLine($"   Файл: {dictionaries[i].FileName}");
                Console.WriteLine($"   Слів: {dictionaries[i].GetWordCount()}");
                Console.WriteLine();
            }

            string choice = GetUserInput("Введіть номер словника для видалення: ");

            Dictionary selectedDictionary = null;

            if (int.TryParse(choice, out int index))
            {
                if (index >= 1 && index <= dictionaries.Count)
                {
                    selectedDictionary = dictionaries[index - 1];
                }
            }

            if (selectedDictionary == null)
            {
                Console.WriteLine("Невірний номер словника!");
                PauseForUser();
                return;
            }

            Console.WriteLine("Інформація про словник:");
            selectedDictionary.DisplayInfo();

            string confirm = GetUserInput($"\nВи впевнені, що хочете видалити словник '{selectedDictionary.Name}'? (y/n): ");

            if (confirm.ToLower() == "y" || confirm.ToLower() == "yes" || confirm.ToLower() == "так")
            {
                if (await manager.RemoveDictionaryAsync(selectedDictionary.Name))
                {
                    Console.WriteLine("Словник успішно видалено!");
                }
            }
            else
            {
                Console.WriteLine("Видалення скасовано");
            }

            PauseForUser();
        }

        private async Task ImportDictionaryMenuAsync()
        {
            ClearScreen();
            Console.WriteLine("=== ІМПОРТ СЛОВНИКА ===");

            string fileName = GetUserInput("Введіть ім'я файлу для імпорту: ");
            if (string.IsNullOrWhiteSpace(fileName))
            {
                Console.WriteLine("Ім'я файлу не може бути порожнім!");
                PauseForUser();
                return;
            }

            if (await manager.ImportDictionaryAsync(fileName))
            {
                Console.WriteLine("Словник успішно імпортовано!");
            }

            PauseForUser();
        }

        private string GetUserInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine() ?? string.Empty;
        }


        private void PauseForUser()
        {
            Console.WriteLine("\nНатисніть Enter для продовження...");
            Console.ReadLine();
        }

        private void ClearScreen()
        {
            Console.Clear();
        }
    }
}

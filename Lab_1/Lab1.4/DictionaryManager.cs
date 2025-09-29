using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lab1._4
{
    internal class DictionaryManager
    {
        // Список усіх доступних словників
        private List<Dictionary> dictionaries;

        // Файл для зберігання списку словників
        private const string DICTIONARIES_INDEX_FILE = "dictionaries_index.dat";

        public DictionaryManager()
        {
            dictionaries = new List<Dictionary>();
            LoadDictionariesIndexAsync();
        }

        public async Task<bool> CreateDictionaryAsync(string name, string sourceLanguage, string targetLanguage)
        {
            if (FindDictionaryByName(name) != null)
            {
                Console.WriteLine($"Словник з назвою '{name}' вже існує");
                return false;
            }

            Dictionary newDictionary = new Dictionary(name, sourceLanguage, targetLanguage);
            dictionaries.Add(newDictionary);

            await newDictionary.SaveToFileAsync();
            await SaveDictionariesIndexAsync();

            Console.WriteLine($"Створено новий словник: {name} ({sourceLanguage} → {targetLanguage})");
            return true;
        }


        public Dictionary FindDictionaryByName(string name)
        {
            foreach (Dictionary dict in dictionaries)
            {
                if (dict.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return dict;
                }
            }
            return null;
        }


        public async Task<List<Dictionary>> GetAllDictionariesAsync()
        {
            await Task.CompletedTask; // Для асинхронності
            return new List<Dictionary>(dictionaries);
        }


        public async Task<bool> RemoveDictionaryAsync(string name)
        {
            Dictionary dictionary = FindDictionaryByName(name);
            if (dictionary == null)
            {
                Console.WriteLine($"Словник '{name}' не знайдено");
                return false;
            }

            try
            {
                if (File.Exists(dictionary.FileName))
                {
                    File.Delete(dictionary.FileName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при видаленні файлу: {ex.Message}");
            }

            dictionaries.Remove(dictionary);
            await SaveDictionariesIndexAsync();

            Console.WriteLine($"Словник '{name}' видалено");
            return true;
        }


        public async Task SaveAllDictionariesAsync()
        {
            foreach (Dictionary dictionary in dictionaries)
            {
                await dictionary.SaveToFileAsync();
            }
            await SaveDictionariesIndexAsync();
            Console.WriteLine("Всі словники збережено");
        }

        public async Task DisplayAllDictionariesAsync()
        {
            await Task.CompletedTask; // Для асинхронності

            if (dictionaries.Count == 0)
            {
                Console.WriteLine("Немає доступних словників");
                return;
            }

            Console.WriteLine("Доступні словники:");
            for (int i = 0; i < dictionaries.Count; i++)
            {
                Dictionary dict = dictionaries[i];
                Console.WriteLine($"{i + 1}. {dict.Name}");
                Console.WriteLine($"   Тип: {dict.SourceLanguage} → {dict.TargetLanguage}");
                Console.WriteLine($"   Слів: {dict.GetWordCount()}");
                Console.WriteLine($"   Файл: {dict.FileName}");
                Console.WriteLine();
            }
        }


        public void SearchInAllDictionaries(string word)
        {
            bool found = false;

            Console.WriteLine($"Пошук слова '{word}' в усіх словниках:");
            Console.WriteLine("=================================");

            foreach (Dictionary dictionary in dictionaries)
            {
                List<string> translations = dictionary.FindTranslations(word);
                if (translations.Count > 0)
                {
                    found = true;
                    Console.WriteLine($"Словник: {dictionary.Name} ({dictionary.SourceLanguage} → {dictionary.TargetLanguage})");
                    for (int i = 0; i < translations.Count; i++)
                    {
                        Console.WriteLine($"  {i + 1}. {translations[i]}");
                    }
                    Console.WriteLine();
                }
            }

            if (!found)
            {
                Console.WriteLine("Слово не знайдено в жодному словнику");
            }
        }


        private async Task SaveDictionariesIndexAsync()
        {
            try
            {
                var indexData = new
                {
                    Created = DateTime.Now,
                    Version = "1.0",
                    Dictionaries = dictionaries.ConvertAll(dict => new
                    {
                        Name = dict.Name,
                        SourceLanguage = dict.SourceLanguage,
                        TargetLanguage = dict.TargetLanguage,
                        FileName = dict.FileName,
                        CreatedDate = dict.CreatedDate,
                        LastModified = dict.LastModified,
                        WordCount = dict.GetWordCount()
                    })
                };

                using (FileStream fileStream = new FileStream(DICTIONARIES_INDEX_FILE, FileMode.Create))
                {
                    await JsonSerializer.SerializeAsync(fileStream, indexData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при збереженні індексного файлу: {ex.Message}");
            }
        }


        private async Task LoadDictionariesIndexAsync()
        {
            try
            {
                if (!File.Exists(DICTIONARIES_INDEX_FILE))
                {
                    return;
                }

                using (FileStream fileStream = new FileStream(DICTIONARIES_INDEX_FILE, FileMode.Open))
                {
                    var indexData = await JsonSerializer.DeserializeAsync<JsonElement>(fileStream);

                    if (indexData.TryGetProperty("dictionaries", out JsonElement dictionariesArray))
                    {
                        foreach (JsonElement dictInfo in dictionariesArray.EnumerateArray())
                        {
                            if (dictInfo.TryGetProperty("fileName", out JsonElement fileNameElement))
                            {
                                string fileName = fileNameElement.GetString();

                                Dictionary dictionary = await Dictionary.LoadFromFileAsync(fileName);
                                if (dictionary != null)
                                {
                                    dictionaries.Add(dictionary);
                                }
                                else if (dictInfo.TryGetProperty("name", out JsonElement nameElement) &&
                                        dictInfo.TryGetProperty("sourceLanguage", out JsonElement sourceElement) &&
                                        dictInfo.TryGetProperty("targetLanguage", out JsonElement targetElement))
                                {
                                    Console.WriteLine($"Файл словника {fileName} не знайдено. Створюємо новий порожній словник.");
                                    dictionary = new Dictionary(nameElement.GetString(), sourceElement.GetString(), targetElement.GetString());
                                    await dictionary.SaveToFileAsync();
                                    dictionaries.Add(dictionary);
                                }
                            }
                        }
                    }
                }

                Console.WriteLine($"Завантажено {dictionaries.Count} словників");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при завантаженні індексного файлу: {ex.Message}");
            }
        }


        public async Task<bool> ImportDictionaryAsync(string fileName)
        {
            Dictionary dictionary = await Dictionary.LoadFromFileAsync(fileName);
            if (dictionary == null)
            {
                return false;
            }

            if (FindDictionaryByName(dictionary.Name) != null)
            {
                Console.WriteLine($"Словник з назвою '{dictionary.Name}' вже існує");
                return false;
            }

            dictionaries.Add(dictionary);
            await SaveDictionariesIndexAsync();

            Console.WriteLine($"Словник '{dictionary.Name}' успішно імпортовано");
            return true;
        }


        public void DisplayStatistics()
        {
            Console.WriteLine("=== СТАТИСТИКА СЛОВНИКІВ ===");
            Console.WriteLine($"Загальна кількість словників: {dictionaries.Count}");

            if (dictionaries.Count == 0)
            {
                return;
            }

            int totalWords = 0;
            int totalTranslations = 0;

            Dictionary<string, int> languageStats = new Dictionary<string, int>();

            foreach (Dictionary dict in dictionaries)
            {
                int wordCount = dict.GetWordCount();
                totalWords += wordCount;

                List<string> words = dict.GetAllWords();
                foreach (string word in words)
                {
                    totalTranslations += dict.FindTranslations(word).Count;
                }

                string langPair = $"{dict.SourceLanguage} → {dict.TargetLanguage}";
                if (languageStats.ContainsKey(langPair))
                {
                    languageStats[langPair]++;
                }
                else
                {
                    languageStats[langPair] = 1;
                }
            }

            Console.WriteLine($"Загальна кількість слів: {totalWords}");
            Console.WriteLine($"Загальна кількість перекладів: {totalTranslations}");
            Console.WriteLine();
            Console.WriteLine("Розподіл по мовних парах:");

            foreach (var entry in languageStats)
            {
                Console.WriteLine($"  {entry.Key}: {entry.Value} словник(ів)");
            }
        }
    }
}

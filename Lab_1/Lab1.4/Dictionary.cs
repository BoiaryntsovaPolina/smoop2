using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lab1._4
{
    internal class Dictionary
    {
        public string Name { get; set; }
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModified { get; set; }

        // Основна структура даних: слово -> список перекладів
        public Dictionary<string, List<string>> Words { get; set; }

        // Ім'я файлу (не серіалізується)
        private string fileName;
        public string FileName
        {
            get { return fileName; }
            private set { fileName = value; }
        }


        public Dictionary()
        {
            Words = new Dictionary<string, List<string>>();
            CreatedDate = DateTime.Now;
            LastModified = DateTime.Now;
        }


        public Dictionary(string name, string sourceLanguage, string targetLanguage) : this()
        {
            Name = name;
            SourceLanguage = sourceLanguage;
            TargetLanguage = targetLanguage;
            FileName = GenerateFileName(name);
        }

        private string GenerateFileName(string name)
        {
            string fileName = name;
            char[] invalidChars = Path.GetInvalidFileNameChars();

            foreach (char invalidChar in invalidChars)
            {
                fileName = fileName.Replace(invalidChar, '_');
            }

            fileName = fileName.Replace(' ', '_');
            return fileName + ".json";
        }


        public void SetFileName()
        {
            FileName = GenerateFileName(Name);
        }

        // Оновлює дату останньої модифікації
        private void UpdateModifiedDate()
        {
            LastModified = DateTime.Now;
        }


        public void AddWord(string word, string translation)
        {
            word = word.Trim().ToLower();
            translation = translation.Trim();

            if (string.IsNullOrEmpty(word) || string.IsNullOrEmpty(translation))
            {
                throw new ArgumentException("Слово та переклад не можуть бути порожніми");
            }

            if (Words.ContainsKey(word))
            {
                if (!Words[word].Contains(translation))
                {
                    Words[word].Add(translation);
                    Console.WriteLine($"Додано новий переклад для слова '{word}': {translation}");
                    UpdateModifiedDate();
                }
                else
                {
                    Console.WriteLine($"Переклад '{translation}' для слова '{word}' вже існує");
                }
            }
            else
            {
                Words[word] = new List<string> { translation };
                Console.WriteLine($"Додано нове слово '{word}' з перекладом: {translation}");
                UpdateModifiedDate();
            }
        }


        public List<string> FindTranslations(string word)
        {
            word = word.Trim().ToLower();

            if (Words.ContainsKey(word))
            {
                return new List<string>(Words[word]);
            }

            return new List<string>();
        }


        public bool ReplaceWord(string oldWord, string newWord)
        {
            oldWord = oldWord.Trim().ToLower();
            newWord = newWord.Trim().ToLower();

            if (!Words.ContainsKey(oldWord))
            {
                return false;
            }

            if (Words.ContainsKey(newWord))
            {
                Console.WriteLine($"Слово '{newWord}' вже існує в словнику");
                return false;
            }

            List<string> translations = Words[oldWord];
            Words.Remove(oldWord);
            Words[newWord] = translations;

            UpdateModifiedDate();
            return true;
        }


        public bool ReplaceTranslation(string word, string oldTranslation, string newTranslation)
        {
            word = word.Trim().ToLower();

            if (!Words.ContainsKey(word))
            {
                return false;
            }

            List<string> translations = Words[word];

            for (int i = 0; i < translations.Count; i++)
            {
                if (translations[i] == oldTranslation)
                {
                    translations[i] = newTranslation.Trim();
                    UpdateModifiedDate();
                    return true;
                }
            }

            return false;
        }


        public bool RemoveWord(string word)
        {
            word = word.Trim().ToLower();
            bool removed = Words.Remove(word);
            if (removed)
            {
                UpdateModifiedDate();
            }
            return removed;
        }


        public bool RemoveTranslation(string word, string translation)
        {
            word = word.Trim().ToLower();

            if (!Words.ContainsKey(word))
            {
                return false;
            }

            List<string> translations = Words[word];

            if (translations.Count <= 1)
            {
                Console.WriteLine("Неможливо видалити останній переклад. Видаліть слово повністю.");
                return false;
            }

            bool removed = translations.Remove(translation);
            if (removed)
            {
                UpdateModifiedDate();
            }
            return removed;
        }


        public List<string> GetAllWords()
        {
            List<string> allWords = new List<string>();

            foreach (string word in Words.Keys)
            {
                allWords.Add(word);
            }

            return allWords;
        }

        public int GetWordCount()
        {
            return Words.Count;
        }

        public bool ContainsWord(string word)
        {
            return Words.ContainsKey(word.Trim().ToLower());
        }


        public async Task SaveToFileAsync()
        {
            try
            {
                using (FileStream fileStream = new FileStream(FileName, FileMode.Create))
                {
                    await JsonSerializer.SerializeAsync(fileStream, this);
                }

                Console.WriteLine($"Словник збережено у файл: {FileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при збереженні словника: {ex.Message}");
            }
        }

        public static async Task<Dictionary> LoadFromFileAsync(string fileName)
        {
            try
            {
                if (!File.Exists(fileName))
                {
                    Console.WriteLine($"Файл {fileName} не знайдено");
                    return null;
                }

                using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
                {
                    Dictionary dictionary = await JsonSerializer.DeserializeAsync<Dictionary>(fileStream);
                    dictionary.FileName = fileName;

                    Console.WriteLine($"Словник '{dictionary.Name}' завантажено з файлу: {fileName}");
                    return dictionary;
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Помилка формату JSON: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при завантаженні словника: {ex.Message}");
                return null;
            }
        }


        public void ExportWord(string word, string exportFileName)
        {
            word = word.Trim().ToLower();

            if (!Words.ContainsKey(word))
            {
                Console.WriteLine($"Слово '{word}' не знайдено в словнику");
                return;
            }

            try
            {
                using (FileStream fileStream = new FileStream(exportFileName, FileMode.Create))
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    writer.WriteLine("=== ЕКСПОРТ СЛОВА ===");
                    writer.WriteLine($"Словник: {Name}");
                    writer.WriteLine($"Тип: {SourceLanguage} → {TargetLanguage}");
                    writer.WriteLine($"Дата експорту: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    writer.WriteLine();
                    writer.WriteLine($"Слово: {word}");
                    writer.WriteLine("Переклади:");

                    List<string> translations = Words[word];
                    for (int i = 0; i < translations.Count; i++)
                    {
                        writer.WriteLine($"  {i + 1}. {translations[i]}");
                    }

                    writer.WriteLine();
                    writer.WriteLine("=== КІНЕЦЬ ЕКСПОРТУ ===");
                }

                Console.WriteLine($"Слово '{word}' експортовано у файл: {exportFileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при експорті: {ex.Message}");
            }
        }


        public void DisplayInfo()
        {
            Console.WriteLine($"Назва: {Name}");
            Console.WriteLine($"Мови: {SourceLanguage} → {TargetLanguage}");
            Console.WriteLine($"Кількість слів: {Words.Count}");
            Console.WriteLine($"Файл: {FileName}");
            Console.WriteLine($"Створено: {CreatedDate:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"Останні зміни: {LastModified:yyyy-MM-dd HH:mm:ss}");
        }


        public void DisplayAllWords()
        {
            if (Words.Count == 0)
            {
                Console.WriteLine("Словник порожній");
                return;
            }

            Console.WriteLine($"Слова в словнику '{Name}':");
            int counter = 1;

            foreach (var entry in Words)
            {
                Console.WriteLine($"{counter}. {entry.Key}");
                foreach (string translation in entry.Value)
                {
                    Console.WriteLine($"   → {translation}");
                }
                counter++;
            }
        }




        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Dictionary other = (Dictionary)obj;

            return Name == other.Name &&
                   SourceLanguage == other.SourceLanguage &&
                   TargetLanguage == other.TargetLanguage;
        }


        public override int GetHashCode()
        {
            return HashCode.Combine(Name, SourceLanguage, TargetLanguage);
        }


        public override string ToString()
        {
            return $"Словник '{Name}' ({SourceLanguage} → {TargetLanguage}) - {Words.Count} слів";
        }
    }
}

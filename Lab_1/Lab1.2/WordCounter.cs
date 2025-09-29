using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lab1._2
{
    internal class WordCounter
    {
        public Dictionary<string, int> CountWords(string text)
        {
            var wordCount = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            if (string.IsNullOrEmpty(text))
                return wordCount;

            // Розбиваємо текст на слова
            List<string> words = ExtractWords(text);

            // Підраховуємо кожне слово
            foreach (string word in words)
            {
                if (!string.IsNullOrEmpty(word))
                {
                    if (wordCount.ContainsKey(word))
                    {
                        wordCount[word]++;
                    }
                    else
                    {
                        wordCount[word] = 1;
                    }
                }
            }
            return wordCount;
        }

      
        private List<string> ExtractWords(string text)
        {
            var words = new List<string>();

            if (string.IsNullOrEmpty(text))
                return words;

            // \p{L} - будь-яка літера
            // \p{Nd} - цифри
            var pattern = @"[\p{L}\p{Nd}'’\-]+";
            var matches = Regex.Matches(text, pattern);

            foreach (Match m in matches)
            {
                string token = m.Value.Trim();
                if (!string.IsNullOrEmpty(token))
                    words.Add(token);
            }

            return words;
        }

        public int GetTotalWordCount(Dictionary<string, int> wordStatistics)
        {
            int total = 0;
            foreach (var kvp in wordStatistics)
            {
                total += kvp.Value;
            }
            return total;
        }

        
        // Знаходить найчастіше вживане слово
        public KeyValuePair<string, int> GetMostFrequentWord(Dictionary<string, int> wordStatistics)
        {
            if (wordStatistics.Count == 0)
                return new KeyValuePair<string, int>("", 0);

            string mostFrequentWord = "";
            int maxCount = 0;

            foreach (var kvp in wordStatistics)
            {
                if (kvp.Value > maxCount)
                {
                    maxCount = kvp.Value;
                    mostFrequentWord = kvp.Key;
                }
            }

            return new KeyValuePair<string, int>(mostFrequentWord, maxCount);
        }
    }
}

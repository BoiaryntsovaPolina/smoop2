using System.Linq;

namespace StoreApp.Helpers
{
    public static class SecurityHelper
    {
        // Крок зсуву (ключ шифрування)
        private const int Shift = 5;

        // Метод шифрування
        public static string Encrypt(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            // Кожен символ зсуваємо на Shift вперед по таблиці Unicode
            return new string(input.Select(c => (char)(c + Shift)).ToArray());
        }

        // Метод дешифрування
        public static string Decrypt(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            // Кожен символ зсуваємо на Shift назад
            return new string(input.Select(c => (char)(c - Shift)).ToArray());
        }
    }
}
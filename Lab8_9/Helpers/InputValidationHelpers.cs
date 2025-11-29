using System.Text.RegularExpressions;

namespace Lab8.Helpers
{
    public static class InputValidationHelpers
    {
        // Дозволені символи у імені/прізвищі: пробіл, букви (включаючи діакритичні), апостроф, дефіс
        public static readonly Regex AllowedNameChars = new Regex(@"^[ \p{L}\p{M}'\-]+$", RegexOptions.Compiled);

        // Швидка перевірка при вставці/символі: шукаємо заборонені символи (будь-що, що не дозволено)
        public static readonly Regex ForbiddenInName = new Regex(@"[^ \p{L}\p{M}'\-]+", RegexOptions.Compiled);

        // Невід'ємні символи для чисел
        public static readonly Regex NonDigitRegex = new Regex("[^0-9]+", RegexOptions.Compiled);

        // Стать: одиночний символ Ч/Ж (регістр може бути будь-який)
        public static readonly Regex GenderSingleRegex = new Regex("^[ЧчЖж]$", RegexOptions.Compiled);

        /// <summary>
        /// Перевіряє коректність розташування дефіса у імені / прізвищі:
        /// - не може стояти на початку або в кінці рядка
        /// - не може бути двійного дефіса ("--")
        /// - не може бути дефіса поруч із пробілом ("- " або " -")
        /// - дефіс має бути між буквами (тобто перед ним та після нього має бути літера)
        /// Повертає true, якщо розташування дефіса коректне або дефісів немає.
        /// </summary>
        public static bool IsHyphenPlacementValid(string s)
        {
            if (string.IsNullOrEmpty(s)) return true;
            if (!s.Contains("-")) return true;
            if (s.StartsWith("-") || s.EndsWith("-")) return false;
            if (s.Contains("--")) return false;
            if (s.Contains("- ") || s.Contains(" -")) return false;

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '-')
                {
                    if (i - 1 < 0 || i + 1 >= s.Length) return false;
                    char left = s[i - 1];
                    char right = s[i + 1];
                    if (!char.IsLetter(left) || !char.IsLetter(right)) return false;
                }
            }

            return true;
        }
    }
}

using System;
using System.Globalization;
using System.Windows.Data;

namespace Lab7_WpfBinding.Converters
{
    // Конвертер int? <-> string для TextBox
    public class NullableIntConverter : IValueConverter
    {
        // int? -> string
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;
            if (value is int i) return i.ToString();
            if (value is int?) return ((int?)value)?.ToString() ?? string.Empty;
            return string.Empty;
        }

        // string -> int? (кидає FormatException при некоректному вводі)
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = value as string;
            if (string.IsNullOrWhiteSpace(s)) return null;
            if (int.TryParse(s, out var i)) return i;
            throw new FormatException("Введіть ціле число");
        }
    }
}

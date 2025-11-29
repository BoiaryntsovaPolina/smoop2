using System;
using System.Globalization;
using System.Windows.Data;

namespace Lab7_WpfBinding.Converters
{
    public class BoolToYesNoConverter : IValueConverter
    {
        // Перетворює bool / bool? -> "Так" / "Ні" / "Не вказано"
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b) return b ? "Так" : "Ні";
            if (value is bool?)
            {
                var nb = (bool?)value;
                return nb.HasValue ? (nb.Value ? "Так" : "Ні") : "Не вказано";
            }
            return "Не вказано";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Для простоти не використовуємо зворотне перетворення
            throw new NotImplementedException();
        }
    }
}

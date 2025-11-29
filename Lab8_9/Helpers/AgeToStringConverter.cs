using System;
using System.Globalization;
using System.Windows.Data;

namespace Lab8.Helpers
{
    public class AgeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int age)
            {
                string suffix;
                int lastTwo = age % 100;
                int last = age % 10;
                if (lastTwo >= 11 && lastTwo <= 14) suffix = "років";
                else if (last == 1) suffix = "рік";
                else if (last >= 2 && last <= 4) suffix = "роки";
                else suffix = "років";
                return $"{age} {suffix}";
            }
            return value?.ToString() ?? "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Binding.DoNothing;
    }
}

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Lab8.Helpers
{
    // Должен быть public - иначе XAML не сможет создать экземпляр в дизайнера/во время загрузки ресурсов
    public class DirtyAndValidToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 2) return Visibility.Collapsed;
            if (values[0] is bool isDirty && values[1] is bool isValid)
            {
                return (isDirty && isValid) ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}

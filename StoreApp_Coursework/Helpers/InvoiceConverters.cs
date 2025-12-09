using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using StoreApp.Models;

namespace StoreApp.Helpers
{
    // Конвертує Enum (Receipt/Issue) в український текст
    public class InvoiceTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is InvoiceType type)
            {
                return type == InvoiceType.Receipt ? "Прибуткова (Закупівля)" : "Видаткова (Продаж)";
            }
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    // Змінює колір тексту (зелений/червоний) та додає знак (+/-) для суми
    public class InvoiceTotalColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Якщо параметр "Color" - повертаємо колір, інакше - текст
            bool returnColor = (parameter as string) == "Color";

            if (value is Invoice inv)
            {
                // Видаткова (Issue) = Продаж = Гроші (+)
                if (inv.Type == InvoiceType.Issue)
                {
                    if (returnColor) return Brushes.Green;
                    return $"+ {inv.Total:F2} грн";
                }
                // Прибуткова (Receipt) = Закупка = Гроші (-)
                else
                {
                    if (returnColor) return Brushes.Red;
                    return $"- {inv.Total:F2} грн";
                }
            }
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
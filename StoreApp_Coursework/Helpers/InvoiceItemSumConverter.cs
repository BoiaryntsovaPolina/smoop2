using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace StoreApp.Helpers
{
    // Автоматично рахує суму рядка в таблиці (Ціна * Кількість)
    public class InvoiceItemSumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "0.00";
            // Якщо передали список — рахуємо загальну суму
            if (value is IEnumerable e && !(value is string))
            {
                decimal total = 0;
                foreach (var it in e) total += GetSum(it);
                return total.ToString("F2", CultureInfo.InvariantCulture);
            }
            // Якщо один елемент — рахуємо його суму
            return GetSum(value).ToString("F2", CultureInfo.InvariantCulture);
        }

        // Отримує значення властивостей Price та Quantity і перемножує їх
        private decimal GetSum(object item)
        {
            if (item == null) return 0;
            var t = item.GetType();
            var qty = GetDecimal(t, item, "Quantity", "Qty", "Count");
            var price = GetDecimal(t, item, "Price", "UnitPrice", "Cost", "Amount");
            return qty * price;
        }

        // Допоміжний метод для пошуку числових значень у властивостях об'єкта
        private decimal GetDecimal(Type t, object item, params string[] names)
        {
            foreach (var n in names)
            {
                var pi = t.GetProperty(n, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (pi != null)
                {
                    var v = pi.GetValue(item);
                    if (v == null) continue;
                    if (decimal.TryParse(v.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var p)) return p;
                }
            }
            return 0;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
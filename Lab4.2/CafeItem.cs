using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Lab4._2_chernovic
{
    public class CafeItem
    {
        public string Name { get; set; }
        public double Price { get; set; }
        // Додано поля для зв'язку з UI елементами
        public CheckBox CheckBox { get; set; }
        public TextBox QuantityTextBox { get; set; }
        // Розрахунок загальної ціни, захищений від null
        public double TotalPrice => Price * (int.TryParse(QuantityTextBox?.Text, out int qty) && qty >= 0 ? qty : 0);
    }
}

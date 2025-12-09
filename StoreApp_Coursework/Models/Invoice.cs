using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreApp.Models
{
    // Тип операції
    public enum InvoiceType { Receipt, Issue }

    public class Invoice
    {
        public string Number { get; set; } = "0001";
        public DateTime Date { get; set; } = DateTime.Now;
        public string Customer { get; set; } = "";
        public InvoiceType Type { get; set; } = InvoiceType.Receipt;

        // Список позицій у накладній
        public List<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();

        // Автоматичний розрахунок загальної суми
        public decimal Total => Items.Sum(x => x.Price * x.Quantity);
    }
}
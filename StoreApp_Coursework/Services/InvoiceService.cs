using System.Collections.Generic;
using System.Linq;
using StoreApp.Models;

namespace StoreApp.Services
{
    public static class InvoiceService
    {
        private static readonly JsonDataStore<Invoice> _store = new JsonDataStore<Invoice>("invoices.json");
        private static List<Invoice> _invoices;

        static InvoiceService()
        {
            _invoices = _store.GetAll().ToList();
        }

        public static IEnumerable<Invoice> GetAll() => _invoices.ToList();

        private static void Save() => _store.SaveAll(_invoices);

        public static void Add(Invoice inv)
        {
            _invoices.Add(inv);
            Save();

            // Тільки в бізнес-режимі накладні впливають на склад
            if (SettingsService.Settings.Mode == AppMode.Business)
            {
                foreach (var item in inv.Items)
                {
                    // Receipt (Прихід) = плюс, Issue (Видаток) = мінус
                    int change = (inv.Type == InvoiceType.Receipt) ? item.Quantity : -item.Quantity;
                    ProductService.UpdateStock(item.Name, change);
                }
            }
        }

        public static void Update(int idx, Invoice inv)
        {
            if (idx >= 0 && idx < _invoices.Count)
            {
                _invoices[idx] = inv;
                Save();
            }
        }

        public static void Remove(int idx)
        {
            if (idx >= 0 && idx < _invoices.Count)
            {
                _invoices.RemoveAt(idx);
                Save();
            }
        }
    }
}
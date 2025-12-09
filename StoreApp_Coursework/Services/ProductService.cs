using System;
using System.Collections.Generic;
using System.Linq;
using StoreApp.Models;

namespace StoreApp.Services
{
    // Сервіс для роботи з товарами (Склад)
    public static class ProductService
    {
        // Подія, щоб оновити таблицю при змінах
        public static event Action? ProductsChanged;

        // Вибираємо файл (дім або бізнес)
        private static JsonDataStore<Product> StoreForMode()
        {
            var filename = SettingsService.Settings.Mode == AppMode.Business
                ? "products_business.json"
                : "products_home.json";
            return new JsonDataStore<Product>(filename);
        }

        // Отримати всі товари
        public static IEnumerable<Product> GetAll() => StoreForMode().GetAll();

        // Додати товар
        public static void Add(Product p)
        {
            var list = GetAll().ToList();
            list.Add(p);
            StoreForMode().SaveAll(list);
            ProductsChanged?.Invoke(); // Сповіщаємо про зміни
        }

        // Оновити товар
        public static void Update(int idx, Product p)
        {
            var list = GetAll().ToList();
            if (idx >= 0 && idx < list.Count)
            {
                list[idx] = p;
                StoreForMode().SaveAll(list);
                ProductsChanged?.Invoke();
            }
        }

        // Видалити за індексом
        public static void Remove(int idx)
        {
            var list = GetAll().ToList();
            if (idx >= 0 && idx < list.Count)
            {
                list.RemoveAt(idx);
                StoreForMode().SaveAll(list);
                ProductsChanged?.Invoke();
            }
        }

        // Видалити конкретний об'єкт
        public static void Remove(Product p)
        {
            var list = GetAll().ToList();
            // Шукаємо точний збіг по всіх полях
            var itemToDelete = list.FirstOrDefault(x =>
                x.Name == p.Name &&
                x.Category == p.Category &&
                x.Price == p.Price &&
                x.Quantity == p.Quantity &&
                x.Location == p.Location);

            if (itemToDelete != null)
            {
                list.Remove(itemToDelete);
                StoreForMode().SaveAll(list);
                ProductsChanged?.Invoke();
            }
        }

        // Змінити кількість (для накладних)
        public static void UpdateStock(string productName, int changeQuantity)
        {
            var list = GetAll().ToList();
            var product = list.FirstOrDefault(x => x.Name.Equals(productName, StringComparison.OrdinalIgnoreCase));

            if (product != null)
            {
                product.Quantity += changeQuantity;
                if (product.Quantity < 0) product.Quantity = 0; // Не допускаємо мінуса
                StoreForMode().SaveAll(list);
                ProductsChanged?.Invoke();
            }
        }

        // Перевірити залишок на складі
        public static int GetStockQuantity(string productName)
        {
            var product = GetAll().FirstOrDefault(x => x.Name.Equals(productName, StringComparison.OrdinalIgnoreCase));
            return product?.Quantity ?? 0;
        }
    }
}
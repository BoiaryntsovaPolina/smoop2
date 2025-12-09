using System;
using System.Linq;
using System.Windows.Controls;
using StoreApp.Services;
using StoreApp.ViewModels;

namespace StoreApp.Views
{
    public partial class ProductsView : UserControl
    {
        public ProductsView()
        {
            InitializeComponent();

            // Керування видимістю стовпчика "Місце"
            if (SettingsService.Settings.Mode == AppMode.Business)
            {
                // У бізнес-режимі ховаємо колонку (ширина 0)
                ColLocation.Width = 0;
            }
            else
            {
                // У Home режимі - показуємо
                ColLocation.Width = 150;
            }
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataContext is ProductsViewModel vm)
            {
                var q = (sender as TextBox)?.Text?.Trim() ?? "";

                // Якщо пошук пустий — показуємо всі товари (скидаємо фільтр)
                if (string.IsNullOrEmpty(q))
                {
                    vm.Refresh();
                    return;
                }

                // Фільтруємо список товарів
                // Шукаємо збіг у назві, категорії або місці зберігання
                var filtered = StoreApp.Services.ProductService.GetAll()
                    .Where(p =>
                        (p.Name != null && p.Name.Contains(q, StringComparison.OrdinalIgnoreCase)) ||
                        (p.Category.ToString().Contains(q, StringComparison.OrdinalIgnoreCase)) ||
                        (p.Location != null && p.Location.Contains(q, StringComparison.OrdinalIgnoreCase)))
                    .ToList();

                // Оновлюємо таблицю відфільтрованими даними
                vm.Products.Clear();
                foreach (var p in filtered) vm.Products.Add(p);
            }
        }
    }
}
using StoreApp.Models;
using StoreApp.Services;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StoreApp.Views
{
    public partial class ProductEditWindow : Window
    {
        public Product ProductResult { get; private set; }

        public ProductEditWindow(Product p)
        {
            InitializeComponent();
            ProductResult = p;

            // Заповнення списків
            CbCategory.ItemsSource = Enum.GetValues(typeof(ProductCategory));
            CbUnit.ItemsSource = Enum.GetValues(typeof(ProductUnit));
            CbQuantity.ItemsSource = Enumerable.Range(0, 101).ToList();

            // Заповнення полів
            TxtName.Text = p.Name;
            CbCategory.SelectedItem = p.Category;
            CbUnit.SelectedItem = p.Unit;
            TxtLocation.Text = p.Location;
            TxtPrice.Text = p.Price.ToString("F2");

            if (p.Quantity == 0 && string.IsNullOrEmpty(p.Name)) CbQuantity.Text = "1";
            else CbQuantity.Text = p.Quantity.ToString();

            // Адаптація під режим
            if (SettingsService.Settings.Mode == AppMode.Business)
            {
                Title = "Товар (Склад)";
                // Приховуємо рядок "Місце"
                RowLocation.Height = new GridLength(0);

                // Приховуємо елементи
                LblLocation.Visibility = Visibility.Collapsed; // Це тепер Border
                TxtLocation.Visibility = Visibility.Collapsed;
                ErrLocation.Visibility = Visibility.Collapsed;
            }
            else
            {
                Title = "Річ (Дім)";
            }
        }

        private void TxtNumbersOnly_PreviewTextInput(object sender, TextCompositionEventArgs e) => e.Handled = !Regex.IsMatch(e.Text, @"^[0-9]+$");

        private void TxtPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb == null) return;
            string newText = tb.Text.Insert(tb.SelectionStart, e.Text);
            e.Handled = !Regex.IsMatch(newText, @"^\d+([.,]\d*)?$");
        }

        // Перевірка тексту: не пусте, мін. 2 символи, велика літера
        private bool ValidateTextFormat(string text, out string error)
        {
            if (string.IsNullOrWhiteSpace(text)) { error = "Поле не може бути порожнім."; return false; }
            if (text.Length < 2) { error = "Мінімум 2 символи."; return false; }
            if (!char.IsUpper(text[0])) { error = "Почніть з великої літери."; return false; }
            error = ""; return true;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            ErrName.Text = ErrQuantity.Text = ErrPrice.Text = ErrLocation.Text = "";
            bool isValid = true;

            // 1. Назва
            string nameInput = TxtName.Text.Trim();
            if (!ValidateTextFormat(nameInput, out string nameError))
            {
                ErrName.Text = nameError; isValid = false;
            }

            // 2. Кількість
            if (!int.TryParse(CbQuantity.Text, out int qty) || qty < 0)
            {
                ErrQuantity.Text = "Вкажіть коректну кількість (>= 0)."; isValid = false;
            }

            // 3. Ціна
            string priceStr = TxtPrice.Text.Replace(',', '.');
            if (!decimal.TryParse(priceStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal price))
            {
                ErrPrice.Text = "Невірний формат."; isValid = false;
            }
            else
            {
                if (SettingsService.Settings.Mode == AppMode.Business && price <= 0)
                { ErrPrice.Text = "Має бути > 0."; isValid = false; }
                else if (price < 0)
                { ErrPrice.Text = "Не може бути < 0."; isValid = false; }
            }

            // 4. Місце (Home)
            if (SettingsService.Settings.Mode == AppMode.Home)
            {
                string locInput = TxtLocation.Text.Trim();
                if (!ValidateTextFormat(locInput, out string locError))
                {
                    ErrLocation.Text = locError; isValid = false;
                }
            }

            if (!isValid) return;

            // Збереження
            ProductResult.Name = nameInput;
            ProductResult.Category = (ProductCategory)CbCategory.SelectedItem;
            ProductResult.Unit = (ProductUnit)CbUnit.SelectedItem;
            ProductResult.Quantity = qty;
            ProductResult.Price = price;
            ProductResult.Location = TxtLocation.Text.Trim();

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
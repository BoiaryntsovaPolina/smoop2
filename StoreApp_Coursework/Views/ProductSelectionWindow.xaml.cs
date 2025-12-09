using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using StoreApp.Models;
using StoreApp.Services;

namespace StoreApp.Views
{
    public partial class ProductSelectionWindow : Window
    {
        public Product SelectedProduct { get; private set; }
        public int SelectedQuantity { get; private set; } = 1;

        private readonly InvoiceType _type;

        public ProductSelectionWindow(InvoiceType type)
        {
            InitializeComponent();
            _type = type;
            App.ApplyTheme(this); // Застосування теми
            LbProducts.ItemsSource = ProductService.GetAll();
        }

        private void LbProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LbProducts.SelectedItem is Product p)
            {
                TxtAvailable.Text = p.Quantity.ToString();

                // <--- ОНОВЛЕННЯ ОДИНИЦЬ ВИМІРУ
                string unitText = p.Unit.ToString();
                TxtUnitLabel.Text = unitText;
                TxtUnitLabelInput.Text = unitText;
            }
            else
            {
                TxtAvailable.Text = "-";
                TxtUnitLabel.Text = "";
                TxtUnitLabelInput.Text = "";
            }
        }

        private void TxtNumbersOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[0-9]+$");
        }

        private void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (LbProducts.SelectedItem is Product p)
            {
                if (int.TryParse(TxtQuantityInput.Text, out int qty) && qty > 0)
                {
                    // Перевірка залишків для Видаткової накладної
                    if (_type == InvoiceType.Issue && qty > p.Quantity)
                    {
                        // <--- ВИВІД ПОВІДОМЛЕННЯ З ОДИНИЦЯМИ
                        MessageBox.Show($"Недостатньо товару на складі!\nДоступно: {p.Quantity} {p.Unit}\nВи хочете: {qty} {p.Unit}",
                                        "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    SelectedProduct = p;
                    SelectedQuantity = qty;
                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("Введіть коректну кількість.");
                }
            }
            else
            {
                MessageBox.Show("Оберіть товар зі списку.");
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) { DialogResult = false; Close(); }
    }
}
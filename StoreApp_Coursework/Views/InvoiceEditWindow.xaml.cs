using System.Windows;
using System.Windows.Controls;
using StoreApp.Models;
using StoreApp.ViewModels;

namespace StoreApp.Views
{
    public partial class InvoiceEditWindow : Window
    {
        private readonly InvoiceEditViewModel _vm;

        public InvoiceEditWindow(Invoice invoice, UserRole role, bool isReadOnly)
        {
            InitializeComponent();

            // Створюємо ViewModel
            _vm = new InvoiceEditViewModel(invoice, role, isReadOnly);

            // Підписуємося на події від ViewModel
            _vm.RequestClose += (result) =>
            {
                DialogResult = result;
                Close();
            };

            _vm.RequestProductSelection += OpenProductSelection;

            // Встановлюємо DataContext для Binding у XAML
            DataContext = _vm;

            // Налаштування початкової дати для календаря
            HiddenDatePicker.SelectedDate = invoice.Date;
        }

        // Відкриття дочірнього вікна вибору товару
        private void OpenProductSelection()
        {
            var currentType = _vm.SelectedType.Value;
            var selector = new ProductSelectionWindow(currentType) { Owner = this };

            if (selector.ShowDialog() == true && selector.SelectedProduct != null)
            {
                _vm.AddItemFromSelection(selector.SelectedProduct, selector.SelectedQuantity);
            }
        }

        // Клік по кнопці календаря відкриває DatePicker
        private void BtnCalendar_Click(object sender, RoutedEventArgs e)
        {
            if (!_vm.IsReadOnly) HiddenDatePicker.IsDropDownOpen = true;
        }

        // --- ВИПРАВЛЕНИЙ МЕТОД ---
        private void HiddenDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HiddenDatePicker.SelectedDate.HasValue)
            {
                // 1. Оновлюємо дату в об'єкті накладної
                _vm.Invoice.Date = HiddenDatePicker.SelectedDate.Value;

                // 2. Оскільки Invoice - це простий клас (не Observable), 
                // інтерфейс може не дізнатися про зміну миттєво.
                // Тому ми просто оновлюємо текст у полі вручну.
                TxtDateDisplay.Text = _vm.Invoice.Date.ToShortDateString();
            }
        }
    }
}
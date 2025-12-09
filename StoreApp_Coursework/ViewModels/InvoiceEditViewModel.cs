using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using StoreApp.Helpers;
using StoreApp.Models;
using StoreApp.Services;

namespace StoreApp.ViewModels
{
    public class InvoiceTypeItem
    {
        public string Name { get; set; } = "";
        public InvoiceType Value { get; set; }
        public override string ToString() => Name;
    }

    // Логіка вікна створення/редагування накладної
    public class InvoiceEditViewModel : ObservableObject
    {
        public Invoice Invoice { get; }
        public ObservableCollection<InvoiceItem> Items { get; } // Список товарів у накладній
        public List<InvoiceTypeItem> InvoiceTypes { get; } // Список типів для ComboBox

        private InvoiceTypeItem _selectedType;
        public InvoiceTypeItem SelectedType
        {
            get => _selectedType;
            set
            {
                // Заборона зміни на "Видаток", якщо немає товару
                if (value.Value == InvoiceType.Issue && !CheckStockForIssue())
                {
                    Application.Current.Dispatcher.Invoke(() => Raise(nameof(SelectedType)));
                    return;
                }
                Set(ref _selectedType, value);
                Invoice.Type = value.Value;
                UpdateLabel();
            }
        }

        private string _counterpartyLabel = "Клієнт:";
        public string CounterpartyLabel { get => _counterpartyLabel; set => Set(ref _counterpartyLabel, value); }

        public bool IsReadOnly { get; }
        // Властивість для відображення кнопок (якщо не ReadOnly - значить редагуємо)
        public bool IsEditing => !IsReadOnly;
        public bool CanChangeType { get; }

        public RelayCommand SaveCommand { get; }
        public RelayCommand AddItemCommand { get; }
        public RelayCommand CancelCommand { get; }

        // Події для керування вікном (закрити, відкрити вибір)
        public event Action<bool?>? RequestClose;
        public event Action? RequestProductSelection;

        public InvoiceEditViewModel(Invoice invoice, UserRole role, bool isReadOnly)
        {
            Invoice = invoice;
            IsReadOnly = isReadOnly;
            Items = new ObservableCollection<InvoiceItem>(invoice.Items);

            InvoiceTypes = new List<InvoiceTypeItem>
            {
                new InvoiceTypeItem { Name = "Прибуткова (Закупівля)", Value = InvoiceType.Receipt },
                new InvoiceTypeItem { Name = "Видаткова (Продаж)", Value = InvoiceType.Issue }
            };

            _selectedType = InvoiceTypes.FirstOrDefault(x => x.Value == invoice.Type) ?? InvoiceTypes[0];
            UpdateLabel();

            // Вантажник бачить тільки "Видаткова"
            if (role == UserRole.Loader)
            {
                _selectedType = InvoiceTypes.FirstOrDefault(x => x.Value == InvoiceType.Issue)!;
                CanChangeType = false;
            }
            else
            {
                CanChangeType = !IsReadOnly;
            }

            SaveCommand = new RelayCommand(_ => Save(), _ => IsEditing);
            AddItemCommand = new RelayCommand(_ => RequestProductSelection?.Invoke(), _ => IsEditing);
            CancelCommand = new RelayCommand(_ => RequestClose?.Invoke(false));
        }

        private void UpdateLabel()
        {
            CounterpartyLabel = (SelectedType.Value == InvoiceType.Receipt) ? "Постачальник:" : "Клієнт:";
        }

        // Додавання товару з вікна вибору
        public void AddItemFromSelection(Product p, int qty)
        {
            // Перевірка наявності для продажу
            if (SelectedType.Value == InvoiceType.Issue)
            {
                var existingItem = Items.FirstOrDefault(x => x.Name == p.Name);
                int currentQtyInInvoice = existingItem?.Quantity ?? 0;

                if (currentQtyInInvoice + qty > p.Quantity)
                {
                    MessageBox.Show($"Недостатньо товару на складі!\nДоступно: {p.Quantity}", "Помилка");
                    return;
                }
            }

            var item = Items.FirstOrDefault(x => x.Name == p.Name);
            if (item != null)
            {
                item.Quantity += qty;
                // Оновлюємо список для перерахунку суми
                int idx = Items.IndexOf(item);
                Items.RemoveAt(idx);
                Items.Insert(idx, item);
            }
            else
            {
                Items.Add(new InvoiceItem { Name = p.Name, Quantity = qty, Price = p.Price, Unit = p.Unit });
            }
            Raise(nameof(Invoice));
        }

        private bool CheckStockForIssue()
        {
            foreach (var item in Items)
            {
                if (item.Quantity > ProductService.GetStockQuantity(item.Name))
                {
                    MessageBox.Show($"Нестача товару '{item.Name}' на складі!", "Помилка");
                    return false;
                }
            }
            return true;
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Invoice.Customer)) { MessageBox.Show("Вкажіть клієнта/постачальника."); return; }
            if (Items.Count == 0) { MessageBox.Show("Додайте товари."); return; }
            if (SelectedType.Value == InvoiceType.Issue && !CheckStockForIssue()) return;

            Invoice.Customer = Invoice.Customer;
            Invoice.Type = SelectedType.Value;
            Invoice.Items = Items.ToList();

            RequestClose?.Invoke(true); // Закриваємо з успіхом
        }
    }
}
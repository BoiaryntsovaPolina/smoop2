using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using StoreApp.Helpers;
using StoreApp.Models;
using StoreApp.Services;

namespace StoreApp.ViewModels
{
    // Логіка для вкладки "Накладні"
    public class InvoicesViewModel : ObservableObject
    {
        public ObservableCollection<Invoice> Invoices { get; } = new ObservableCollection<Invoice>();

        private Invoice? _selected;
        public Invoice? Selected
        {
            get => _selected;
            set
            {
                Set(ref _selected, value);
                // Оновлюємо активність кнопок
                EditCommand.RaiseCanExecuteChanged();
                DeleteCommand.RaiseCanExecuteChanged();
                ViewCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand CreateCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand ViewCommand { get; }

        private bool _canCreate = false;
        private bool _isAdmin = false;
        private bool _canView = false;
        private UserRole _currentRole = UserRole.Loader;

        public InvoicesViewModel()
        {
            // Налаштування команд (що робити, коли доступно)
            CreateCommand = new RelayCommand(_ => Create(), _ => _canCreate);
            EditCommand = new RelayCommand(_ => Edit(), _ => _isAdmin && Selected != null);
            DeleteCommand = new RelayCommand(_ => Delete(), _ => _isAdmin && Selected != null);
            ViewCommand = new RelayCommand(_ => ViewDetails(), _ => _canView && Selected != null);

            Load();
        }

        // Оновлення прав доступу
        public void UpdatePermissions(User? user)
        {
            if (SettingsService.Settings.Mode == AppMode.Home)
            {
                _canCreate = false; _isAdmin = false; _canView = false;
            }
            else if (user != null)
            {
                _currentRole = user.Role;
                _canCreate = true;
                _isAdmin = (user.Role == UserRole.Admin);
                _canView = true;
            }

            CreateCommand.RaiseCanExecuteChanged();
            EditCommand.RaiseCanExecuteChanged();
            DeleteCommand.RaiseCanExecuteChanged();
            ViewCommand.RaiseCanExecuteChanged();
        }

        public void Load()
        {
            Invoices.Clear();
            foreach (var inv in InvoiceService.GetAll()) Invoices.Add(inv);
        }

        private void Create()
        {
            var nextNum = (InvoiceService.GetAll().Count() + 1).ToString("D4");
            var inv = new Invoice { Number = nextNum, Date = System.DateTime.Now, Customer = "" };

            var dlg = new Views.InvoiceEditWindow(inv, _currentRole, false) { Owner = Application.Current.MainWindow };
            if (dlg.ShowDialog() == true)
            {
                InvoiceService.Add(inv);
                Load();
            }
        }

        private void ViewDetails()
        {
            if (Selected == null) return;
            // Відкриваємо в режимі читання (true)
            var dlg = new Views.InvoiceEditWindow(Selected, _currentRole, true) { Owner = Application.Current.MainWindow };
            dlg.ShowDialog();
        }

        private void Edit()
        {
            if (Selected == null || !_isAdmin) return;
            var idx = Invoices.IndexOf(Selected);
            // Копіюємо накладну перед редагуванням
            var copy = new Invoice
            {
                Number = Selected.Number,
                Date = Selected.Date,
                Customer = Selected.Customer,
                Type = Selected.Type,
                Items = Selected.Items.Select(i => new InvoiceItem { Name = i.Name, Quantity = i.Quantity, Price = i.Price, Unit = i.Unit }).ToList()
            };
            var dlg = new Views.InvoiceEditWindow(copy, _currentRole, false) { Owner = Application.Current.MainWindow };
            if (dlg.ShowDialog() == true)
            {
                InvoiceService.Update(idx, copy);
                Load();
            }
        }

        private void Delete()
        {
            if (Selected == null || !_isAdmin) return;
            if (MessageBox.Show($"Видалити накладну #{Selected.Number}?", "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var idx = Invoices.IndexOf(Selected);
                InvoiceService.Remove(idx);
                Load();
            }
        }
    }
}
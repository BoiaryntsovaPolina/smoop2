using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using StoreApp.Helpers;
using StoreApp.Models;
using StoreApp.Services;

namespace StoreApp.ViewModels
{
    // Логіка для вкладки "Товари"
    public class ProductsViewModel : ObservableObject
    {
        // Колекція товарів для таблиці
        public ObservableCollection<Product> Products { get; } = new ObservableCollection<Product>();

        private Product? _selected;
        public Product? Selected
        {
            get => _selected;
            set
            {
                Set(ref _selected, value);
                EditCommand.RaiseCanExecuteChanged();
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand AddCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand DeleteCommand { get; }

        private bool _isFullAccess = false;

        public ProductsViewModel()
        {
            _isFullAccess = (SettingsService.Settings.Mode == AppMode.Home);

            AddCommand = new RelayCommand(_ => Add(), _ => _isFullAccess);
            EditCommand = new RelayCommand(_ => Edit(), _ => _isFullAccess && Selected != null);
            DeleteCommand = new RelayCommand(param => Delete(param), param => _isFullAccess);

            // Підписка на оновлення бази
            ProductService.ProductsChanged += OnProductsChanged;
            Load();
        }

        private void OnProductsChanged()
        {
            Application.Current.Dispatcher.Invoke(Load);
        }

        public void UpdatePermissions(User? user)
        {
            if (SettingsService.Settings.Mode == AppMode.Home) _isFullAccess = true;
            else if (user != null) _isFullAccess = (user.Role == UserRole.Admin);
            else _isFullAccess = false;

            AddCommand.RaiseCanExecuteChanged();
            EditCommand.RaiseCanExecuteChanged();
            DeleteCommand.RaiseCanExecuteChanged();
        }

        public void Load()
        {
            Products.Clear();
            foreach (var p in ProductService.GetAll()) Products.Add(p);
        }

        public void Refresh() => Load();

        private void Add()
        {
            var p = new Product();
            var dlg = new Views.ProductEditWindow(p) { Owner = Application.Current.MainWindow };
            if (dlg.ShowDialog() == true) ProductService.Add(p);
        }

        private void Edit()
        {
            if (Selected == null) return;
            var idx = Products.IndexOf(Selected);
            var copy = new Product
            {
                Name = Selected.Name,
                Category = Selected.Category,
                Unit = Selected.Unit,
                Quantity = Selected.Quantity,
                Price = Selected.Price,
                Location = Selected.Location
            };
            var dlg = new Views.ProductEditWindow(copy) { Owner = Application.Current.MainWindow };
            if (dlg.ShowDialog() == true) ProductService.Update(idx, copy);
        }

        private void Delete(object parameter)
        {
            var selectedItems = (parameter as IList)?.Cast<Product>().ToList();
            if (selectedItems == null || !selectedItems.Any())
            {
                if (Selected != null) selectedItems = new System.Collections.Generic.List<Product> { Selected };
                else { MessageBox.Show("Нічого не обрано."); return; }
            }

            if (MessageBox.Show($"Видалити {selectedItems.Count} товарів?", "Підтвердження", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                foreach (var p in selectedItems) ProductService.Remove(p);
            }
        }
    }
}
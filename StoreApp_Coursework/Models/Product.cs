using System.ComponentModel;

namespace StoreApp.Models
{
    public class Product : INotifyPropertyChanged
    {
        private string _name = "";

        // тип
        private ProductCategory _category = ProductCategory.Other;

        //Одиниця виміру
        private ProductUnit _unit = ProductUnit.Piece;

        private int _quantity = 0;
        private decimal _price = 0m;
        private string _location = "";

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        // Enum категорія
        public ProductCategory Category
        {
            get => _category;
            set { _category = value; OnPropertyChanged(nameof(Category)); }
        }

        // Enum тип
        public ProductUnit Unit
        {
            get => _unit;
            set { _unit = value; OnPropertyChanged(nameof(Unit)); }
        }

        public int Quantity
        {
            get => _quantity;
            set { _quantity = value; OnPropertyChanged(nameof(Quantity)); }
        }

        public decimal Price
        {
            get => _price;
            set { _price = value; OnPropertyChanged(nameof(Price)); }
        }

        public string Location
        {
            get => _location;
            set { _location = value; OnPropertyChanged(nameof(Location)); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
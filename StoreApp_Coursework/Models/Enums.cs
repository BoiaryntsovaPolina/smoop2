using System.ComponentModel;

namespace StoreApp.Models
{
    // Категорії товарів
    public enum ProductCategory
    {
        Other,
        Electronics,
        Furniture,
        Food,
        Clothing,
        Appliances,
        Stationery
    }

    // Одиниці виміру
    public enum ProductUnit
    {
        Piece,
        Kilogram,
        Liter,
        Meter,
        Pack
    }
}
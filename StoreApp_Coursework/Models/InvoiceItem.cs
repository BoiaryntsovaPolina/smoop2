namespace StoreApp.Models
{
    public class InvoiceItem
    {
        public string Name { get; set; } = "";

        // Одиниця виміру (кг, шт, л)
        public ProductUnit Unit { get; set; } = ProductUnit.Piece;

        public int Quantity { get; set; } = 1;
        public decimal Price { get; set; } = 0m;
    }
}

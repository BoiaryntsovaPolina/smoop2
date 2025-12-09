using System.Collections.Generic;
using StoreApp.Models;
using Bogus;

namespace StoreApp.Services
{
    public static class FakerService
    {
        public static List<Product> GenerateProducts(int count)
        {
            // Масив місць зберігання 
            var locations = new[]
            {
                "Garage", "Kitchen", "Bedroom", "Living Room",
                "Pantry", "Balcony", "Basement", "Hallway Closet",
                "Top Shelf", "Desk", "Kids Room", "Bathroom",
                "Warehouse A", "Warehouse B", "Office"
            };

            // явно вказуємо мову
            var faker = new Faker<Product>("en")

                .RuleFor(p => p.Name, f => f.Commerce.ProductName())

                // Категорія: випадково з Enum
                .RuleFor(p => p.Category, f => f.PickRandom<ProductCategory>())

                // Одиниця виміру: випадково з Enum
                .RuleFor(p => p.Unit, f => f.PickRandom<ProductUnit>())

                // Кількість
                .RuleFor(p => p.Quantity, f => f.Random.Int(1, 100))

                // Ціна
                .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price(10, 5000, 2)))

                // Місце: Беремо з нашого англійського масиву
                .RuleFor(p => p.Location, f => f.PickRandom(locations));

            return faker.Generate(count);
        }
    }
}
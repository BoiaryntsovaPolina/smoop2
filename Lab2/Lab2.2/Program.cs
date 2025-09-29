using Lab2._2;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("ЛАБОРАТОРНА РОБОТА: LINQ запити та методи розширення\n");
        Console.WriteLine("====================================================");

        Console.WriteLine("=== ЗАВДАННЯ: Робота з телефонами ===");
        Console.WriteLine("====================================================\n");

        Console.WriteLine("\n--- LINQ Аналітика ---");

        var phones = PhoneDataService.GetPhones();

        try
        {

            // 1. Загальна кількість телефонів
            Console.WriteLine($"1. Загальна кількість телефонів: {PhoneLinqService.CountAllPhones(phones)}");

            // 2. Кількість телефонів із ціною більше 100
            Console.WriteLine($"2. Телефонів із ціною > 100: {PhoneLinqService.CountPhonesAbovePrice(phones, 100)}");

            // 3. Кількість телефонів у діапазоні від 400 до 700
            Console.WriteLine($"3. Телефонів у діапазоні 400-700: {PhoneLinqService.CountPhonesInPriceRange(phones, 400, 700)}");

            // 4. Кількість телефонів Apple
            Console.WriteLine($"4. Телефонів Apple: {PhoneLinqService.CountPhonesByManufacturer(phones, "Apple")}");

            // 5. Телефон з мінімальною ціною
            var cheapest = PhoneLinqService.FindCheapestPhone(phones);
            Console.WriteLine($"5. Найдешевший телефон: {cheapest}");

            // 6. Телефон з максимальною ціною
            var mostExpensive = PhoneLinqService.FindMostExpensivePhone(phones);
            Console.WriteLine($"6. Найдорожчий телефон: {mostExpensive}");

            // 7. Найстаріший телефон
            var oldest = PhoneLinqService.FindOldestPhone(phones);
            Console.WriteLine($"7. Найстаріший телефон: {oldest}");

            // 8. Найновіший телефон
            var newest = PhoneLinqService.FindNewestPhone(phones);
            Console.WriteLine($"8. Найновіший телефон: {newest}");

            // 9. Середня ціна телефону
            Console.WriteLine($"9. Середня ціна телефону: {PhoneLinqService.CalculateAveragePrice(phones):C}");

            // 10. П'ять найдорожчих телефонів
            Console.WriteLine("\n10. П'ять найдорожчих телефонів:");
            var topExpensive = PhoneLinqService.GetTopExpensivePhones(phones, 5);
            foreach (var phone in topExpensive)
            {
                Console.WriteLine($"    {phone}");
            }

            // 11. П'ять найдешевших телефонів
            Console.WriteLine("\n11. П'ять найдешевших телефонів:");
            var topCheapest = PhoneLinqService.GetTopCheapestPhones(phones, 5);
            foreach (var phone in topCheapest)
            {
                Console.WriteLine($"    {phone}");
            }

            // 12. Три найстаріші телефони
            Console.WriteLine("\n12. Три найстаріші телефони:");
            var oldestPhones = PhoneLinqService.GetOldestPhones(phones, 3);
            foreach (var phone in oldestPhones)
            {
                Console.WriteLine($"    {phone}");
            }

            // 13. Три найновіші телефони
            Console.WriteLine("\n13. Три найновіші телефони:");
            var newestPhones = PhoneLinqService.GetNewestPhones(phones, 3);
            foreach (var phone in newestPhones)
            {
                Console.WriteLine($"    {phone}");
            }

            // 14. Статистика за виробниками
            Console.WriteLine("\n14. Статистика за виробниками:");
            var manufacturerStats = PhoneLinqService.GetPhoneStatsByManufacturer(phones);
            foreach (var s in manufacturerStats)
            {
                Console.WriteLine($"    {s.Manufacturer}: {s.Count}");
            }

            // 15. Статистика за моделями
            Console.WriteLine("\n15. Статистика за моделями:");
            var modelStats = PhoneLinqService.GetPhoneStatsByModel(phones);
            foreach (var s in modelStats)
            {
                Console.WriteLine($"    {s.Model}: {s.Count}");
            }

            // 16. Статистика за роками
            Console.WriteLine("\n16. Статистика за роками:");
            var yearStats = PhoneLinqService.GetPhoneStatsByYear(phones);
            foreach (var s in yearStats)
            {
                Console.WriteLine($"    {s.Year}: {s.Count}");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка під час виконання: {ex.Message}");
        }

        Console.WriteLine("\n====================================================");
        Console.WriteLine("Натисніть будь-яку клавішу для завершення...");
        Console.ReadKey();
    }
}
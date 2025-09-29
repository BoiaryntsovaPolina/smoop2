using Lab2._1;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("=== ЛАБОРАТОРНА РОБОТА: LINQ ЗАПИТИ ДЛЯ ФІРМ ===\n");

        List<Company> companies = CompanyFactory.CreateSeedCompanies();

        try
        {
            // Виконуємо всі запити згідно завдання
            TaskQueries.RunAllTaskQueries(companies);

            // Додаткові приклади з демонстрацією переваг record
            DemoMethods.RunAllDemoMethods(companies);

            // Демонстрація record features (опціонально, можна прибрати або викликати за прапорцем)
            CompanyFactory.DemonstrateRecordFeatures();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }

        Console.WriteLine("Натисніть будь-яку клавішу для виходу...");
        Console.ReadKey();
    }
}

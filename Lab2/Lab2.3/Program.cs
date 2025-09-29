using Lab2._3;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        try
        {
            // Демонстрація роботи з фіксованими даними
            RunWithSampleData();

            Console.WriteLine("\n" + new string('═', 60));
            Console.WriteLine("Бажаете подивитися на роботу з випадковими даними? (y/n)");
            var input = Console.ReadLine();

            if (input?.ToLower() == "y" || input?.ToLower() == "yes")
            {
                RunWithRandomData();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка виконання програми: {ex.Message}");
            Console.WriteLine($"Деталі: {ex.StackTrace}");
        }
        finally
        {
            Console.WriteLine("\nНатисніть будь-яку клавішу для завершення...");
            Console.ReadKey();
        }
    }

    private static void RunWithSampleData()
    {
        Console.WriteLine("ДЕМОНСТРАЦІЯ ЛАБОРАТОРНОЇ РОБОТИ З RECORDS");
        Console.WriteLine(new string('═', 60));

        // Створюємо підприємство з тестовими даними
        var company = DataGenerator.CreateSampleCompany();
        var service = new CompanyService(company);

        // Виконуємо всі завдання
        service.ExecuteAllTasks();
        service.DisplaySummary();
    }

    // Виконує програму з випадково згенерованими даними
    private static void RunWithRandomData()
    {
        Console.WriteLine("\nДЕМОНСТРАЦІЯ З ВИПАДКОВИМИ ДАНИМИ");
        Console.WriteLine(new string('═', 60));

        // Створюємо підприємство з випадковими даними
        var randomCompany = DataGenerator.CreateRandomCompany("ПрАТ 'Випадкові Інновації'");
        var randomService = new CompanyService(randomCompany);

        // Виконуємо всі завдання
        randomService.ExecuteAllTasks();
        randomService.DisplaySummary();
    }
}

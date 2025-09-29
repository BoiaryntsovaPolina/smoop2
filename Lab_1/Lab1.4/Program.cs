using Lab1._4;

internal class Program
{
    private static async Task Main(string[] args)
    {
        try
        {
            // Налаштовуємо кодування консолі для підтримки українських символів
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.InputEncoding = System.Text.Encoding.Unicode;

            // Встановлюємо заголовок консолі
            Console.Title = "Система словників - Лабораторна робота №2";

            Console.WriteLine("Ласкаво просимо до системи словників!");
            Console.WriteLine();

            // Створюємо та запускаємо систему меню
            MenuSystem menuSystem = new MenuSystem();
            await menuSystem.RunAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Критична помилка: {ex.Message}");
            Console.WriteLine("Натисніть Enter для виходу...");
            Console.ReadLine();
        }
    }
}
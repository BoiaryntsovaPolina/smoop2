using Lab1._3;

internal class Program
{
    private static void Main(string[] args)
    {
        try
        {
            MenuManager menuManager = new MenuManager();
            menuManager.Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Критична помилка: {ex.Message}");
            Console.WriteLine("Натисніть будь-яку клавішу для виходу...");
            Console.ReadKey();
        }
    }
}
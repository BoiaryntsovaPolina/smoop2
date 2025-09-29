using Lab1;
using Lab1.Services;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var demo = new DemonstrationService();
        demo.RunDemoAsync();

        Console.WriteLine("Демонстрація завершена. Натисніть будь-яку клавішу для виходу...");
        Console.ReadKey();
    }
}
using Lab1._2;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("=== Аналізатор текстових файлів ===");

        var analyzer = new TextAnalyzer();
        analyzer.StartAnalysis();

        Console.WriteLine("\nПрограма завершена. Натисніть будь-яку клавішу...");
        Console.ReadKey();
    }
}
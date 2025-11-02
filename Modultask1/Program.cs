using Modultask1;

internal class Program
{
    private static List<Worker> _workers = new List<Worker>();
    private static List<SalaryRecord> _salaries = new List<SalaryRecord>();
    private static readonly string _workersPath = Path.Combine("Data", "Workers.txt");
    private static readonly string _salariesPath = Path.Combine("Data", "Salaries.txt");

    private static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("Обробка даних працівників (консольна версія)\n");

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("Меню:");
            Console.WriteLine("1 - Згенерувати приклади");
            Console.WriteLine("2 - Завантажити з файлів");
            Console.WriteLine("3 - Виконати запити");
            Console.WriteLine("4 - Зберегти дані");
            Console.WriteLine("5 - Вийти");
            Console.Write("Ваш вибір: ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    GenerateSampleData();
                    break;
                case "2":
                    LoadData();
                    break;
                case "3":
                    RunQueries();
                    break;
                case "4":
                    SaveData();
                    break;
                case "5":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Невірний вибір!");
                    break;
            }

            Console.WriteLine();
        }
    }

    private static void GenerateSampleData()
    {
        var sample = JsonDataService.GenerateSampleData();
        _workers = sample.workers;
        _salaries = sample.salaries;

        try
        {
            JsonDataService.SaveToTxt(_workersPath, _workers);
            JsonDataService.SaveToTxt(_salariesPath, _salaries);
            Console.WriteLine("Згенеровано та збережено приклади.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Помилка збереження: " + ex.Message);
        }
    }

    private static void LoadData()
    {
        try
        {
            _workers = JsonDataService.LoadFromTxt<Worker>(_workersPath);
            _salaries = JsonDataService.LoadFromTxt<SalaryRecord>(_salariesPath);
            Console.WriteLine($"Завантажено: {_workers.Count} працівників, {_salaries.Count} зарплат.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Помилка завантаження: " + ex.Message);
        }
    }

    private static void SaveData()
    {
        try
        {
            JsonDataService.SaveToTxt(_workersPath, _workers);
            JsonDataService.SaveToTxt(_salariesPath, _salaries);
            Console.WriteLine("Дані збережено.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Помилка збереження: " + ex.Message);
        }
    }

    private static void RunQueries()
    {
        if (_workers.Count == 0 || _salaries.Count == 0)
        {
            Console.WriteLine("Немає даних для обробки.");
            return;
        }

        decimal threshold = ReadThreshold();

        Console.WriteLine("\n1) Працівники старші за 35 років:");
        var olderThan35 =
            from w in _workers
            where (w.GetAge() is int age && age > 35)
            select w.ToString();
        foreach (var line in olderThan35)
            Console.WriteLine(" - " + line);

        Console.WriteLine("\n2) Працівник з найбільшою зарплатою за 2-ге півріччя:");
        var maxSecond = _salaries.OrderByDescending(s => s.SecondHalf).FirstOrDefault();
        if (maxSecond != null)
        {
            var w = _workers.FirstOrDefault(x => x.Id == maxSecond.Id);
            if (w != null)
                Console.WriteLine($" - {w.Id} : {w.Specialty}");
        }

        Console.WriteLine("\n3) Працівники, чия річна зарплата нижча за середню:");
        var avgAnnual = _salaries.Average(s => s.Annual);
        var belowAvg =
            from w in _workers
            join s in _salaries on w.Id equals s.Id
            where s.Annual < avgAnnual
            select $"{w.FullName} — Освіта: {w.Education} — Річна: {s.Annual:F2}";
        foreach (var line in belowAvg)
            Console.WriteLine(" - " + line);

        Console.WriteLine($"\n4) Працівники з вищою освітою та зарплатою не менше порогу ({threshold}):");
        var qualified =
            from w in _workers
            join s in _salaries on w.Id equals s.Id
            where string.Equals(w.Education, "Вища", StringComparison.OrdinalIgnoreCase)
                  && s.Annual >= threshold
            select $"{w.FullName} — Річна: {s.Annual:F2}";

        foreach (var line in qualified)
            Console.WriteLine(" - " + line);
    }

    private static decimal ReadThreshold()
    {
        decimal threshold = 0;
        bool valid = false;
        while (!valid)
        {
            Console.Write("\nВведіть поріг річної зарплати (>=1): ");
            string? input = Console.ReadLine();
            valid = decimal.TryParse(input, out threshold) && threshold >= 1;
            if (!valid)
            {
                Console.WriteLine("Помилка: введіть додатнє число >= 1.");
            }
        }
        return threshold;
    }
}
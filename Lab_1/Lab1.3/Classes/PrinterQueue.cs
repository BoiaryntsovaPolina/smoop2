using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1._3.Classes
{
    internal class PrinterQueue
    {
        private PriorityQueue<PrintJob, (int priority, long seq)> printQueue;
        private List<PrintStatistics> statistics;
        private List<PrintJob> jobList;

        private long _sequence;
        private bool isPrinting;

        public PrinterQueue()
        {
            printQueue = new PriorityQueue<PrintJob, (int, long)>();
            statistics = new List<PrintStatistics>();
            jobList = new List<PrintJob>();
            _sequence = 0;
            isPrinting = false;
        }

        public void AddPrintJob(PrintJob job)
        {
            if (job == null) throw new ArgumentNullException(nameof(job));

            _sequence++;
            job.Sequence = _sequence;

            var key = (job.Priority, job.Sequence);

            printQueue.Enqueue(job, key);
            jobList.Add(job);

            Console.WriteLine($"Завдання додано до черги: {job}");
            Console.WriteLine($"Завдань у черзі: {GetQueueCount()}");
        }

        public void ProcessNextJob()
        {
            if (printQueue.Count == 0)
            {
                Console.WriteLine("Черга друку порожня!");
                return;
            }

            if (isPrinting)
            {
                Console.WriteLine("Принтер зараз зайнятий!");
                return;
            }

            isPrinting = true;

            // Беремо наступне завдання з найвищим пріоритетом
            PrintJob currentJob = printQueue.Dequeue();

            //видаляємо з jobList по Sequence, щоб не накопичувати записи
            int idx = jobList.FindIndex(j => j.Sequence == currentJob.Sequence);
            if (idx >= 0)
            {
                jobList.RemoveAt(idx);
            }
            else
            {
                // Для діагностики — якщо раптом не знайшлося
                Console.WriteLine("WARN: не знайдено job у jobList при ProcessNextJob.");
            }

            DateTime printTime = DateTime.Now;

            Console.WriteLine($"\nДрукується документ: {currentJob.DocumentName} ({currentJob.Pages} сторінок)");
            Console.WriteLine($"Користувач: {currentJob.User}, Пріоритет: {currentJob.Priority}");

            // Імітація друку по сторінках
            for (int i = 1; i <= currentJob.Pages; i++)
            {
                Console.WriteLine($"   Сторінка {i} надрукована...");
            }

            Console.WriteLine($"Документ '{currentJob.DocumentName}' надруковано повністю!\n");

            // Додаємо статистику
            statistics.Add(new PrintStatistics(currentJob, printTime));

            isPrinting = false;
        }

        // Відображає поточну чергу — використовує jobList для стабільного сортування.
        public void DisplayQueue()
        {
            Console.WriteLine("\n=== ПОТОЧНА ЧЕРГА ДРУКУ ===");

            if (GetQueueCount() == 0)
            {
                Console.WriteLine("Черга порожня");
                return;
            }

            Console.WriteLine($"Кількість завдань у черзі: {GetQueueCount()}");
            Console.WriteLine("Примітка: Завдання впорядковані за пріоритетом (1 - найвищий). Для однакового пріоритету зберігається порядок додавання (FIFO).");

            ShowQueueDetails();
        }

        private void ShowQueueDetails()
        {
            // Сортування вручну без LINQ
            List<PrintJob> snapshot = new List<PrintJob>(jobList);

            snapshot.Sort((a, b) =>
            {
                int cmp = a.Priority.CompareTo(b.Priority);
                if (cmp == 0)
                    cmp = a.Sequence.CompareTo(b.Sequence);
                return cmp;
            });

            Console.WriteLine("\nЗавдання в черзі (за пріоритетом, стабільно):");
            for (int i = 0; i < snapshot.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {snapshot[i]}");
            }
        }

        public void DisplayStatistics()
        {
            Console.WriteLine("\n=== СТАТИСТИКА ДРУКУ ===");

            if (statistics.Count == 0)
            {
                Console.WriteLine("Статистика порожня");
                return;
            }

            foreach (var stat in statistics)
            {
                Console.WriteLine(stat);
            }

            int totalPages = 0;
            foreach (var s in statistics)
            {
                totalPages += s.Pages;
            }

            Dictionary<string, int> userStats = new Dictionary<string, int>();
            foreach (var s in statistics)
            {
                if (!userStats.ContainsKey(s.User))
                    userStats[s.User] = 0;
                userStats[s.User]++;
            }

            Console.WriteLine($"\nЗагалом надруковано: {statistics.Count} документів, {totalPages} сторінок");
            Console.WriteLine("\nСтатистика по користувачах:");
            foreach (var user in userStats)
            {
                Console.WriteLine($"  {user.Key}: {user.Value} документів");
            }
        }

        public void SaveStatisticsToFile(string fileName = "print_statistics.txt")
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    writer.WriteLine("=== СТАТИСТИКА ДРУКУ ===");
                    writer.WriteLine($"Створено: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
                    writer.WriteLine();

                    if (statistics.Count == 0)
                    {
                        writer.WriteLine("Статистика порожня");
                    }
                    else
                    {
                        foreach (var stat in statistics)
                        {
                            writer.WriteLine(stat);
                        }

                        int totalPages = 0;
                        foreach (var s in statistics)
                        {
                            totalPages += s.Pages;
                        }

                        Dictionary<string, int> userStats = new Dictionary<string, int>();
                        foreach (var s in statistics)
                        {
                            if (!userStats.ContainsKey(s.User))
                                userStats[s.User] = 0;
                            userStats[s.User]++;
                        }

                        writer.WriteLine();
                        writer.WriteLine($"Загалом надруковано: {statistics.Count} документів, {totalPages} сторінок");
                        writer.WriteLine();
                        writer.WriteLine("Статистика по користувачах:");
                        foreach (var user in userStats)
                        {
                            writer.WriteLine($"  {user.Key}: {user.Value} документів");
                        }
                    }
                }

                Console.WriteLine($"Статистика збережена у файл: {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка збереження файлу: {ex.Message}");
            }
        }

        public int GetQueueCount()
        {
            return jobList.Count;
        }

        public void ClearQueue()
        {
            printQueue.Clear();
            jobList.Clear();
            Console.WriteLine("Черга очищена!");
        }
    }
}

using Lab1._3.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1._3
{
    internal class MenuManager
    {
        private PrinterQueue printerQueue;
        private DataGenerator dataGenerator;

        public MenuManager()
        {
            printerQueue = new PrinterQueue();
            dataGenerator = new DataGenerator();
        }

        public void Run()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== СИСТЕМА УПРАВЛІННЯ ЧЕРГОЮ ДРУКУ ===");

            bool running = true;

            while (running)
            {
                DisplayMainMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddNewPrintJob();
                        break;
                    case "2":
                        printerQueue.ProcessNextJob();
                        break;
                    case "3":
                        printerQueue.DisplayQueue();
                        break;
                    case "4":
                        printerQueue.DisplayStatistics();
                        break;
                    case "5":
                        printerQueue.SaveStatisticsToFile();
                        break;
                    case "6":
                        printerQueue.ClearQueue();
                        break;
                    case "7":
                        RunDemo();
                        break;
                    case "8":
                        RunAdvancedDemo();
                        break;
                    case "9":
                        GenerateRandomJobs();
                        break;
                    case "0":
                        running = false;
                        Console.WriteLine("До побачення!");
                        break;
                    default:
                        Console.WriteLine("Невірна опція! Спробуйте знову.");
                        break;
                }

                if (running)
                {
                    Console.WriteLine("\nНатисніть будь-яку клавішу для продовження...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }


        private void DisplayMainMenu()
        {
            Console.WriteLine("\n--- МЕНЮ ---");
            Console.WriteLine("1. Додати завдання до черги");
            Console.WriteLine("2. Обробити наступне завдання");
            Console.WriteLine("3. Показати чергу");
            Console.WriteLine("4. Показати статистику");
            Console.WriteLine("5. Зберегти статистику у файл");
            Console.WriteLine("6. Очистити чергу");
            Console.WriteLine("7. Демо-режим (базовий)");
            Console.WriteLine("8. Демо-режим (розширений)");
            Console.WriteLine("9. Згенерувати випадкові завдання");
            Console.WriteLine("0. Вихід");
            Console.Write("Виберіть опцію: ");
        }


        private void AddNewPrintJob()
        {
            try
            {
                Console.Write("Введіть ім'я користувача: ");
                string user = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(user))
                {
                    Console.WriteLine("Ім'я користувача не може бути порожнім!");
                    return;
                }

                Console.Write("Введіть назву документа: ");
                string document = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(document))
                {
                    Console.WriteLine("Назва документа не може бути порожньою!");
                    return;
                }

                Console.Write("Введіть пріоритет (1-високий, 5-низький): ");
                if (!int.TryParse(Console.ReadLine(), out int priority) || priority < 1 || priority > 5)
                {
                    Console.WriteLine("Пріоритет повинен бути цілим числом від 1 до 5!");
                    return;
                }

                Console.Write("Введіть кількість сторінок: ");
                if (!int.TryParse(Console.ReadLine(), out int pages) || pages <= 0)
                {
                    Console.WriteLine("Кількість сторінок повинна бути додатнім цілим числом!");
                    return;
                }

                PrintJob job = new PrintJob(user, document, priority, pages);
                printerQueue.AddPrintJob(job);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }

        // Базовий демо-режим
        private void RunDemo()
        {
            Console.WriteLine("\n=== БАЗОВИЙ ДЕМО-РЕЖИМ ===");

            // Додаємо тестові завдання
            printerQueue.AddPrintJob(new PrintJob("Іван Петренко", "Звіт.docx", 3, 5));
            printerQueue.AddPrintJob(new PrintJob("Марія Коваленко", "Презентація.pptx", 1, 10));
            printerQueue.AddPrintJob(new PrintJob("Олег Сидоренко", "Договір.pdf", 2, 3));
            printerQueue.AddPrintJob(new PrintJob("Анна Левченко", "Фото.jpg", 5, 1));
            printerQueue.AddPrintJob(new PrintJob("Петро Мельник", "Аналіз.xlsx", 2, 7));

            Console.WriteLine("Тестові завдання додано!");

            // Показуємо чергу
            printerQueue.DisplayQueue();

            Console.WriteLine("\nПочинаємо обробку завдань...");

            // Обробляємо кілька завдань
            int processedCount = 0;
            while (printerQueue.GetQueueCount() > 0)
            {
                Console.WriteLine($"\n--- Обробка завдання #{processedCount + 1} ---");
                printerQueue.ProcessNextJob();
                processedCount++;
            }

            // Показуємо статистику
            printerQueue.DisplayStatistics();

            Console.WriteLine("\nБазове демо завершено!");
        }

        // Розширений демо-режим з випадковими даними
        private void RunAdvancedDemo()
        {
            Console.WriteLine("\n=== РОЗШИРЕНИЙ ДЕМО-РЕЖИМ ===");

            // Генеруємо випадкові завдання
            var randomJobs = dataGenerator.GenerateRandomPrintJobs(10);

            Console.WriteLine("Додаємо випадкові завдання...");
            foreach (var job in randomJobs)
            {
                printerQueue.AddPrintJob(job);
            }

            // Показуємо чергу
            printerQueue.DisplayQueue();

            Console.WriteLine("\nПочинаємо автоматичну обробку...");

            // Обробляємо всі завдання
            int processedCount = 0;
            while (printerQueue.GetQueueCount() > 0)
            {
                Console.WriteLine($"\n--- Автоматична обробка #{processedCount + 1} ---");
                printerQueue.ProcessNextJob();
                processedCount++;
            }

            // Показуємо статистику
            printerQueue.DisplayStatistics();

            Console.WriteLine("\nРозширене демо завершено!");
        }


        private void GenerateRandomJobs()
        {
            Console.Write("Скільки випадкових завдань згенерувати? (1-20): ");
            if (!int.TryParse(Console.ReadLine(), out int count) || count < 1 || count > 20)
            {
                Console.WriteLine("Кількість повинна бути від 1 до 20!");
                return;
            }

            var randomJobs = dataGenerator.GenerateRandomPrintJobs(count);

            Console.WriteLine($"\nГенерація {count} випадкових завдань...");
            foreach (var job in randomJobs)
            {
                printerQueue.AddPrintJob(job);
            }

            Console.WriteLine("Випадкові завдання згенеровано та додано до черги!");
            printerQueue.DisplayQueue();
        }
    }
}

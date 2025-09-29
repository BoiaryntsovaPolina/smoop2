using Lab1._3.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1._3
{
    internal class DataGenerator
    {
        private Random random;
        private string[] userNames;
        private string[] documentTypes;
        private string[] documentNames;

        public DataGenerator()
        {
            random = new Random();
            InitializeTestData();
        }

        private void InitializeTestData()
        {
            userNames = new string[]
            {
                "Іван Петренко", "Марія Коваленко", "Олег Сидоренко",
                "Анна Левченко", "Петро Мельник", "Оксана Шевченко",
                "Андрій Кравченко", "Наталія Бондаренко", "Сергій Тимошенко",
                "Юлія Гриценко", "Володимир Савченко", "Тетяна Лисенко",
                "Дмитро Попович", "Ірина Павленко", "Олександр Ткаченко",
                "Катерина Морозова", "Віталій Романенко", "Світлана Федоренко"
            };

            documentTypes = new string[]
            {
                ".docx", ".pdf", ".xlsx", ".pptx", ".txt", ".jpg", ".png", ".doc"
            };

            documentNames = new string[]
            {
                "Звіт", "Презентація", "Договір", "Аналіз", "План", "Кошторис",
                "Інструкція", "Протокол", "Заява", "Довідка", "Акт", "Рахунок",
                "Фото", "Схема", "Таблиця", "Графік", "Діаграма", "Резюме",
                "Проект", "Пропозиція", "Лист", "Записка", "Висновок", "Огляд"
            };
        }


        public PrintJob GenerateRandomPrintJob()
        {
            string user = userNames[random.Next(userNames.Length)];
            string docName = documentNames[random.Next(documentNames.Length)];
            string docType = documentTypes[random.Next(documentTypes.Length)];
            string fullDocName = docName + docType;

            int priority = random.Next(1, 6); // 1-5
            int pages = GenerateRandomPageCount();

            return new PrintJob(user, fullDocName, priority, pages);
        }


        public List<PrintJob> GenerateRandomPrintJobs(int count)
        {
            if (count <= 0)
            {
                throw new ArgumentException("Кількість завдань повинна бути більше 0");
            }

            List<PrintJob> jobs = new List<PrintJob>();

            for (int i = 0; i < count; i++)
            {
                jobs.Add(GenerateRandomPrintJob());
            }

            return jobs;
        }

        private int GenerateRandomPageCount()
        {
            // Різна ймовірність для різної кількості сторінок
            int roll = random.Next(100);

            if (roll < 30) // 30% - малі документи (1-3 сторінки)
                return random.Next(1, 4);
            else if (roll < 60) // 30% - середні документи (4-10 сторінок)
                return random.Next(4, 11);
            else if (roll < 85) // 25% - великі документи (11-50 сторінок)
                return random.Next(11, 51);
            else // 15% - дуже великі документи (51-200 сторінок)
                return random.Next(51, 201);
        }


        public PrintJob GenerateJobWithPriority(int priority)
        {
            if (priority < 1 || priority > 5)
            {
                throw new ArgumentException("Пріоритет повинен бути від 1 до 5");
            }

            var job = GenerateRandomPrintJob();

            // Створюємо новий об'єкт з потрібним пріоритетом
            return new PrintJob(job.User, job.DocumentName, priority, job.Pages);
        }


        public List<PrintJob> GeneratePriorityTestJobs()
        {
            List<PrintJob> jobs = new List<PrintJob>();

            // Додаємо по 2 завдання кожного пріоритету
            for (int priority = 1; priority <= 5; priority++)
            {
                for (int i = 0; i < 2; i++)
                {
                    jobs.Add(GenerateJobWithPriority(priority));
                }
            }

            // Перемішуємо список для демонстрації сортування
            ShuffleList(jobs);

            return jobs;
        }

        private void ShuffleList<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int randomIndex = random.Next(i + 1);
                T temp = list[i];
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }
        }

 
        public List<PrintJob> GenerateJobsForUser(string userName, int count)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException("Ім'я користувача не може бути порожнім");
            }

            if (count <= 0)
            {
                throw new ArgumentException("Кількість завдань повинна бути більше 0");
            }

            List<PrintJob> jobs = new List<PrintJob>();

            for (int i = 0; i < count; i++)
            {
                string docName = documentNames[random.Next(documentNames.Length)];
                string docType = documentTypes[random.Next(documentTypes.Length)];
                string fullDocName = $"{docName}_{i + 1}{docType}";

                int priority = random.Next(1, 6);
                int pages = GenerateRandomPageCount();

                jobs.Add(new PrintJob(userName, fullDocName, priority, pages));
            }

            return jobs;
        }

        public void DisplayGeneratorInfo()
        {
            Console.WriteLine("=== ІНФОРМАЦІЯ ПРО ГЕНЕРАТОР ДАНИХ ===");
            Console.WriteLine($"Доступно імен користувачів: {userNames.Length}");
            Console.WriteLine($"Доступно типів документів: {documentTypes.Length}");
            Console.WriteLine($"Доступно назв документів: {documentNames.Length}");
            Console.WriteLine($"Можливих комбінацій: {userNames.Length * documentNames.Length * documentTypes.Length}");
        }
    }
}

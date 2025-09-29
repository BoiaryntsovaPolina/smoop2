using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    internal class WorkerDataGenerator
    {
        private static Random random = new Random();

        private static string[] FirstNames =
        {
            "Alexander", "Andrew", "William", "John", "Michael", "Peter", "Sergey", "Thomas",
            "Oliver", "David", "Max", "Arthur", "Victor", "George", "Roman", "Igor",
            "Anna", "Maria", "Elena", "Tanya", "Natalia", "Irene", "Svetlana", "Catherine",
            "Linda", "Helen", "Oksana", "Julia", "Valentina", "Larisa", "Vera"
        };

        private static string[] LastNames =
        {
            "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez",
            "Martinez", "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Taylor",
            "Moore", "Jackson", "Martin", "Lee", "Perez", "Thompson", "White", "Harris"
        };

        private static string[] Positions =
        {
            "Software Engineer", "System Analyst", "QA Tester", "UI/UX Designer", "Project Manager",
            "DevOps Engineer", "System Administrator", "HR Specialist", "Marketing Manager",
            "Product Manager", "Frontend Developer", "Backend Developer", "Business Analyst", "Technical Writer",
            "Team Lead", "Software Architect", "QA Engineer", "Fullstack Developer",
            "Mobile Developer", "Data Scientist"
        };

        public static string GenerateFullName()
        {
            string firstName = FirstNames[random.Next(FirstNames.Length)];
            string lastName = LastNames[random.Next(LastNames.Length)];

            return $"{firstName} {lastName}";
        }

        // Генерує випадкову посаду
        public static string GeneratePosition()
        {
            return Positions[random.Next(Positions.Length)];
        }

        // Генерує випадковий оклад від мінімальної до максимальної суми
        public static decimal GenerateSalary(decimal minSalary = 15000m, decimal maxSalary = 50000m)
        {
            // Генеруємо оклад з кроком 1000 грн
            int min = (int)(minSalary / 1000);
            int max = (int)(maxSalary / 1000);
            return random.Next(min, max + 1) * 1000m;
        }

        // Генерує випадкову дату найму за останні N років
        public static DateTime GenerateHireDate(int maxYearsAgo = 10)
        {
            DateTime minDate = DateTime.Now.AddYears(-maxYearsAgo);
            DateTime maxDate = DateTime.Now.AddMonths(-1); // Мінімум місяць тому

            int range = (maxDate - minDate).Days;
            return minDate.AddDays(random.Next(range + 1));
        }

        // Створює одного випадкового робітника
        public static Worker CreateRandomWorker()
        {
            return new Worker(
                GenerateFullName(),
                GeneratePosition(),
                GenerateSalary(),
                GenerateHireDate()
            );
        }

        // Створює список випадкових робітників
        public static List<Worker> CreateRandomWorkers(int count)
        {
            List<Worker> workers = new List<Worker>();

            for (int i = 0; i < count; i++)
            {
                workers.Add(CreateRandomWorker());
            }

            return workers;
        }

        // Створює попередньо визначений список робітників для демонстрації
        public static List<Worker> CreatePredefinedWorkers()
        {
            return new List<Worker>
            {
                new Worker("John Johnson", "Software Engineer", 25000m, new DateTime(2015, 03, 15)),
                new Worker("Peter Williams", "Project Manager", 30000m, new DateTime(2018, 07, 20)),
                new Worker("Michael Brown", "System Analyst", 28000m, new DateTime(2016, 11, 10)),
                new Worker("Anna Garcia", "UI/UX Designer", 22000m, new DateTime(2020, 01, 25)),
                new Worker("Oliver Miller", "QA Tester", 24000m, new DateTime(2017, 05, 08)),
                new Worker("Maria Davis", "HR Specialist", 26000m, new DateTime(2014, 09, 01)),
                new Worker("Andrew Rodriguez", "System Administrator", 27000m, new DateTime(2013, 04, 12))
            };
        }

        // Створює робітника зі стажем більше вказаного значення (для тестування)
        public static Worker CreateExperiencedWorker(int minExperienceYears)
        {
            DateTime hireDate = DateTime.Now.AddYears(-(minExperienceYears + random.Next(1, 5)));

            return new Worker(
                GenerateFullName(),
                GeneratePosition(),
                GenerateSalary(),
                hireDate
            );
        }

        // Створює змішаний список: частина з великим стажем, частина з малим
        public static List<Worker> CreateMixedExperienceWorkers(int totalCount, int experiencedCount, int minExperience = 5)
        {
            List<Worker> workers = new List<Worker>();

            // Додаємо досвідчених робітників
            for (int i = 0; i < experiencedCount && i < totalCount; i++)
            {
                workers.Add(CreateExperiencedWorker(minExperience));
            }

            // Додаємо молодших робітників
            for (int i = experiencedCount; i < totalCount; i++)
            {
                DateTime recentHireDate = DateTime.Now.AddYears(-random.Next(0, minExperience));
                workers.Add(new Worker(
                    GenerateFullName(),
                    GeneratePosition(),
                    GenerateSalary(),
                    recentHireDate
                ));
            }

            return workers;
        }
    }
}


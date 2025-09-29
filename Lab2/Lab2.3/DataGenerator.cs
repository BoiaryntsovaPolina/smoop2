using Lab2._3.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2._3
{
    internal class DataGenerator
    {
        private static readonly Random _random = new Random();

        // Масиви даних для генерації
        private static readonly string[] MaleNames = {
            "Олександр", "Володимир", "Петро", "Сергій", "Андрій", "Віктор",
            "Дмитро", "Микола", "Євген", "Ігор", "Олег", "Василь"
        };

        private static readonly string[] FemaleNames = {
            "Ірина", "Анна", "Марія", "Наталія", "Олена", "Юлія",
            "Тетяна", "Світлана", "Людмила", "Катерина", "Оксана", "Галина"
        };

        private static readonly string[] LastNames = {
            "Петренко", "Коваленко", "Сидоренко", "Мельник", "Іваненко",
            "Шевченко", "Бондаренко", "Ткаченко", "Морозенко", "Литвиненко",
            "Гриценко", "Кравченко", "Савченко", "Павленко", "Романенко",
            "Левченко", "Гончаренко", "Даниленко", "Захаренко", "Марченко"
        };

        private static readonly string[] Departments = {
            "Фінансовий відділ", "Відділ маркетингу", "HR відділ",
            "IT відділ", "Юридичний відділ", "Відділ продажів"
        };

        private static readonly string[] WorkerPositions = {
            "Слюсар", "Інженер", "Технолог", "Оператор", "Провідний інженер",
            "Секретар", "Старший інженер", "Контролер якості",
            "Системний адміністратор", "Бухгалтер", "Електрик", "Зварщик",
            "Водій", "Охоронець", "Прибиральник"
        };

        // Створює підприємство з тестовими даними
        public static Company CreateSampleCompany()
        {
            var company = new Company("ТОВ 'Інноваційні рішення'");

            // Додаємо фіксовані дані для демонстрації функціоналу
            AddFixedSampleEmployees(company);

            return company;
        }

        // Створює підприємство зі згенерованими випадковими даними
        public static Company CreateRandomCompany(string companyName = "Випадкове підприємство")
        {
            var company = new Company(companyName);

            // Генеруємо випадкових працівників
            GenerateRandomEmployees(company);

            return company;
        }

        // Додає фіксовані тестові дані (для демонстрації всіх завдань)
        private static void AddFixedSampleEmployees(Company company)
        {
            // Президент
            company.AddEmployee(new President(
                Id: 1,
                FirstName: "Олександр",
                LastName: "Петренко",
                BirthDate: new DateTime(1975, 3, 15),
                HireDate: new DateTime(2010, 1, 1),
                Salary: 100000,
                Education: EducationType.Higher,
                Direction: ProfessionalDirection.Administrative
            ));

            // Менеджери різного віку
            company.AddEmployee(new Manager(
                Id: 2,
                FirstName: "Ірина",
                LastName: "Коваленко",
                BirthDate: new DateTime(1985, 7, 22),
                HireDate: new DateTime(2015, 3, 1),
                Salary: 45000,
                Education: EducationType.Higher,
                Direction: ProfessionalDirection.Financial,
                Department: "Фінансовий відділ"
            ));

            company.AddEmployee(new Manager(
                Id: 3,
                FirstName: "Володимир",
                LastName: "Сидоренко",
                BirthDate: new DateTime(1990, 10, 8),
                HireDate: new DateTime(2018, 6, 15),
                Salary: 42000,
                Education: EducationType.Higher,
                Direction: ProfessionalDirection.Marketing,
                Department: "Відділ маркетингу"
            ));

            company.AddEmployee(new Manager(
                Id: 4,
                FirstName: "Анна",
                LastName: "Мельник",
                BirthDate: new DateTime(1988, 12, 3),
                HireDate: new DateTime(2016, 9, 1),
                Salary: 48000,
                Education: EducationType.Higher,
                Direction: ProfessionalDirection.Administrative,
                Department: "HR відділ"
            ));

            // Робітники з різним стажем, освітою та датами народження (включаючи жовтень)
            company.AddEmployee(new Worker(
                Id: 5,
                FirstName: "Петро",
                LastName: "Іваненко",
                BirthDate: new DateTime(1992, 10, 12),
                HireDate: new DateTime(2019, 2, 1),
                Salary: 25000,
                Education: EducationType.Vocational,
                Direction: ProfessionalDirection.Technical,
                Position: "Слюсар",
                HourlyRate: 150,
                HoursPerMonth: 160
            ));

            company.AddEmployee(new Worker(
                Id: 6,
                FirstName: "Володимир",
                LastName: "Шевченко",
                BirthDate: new DateTime(1995, 4, 18),
                HireDate: new DateTime(2020, 8, 1),
                Salary: 28000,
                Education: EducationType.Higher,
                Direction: ProfessionalDirection.Technical,
                Position: "Інженер"
            ));

            company.AddEmployee(new Worker(
                Id: 7,
                FirstName: "Марія",
                LastName: "Бондаренко",
                BirthDate: new DateTime(1987, 10, 25),
                HireDate: new DateTime(2012, 5, 1),
                Salary: 32000,
                Education: EducationType.Higher,
                Direction: ProfessionalDirection.Production,
                Position: "Технолог"
            ));

            // Додаємо ще кілька Володимирів різного віку
            company.AddEmployee(new Worker(
                Id: 8,
                FirstName: "Володимир",
                LastName: "Морозенко",
                BirthDate: new DateTime(1989, 10, 30),
                HireDate: new DateTime(2013, 4, 1),
                Salary: 35000,
                Education: EducationType.Higher,
                Direction: ProfessionalDirection.Technical,
                Position: "Провідний інженер"
            ));

            company.AddEmployee(new Worker(
                Id: 9,
                FirstName: "Володимир",
                LastName: "Гриценко",
                BirthDate: new DateTime(1993, 1, 20),
                HireDate: new DateTime(2011, 7, 1),
                Salary: 38000,
                Education: EducationType.Higher,
                Direction: ProfessionalDirection.Technical,
                Position: "Старший інженер"
            ));

            // Додаємо більше робітників для демонстрації топ-10 за стажем
            for (int i = 10; i <= 20; i++)
            {
                company.AddEmployee(GenerateRandomWorker(i));
            }
        }

        // Генерує випадкових працівників
        private static void GenerateRandomEmployees(Company company)
        {
            // Додаємо президента
            company.AddEmployee(GenerateRandomPresident(1));

            // Додаємо менеджерів (3-5)
            int managersCount = _random.Next(3, 6);
            for (int i = 2; i <= managersCount + 1; i++)
            {
                company.AddEmployee(GenerateRandomManager(i));
            }

            // Додаємо робітників (15-25)
            int workersCount = _random.Next(15, 26);
            int startId = managersCount + 2;
            for (int i = startId; i < startId + workersCount; i++)
            {
                company.AddEmployee(GenerateRandomWorker(i));
            }
        }

        // Генерує випадкового президента
        private static President GenerateRandomPresident(int id)
        {
            var isMale = _random.Next(2) == 0;
            var firstName = GetRandomName(isMale);
            var lastName = GetRandomLastName();
            var birthDate = GenerateRandomBirthDate(45, 65);
            var hireDate = GenerateRandomHireDate(birthDate, 5, 20);
            var salary = _random.Next(80000, 150000);

            return new President(
                Id: id,
                FirstName: firstName,
                LastName: lastName,
                BirthDate: birthDate,
                HireDate: hireDate,
                Salary: salary,
                Education: EducationType.Higher,
                Direction: GetRandomProfessionalDirection(),
                BonusPercentage: _random.Next(30, 70)
            );
        }

        // Генерує випадкового менеджера
        private static Manager GenerateRandomManager(int id)
        {
            var isMale = _random.Next(2) == 0;
            var firstName = GetRandomName(isMale);
            var lastName = GetRandomLastName();
            var birthDate = GenerateRandomBirthDate(28, 55);
            var hireDate = GenerateRandomHireDate(birthDate, 2, 15);
            var salary = _random.Next(35000, 60000);
            var department = GetRandomDepartment();

            return new Manager(
                Id: id,
                FirstName: firstName,
                LastName: lastName,
                BirthDate: birthDate,
                HireDate: hireDate,
                Salary: salary,
                Education: GetRandomEducation(0.8),
                Direction: GetRandomProfessionalDirection(),
                Department: department,
                BonusPercentage: _random.Next(10, 30)
            );
        }

        // Генерує випадкового робітника
        private static Worker GenerateRandomWorker(int id)
        {
            var isMale = _random.Next(2) == 0;
            var firstName = GetRandomName(isMale);
            var lastName = GetRandomLastName();
            var birthDate = GenerateRandomBirthDate(22, 60);
            var hireDate = GenerateRandomHireDate(birthDate, 1, 20);
            var salary = _random.Next(20000, 45000);
            var position = GetRandomWorkerPosition();

            return new Worker(
                Id: id,
                FirstName: firstName,
                LastName: lastName,
                BirthDate: birthDate,
                HireDate: hireDate,
                Salary: salary,
                Education: GetRandomEducation(0.4),
                Direction: GetRandomProfessionalDirection(),
                Position: position,
                HourlyRate: _random.Next(120, 200),
                HoursPerMonth: 160
            );
        }

        // Допоміжні методи для генерації випадкових даних
        private static string GetRandomName(bool isMale)
        {
            var names = isMale ? MaleNames : FemaleNames;
            return names[_random.Next(names.Length)];
        }

        private static string GetRandomLastName()
        {
            return LastNames[_random.Next(LastNames.Length)];
        }

        private static DateTime GenerateRandomBirthDate(int minAge, int maxAge)
        {
            var age = _random.Next(minAge, maxAge + 1);
            var year = DateTime.Now.Year - age;
            var month = _random.Next(1, 13);
            var day = _random.Next(1, DateTime.DaysInMonth(year, month) + 1);

            return new DateTime(year, month, day);
        }

        private static DateTime GenerateRandomHireDate(DateTime birthDate, int minWorkYears, int maxWorkYears)
        {
            var workStartAge = _random.Next(18, 25);
            var minHireDate = birthDate.AddYears(workStartAge);
            var maxHireDate = DateTime.Now.AddYears(-minWorkYears);

            if (minHireDate > maxHireDate)
                minHireDate = maxHireDate.AddYears(-1);

            var range = (maxHireDate - minHireDate).Days;
            if (range <= 0) range = 365;

            return minHireDate.AddDays(_random.Next(range));
        }

        private static EducationType GetRandomEducation(double higherEducationProbability = 0.5)
        {
            var values = Enum.GetValues<EducationType>();

            // Збільшуємо ймовірність вищої освіти
            if (_random.NextDouble() < higherEducationProbability)
                return EducationType.Higher;

            return values[_random.Next(values.Length - 1)]; // Виключаємо Higher з загального випадку
        }

        private static ProfessionalDirection GetRandomProfessionalDirection()
        {
            var values = Enum.GetValues<ProfessionalDirection>();
            return values[_random.Next(values.Length)];
        }

        private static string GetRandomDepartment()
        {
            return Departments[_random.Next(Departments.Length)];
        }

        private static string GetRandomWorkerPosition()
        {
            return WorkerPositions[_random.Next(WorkerPositions.Length)];
        }
    }
}

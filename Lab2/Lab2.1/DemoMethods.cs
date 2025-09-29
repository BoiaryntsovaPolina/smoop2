using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2._1
{
    internal class DemoMethods
    {
        // Додатковий приклад: Групування фірм за профілем бізнесу
        public static void GroupCompaniesByBusinessProfile(List<Company> companies)
        {
            Console.WriteLine("=== ГРУПУВАННЯ ФІРМ ЗА ПРОФІЛЕМ БІЗНЕСУ ===");

            // Синтаксис запитів LINQ
            var groupedCompanies = from c in companies
                                   group c by c.BusinessProfile;

            // Альтернатива через методи розширення
            // var groupedCompanies = companies.GroupBy(c => c.BusinessProfile);

            foreach (var group in groupedCompanies)
            {
                Console.WriteLine($"\nПрофіль: {group.Key} ({group.Count()} фірм)");
                foreach (var company in group)
                {
                    Console.WriteLine($"  - {company.Name} ({company.EmployeesCount} співробітників)");
                }
            }
            Console.WriteLine();
        }

        // Додатковий приклад: Статистика з використанням обчислюваних властивостей record
        public static void ShowStatistics(List<Company> companies)
        {
            Console.WriteLine("=== СТАТИСТИКА ===");

            // Використання агрегатних функцій
            int totalCompanies = companies.Count();
            int totalEmployees = companies.Sum(c => c.EmployeesCount);
            double averageEmployees = companies.Average(c => c.EmployeesCount);
            int maxEmployees = companies.Max(c => c.EmployeesCount);
            int minEmployees = companies.Min(c => c.EmployeesCount);

            Console.WriteLine($"Загальна кількість фірм: {totalCompanies}");
            Console.WriteLine($"Загальна кількість співробітників: {totalEmployees}");
            Console.WriteLine($"Середня кількість співробітників: {averageEmployees:F2}");
            Console.WriteLine($"Максимальна кількість співробітників: {maxEmployees}");
            Console.WriteLine($"Мінімальна кількість співробітників: {minEmployees}");

            // Використання обчислюваних властивостей record
            var largeCompaniesCount = companies.Count(c => c.IsLargeCompany);
            var averageAge = companies.Average(c => c.AgeInYears);

            Console.WriteLine($"Великих фірм (>200 співроб.): {largeCompaniesCount}");
            Console.WriteLine($"Середній вік фірм: {averageAge:F1} років");
            Console.WriteLine();
        }

        // Демонстрація переваг record: створення статистики за допомогою record
        public static void ShowAdvancedStatistics(List<Company> companies)
        {
            Console.WriteLine("=== РОЗШИРЕНА СТАТИСТИКА (RECORD FEATURES) ===");

            // Створення статистики за профілями бізнесу з використанням record
            var statistics = companies
                .GroupBy(c => c.BusinessProfile)
                .Select(g => new CompanyStatistics(
                    g.Key,
                    g.Count(),
                    g.Sum(c => c.EmployeesCount),
                    g.Average(c => c.EmployeesCount),
                    g.Min(c => c.EmployeesCount),
                    g.Max(c => c.EmployeesCount)
                ))
                .OrderByDescending(s => s.TotalEmployees);

            foreach (var stat in statistics)
            {
                Console.WriteLine(stat);
            }
            Console.WriteLine();
        }

        // Демонстрація with expression - створення модифікованих копій
        public static void DemonstrateWithExpressions(List<Company> companies)
        {
            Console.WriteLine("=== ДЕМОНСТРАЦІЯ WITH EXPRESSIONS ===");

            var originalCompany = companies.First();
            Console.WriteLine($"Оригінал: {originalCompany}");

            // Створюємо копії з змінами за допомогою with
            var expandedCompany = originalCompany.WithNewEmployeeCount(originalCompany.EmployeesCount * 2);
            var relocatedCompany = originalCompany.WithNewAddress("Kyiv, Ukraine, Khreshchatyk 1");

            Console.WriteLine($"Розширена: {expandedCompany}");
            Console.WriteLine($"Переміщена: {relocatedCompany}");

            // Демонстрація деструктуризації
            var (id, name, foundationDate, profile, director, employees, address) = originalCompany;
            Console.WriteLine($"Деструктуризація: ID={id}, Name={name}, Employees={employees}");
            Console.WriteLine();
        }

        // Запити з використанням обчислюваних властивостей record
        public static void QueriesWithComputedProperties(List<Company> companies)
        {
            Console.WriteLine("=== ЗАПИТИ З ОБЧИСЛЮВАНИМИ ВЛАСТИВОСТЯМИ ===");

            // Великі компанії (використовуємо обчислювану властивість)
            var largeCompanies = companies.Where(c => c.IsLargeCompany);
            Console.WriteLine("Великі компанії:");
            foreach (var company in largeCompanies)
            {
                Console.WriteLine($"  {company.Name} ({company.EmployeesCount} співроб.)");
            }

            // Групування за містами (використовуємо обчислювану властивість City)
            Console.WriteLine("\nГрупування за містами:");
            var companiesByCity = companies.GroupBy(c => c.City);
            foreach (var cityGroup in companiesByCity)
            {
                Console.WriteLine($"  {cityGroup.Key}: {cityGroup.Count()} фірм");
            }

            // Фірми старше 3 років (використовуємо AgeInYears)
            Console.WriteLine("\nФірми старше 3 років:");
            var oldCompanies = companies.Where(c => c.AgeInYears > 3);
            foreach (var company in oldCompanies)
            {
                Console.WriteLine($"  {company.Name} - {company.AgeInYears:F1} років");
            }
            Console.WriteLine();
        }


        // Допоміжний метод — виклик усіх запитів (зручно в Main)
        public static void RunAllDemoMethods(List<Company> companies)
        {
            GroupCompaniesByBusinessProfile(companies);
            ShowStatistics(companies);
            ShowAdvancedStatistics(companies);
            DemonstrateWithExpressions(companies);
            QueriesWithComputedProperties(companies);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2._1
{
    public static class TaskQueries
    {
        // 1. Отримати інформацію про всі фірми
        public static void PrintAllCompanies(List<Company> companies)
        {
            Console.WriteLine("=== 1. ВСІ ФІРМИ ===");

            // Синтаксис запитів LINQ
            var allCompanies = from c in companies
                               select c;

            // Альтернатива через методи розширення
            // var allCompanies = companies.Select(c => c);

            foreach (var company in allCompanies)
            {
                Console.WriteLine(company);
            }
            Console.WriteLine();
        }

        // 2. Отримати фірми, які мають назву Food
        public static void PrintCompaniesWithNameFood(List<Company> companies)
        {
            Console.WriteLine("=== 2. ФІРМИ, ЯКІ МАЮТЬ НАЗВУ 'Food' ===");

            // Синтаксис запитів LINQ
            var foodCompanies = from c in companies
                                where c.Name.Contains("Food")
                                select c;

            // Альтернатива через методи розширення
            // var foodCompanies = companies.Where(c => c.Name.Contains("Food", StringComparison.OrdinalIgnoreCase));));

            if (!foodCompanies.Any())
            {
                Console.WriteLine("Не знайдено фірм з 'Food' у назві.");
            }
            else
            {
                foreach (var company in foodCompanies)
                {
                    Console.WriteLine(company);
                }
            }
            Console.WriteLine();
        }

        // 3. Отримати фірми, що працюють у галузі маркетингу
        public static void PrintMarketingCompanies(List<Company> companies)
        {
            Console.WriteLine("=== 3. ФІРМИ У ГАЛУЗІ МАРКЕТИНГУ ===");

            // Синтаксис запитів LINQ
            var marketingCompanies = from c in companies
                                     where c.BusinessProfile.Equals("Marketing", StringComparison.OrdinalIgnoreCase)
                                     select c;

            // Альтернатива через методи розширення
            // var marketingCompanies = companies.Where(c => c.BusinessProfile.Equals("Marketing", StringComparison.OrdinalIgnoreCase));

            if (!marketingCompanies.Any())
            {
                Console.WriteLine("Не знайдено маркетингових фірм.");
            }
            else
            {
                foreach (var company in marketingCompanies)
                {
                    Console.WriteLine(company);
                }
            }
            Console.WriteLine();
        }

        // 4. Отримати фірми, що працюють у галузі маркетингу або IT
        public static void PrintMarketingOrItCompanies(List<Company> companies)
        {
            Console.WriteLine("=== 4. ФІРМИ У ГАЛУЗІ МАРКЕТИНГУ АБО IT ==="); ;

            // Синтаксис запитів LINQ
            var marketingOrItCompanies = from c in companies
                                         where c.BusinessProfile.Equals("Marketing", StringComparison.OrdinalIgnoreCase) ||
                                               c.BusinessProfile.Equals("IT", StringComparison.OrdinalIgnoreCase)
                                         select c;

            // Альтернатива через методи розширення
            // var marketingOrItCompanies = companies.Where(c => 
            //     c.BusinessProfile.Equals("Marketing", StringComparison.OrdinalIgnoreCase) ||
            //     c.BusinessProfile.Equals("IT", StringComparison.OrdinalIgnoreCase));

            if (!marketingOrItCompanies.Any())
            {
                Console.WriteLine("Не знайдено фірм у профілі Marketing або IT.");
            }
            else
            {
                foreach (var company in marketingOrItCompanies)
                {
                    Console.WriteLine(company);
                }
            }
            Console.WriteLine();
        }

        // 5. Отримати фірми з кількістю співробітників більше 100
        public static void PrintCompaniesWithMoreThan100Employees(List<Company> companies)
        {
            Console.WriteLine("=== 5. ФІРМИ З КІЛЬКІСТЮ СПІВРОБІТНИКІВ > 100 ===");

            // Методи розширення
            var largeCompanies = companies.Where(c => c.EmployeesCount > 100);

            // Альтернатива через синтаксис запитів LINQ
            // var largeCompanies = from c in companies
            //                     where c.EmployeesCount > 100
            //                     select c;

            if (!largeCompanies.Any())
            {
                Console.WriteLine("Не знайдено фірм з >100 співробітників.");
            }
            else
            {
                foreach (var company in largeCompanies)
                {
                    Console.WriteLine(company);
                }
            }
            Console.WriteLine();
        }

        /// <summary>
        /// 6. Отримати фірми з кількістю співробітників у діапазоні від 100 до 300
        /// </summary>
        public static void PrintCompaniesWithEmployeesInRange100To300(List<Company> companies)
        {
            Console.WriteLine("=== 6. ФІРМИ З КІЛЬКІСТЮ СПІВРОБІТНИКІВ 100-300 ===");

            // Методи розширення
            var mediumCompanies = companies.Where(c => c.EmployeesCount >= 100 && c.EmployeesCount <= 300);

            if (!mediumCompanies.Any())
            {
                Console.WriteLine("Не знайдено фірм у діапазоні 100-300 співробітників.");
            }
            else
            {
                foreach (var company in mediumCompanies)
                {
                    Console.WriteLine(company);
                }
            }
            Console.WriteLine();
        }

        // 7. Отримати фірми, що знаходяться у Лондоні
        public static void PrintLondonCompanies(List<Company> companies)
        {
            Console.WriteLine("=== 7. ФІРМИ У ЛОНДОНІ ===");

            // Синтаксис запитів LINQ
            var londonCompanies = from c in companies
                                  where c.Address.Contains("London")
                                  select c;

            if (!londonCompanies.Any())
            {
                Console.WriteLine("Не знайдено фірм у Лондоні.");
            }
            else
            {
                foreach (var company in londonCompanies)
                {
                    Console.WriteLine(company);
                }
            }
            Console.WriteLine();
        }

        // 8. Отримати фірми, які мають прізвище директора White
        public static void PrintCompaniesWithDirectorWhite(List<Company> companies)
        {
            Console.WriteLine("=== 8. ФІРМИ З ДИРЕКТОРОМ (PRIZVISCHE: White) ===");

            // Методи розширення
            var whiteDirectorCompanies = companies.Where(c => c.DirectorFullName != null && c.DirectorFullName.Contains("White", StringComparison.OrdinalIgnoreCase));

            if (!whiteDirectorCompanies.Any())
            {
                Console.WriteLine("Не знайдено фірм з директором 'White'.");
            }
            else
            {
                foreach (var company in whiteDirectorCompanies)
                {
                    Console.WriteLine(company);
                }
            }
            Console.WriteLine();
        }

        // 9. Отримати фірми, які засновані понад два роки тому
        public static void PrintCompaniesFoundedMoreThanTwoYearsAgo(List<Company> companies)
        {
            Console.WriteLine("=== 9. ФІРМИ, ЗАСНОВАНІ ПОНАД 2 РОКИ ТОМУ ===");

            DateTime twoYearsAgo = DateTime.Now.AddYears(-2);

            // Синтаксис запитів LINQ
            var oldCompanies = from c in companies
                               where c.FoundationDate < twoYearsAgo
                               select c;

            if (!oldCompanies.Any())
            {
                Console.WriteLine("Не знайдено фірм, заснованих понад 2 роки тому.");
            }
            else
            {
                foreach (var company in oldCompanies)
                {
                    Console.WriteLine($"{company} (Засновано: {(DateTime.Now - company.FoundationDate).Days} днів тому)");
                }
            }
            Console.WriteLine();
        }

        // 10. Отримати фірми, з дня заснування яких минуло більше 150 днів
        public static void PrintCompaniesOlderThan150Days(List<Company> companies)
        {
            Console.WriteLine("=== 10. ФІРМИ, ЗАСНОВАНІ БІЛЬШЕ 150 ДНІВ ТОМУ ===");

            DateTime date150DaysAgo = DateTime.Now.AddDays(-150);

            // Методи розширення
            var companiesOlder150Days = companies.Where(c => c.FoundationDate < date150DaysAgo);

            if (!companiesOlder150Days.Any())
            {
                Console.WriteLine("Не знайдено фірм, заснованих понад 2 роки тому.");
            }
            else
            {
                foreach (var company in companiesOlder150Days)
                {
                    int daysOld = (DateTime.Now - company.FoundationDate).Days;
                    Console.WriteLine($"{company} (Засновано: {daysOld} днів тому)");
                }
            }
            Console.WriteLine();
        }

        // 11. Отримати фірми, у яких прізвище директора Black та назва фірми містить слово White
        public static void PrintCompaniesDirectorBlackAndNameContainsWhite(List<Company> companies)
        {
            Console.WriteLine("=== 11. ФІРМИ: директор має прізвище 'Black' І назва містить 'White' ===");

            // Синтаксис запитів LINQ
            var specificCompanies = from c in companies
                                    where c.DirectorFullName != null && c.DirectorFullName.Contains("Black", StringComparison.OrdinalIgnoreCase) &&
                                        c.Name != null && c.Name.Contains("White", StringComparison.OrdinalIgnoreCase)
                                    select c;


            foreach (var company in specificCompanies)
            {
                Console.WriteLine(company);
            }

            if (!specificCompanies.Any())
            {
                Console.WriteLine("Фірми з такими критеріями не знайдено.");
            }
            Console.WriteLine();
        }

        // Допоміжний метод — виклик усіх запитів (зручно в Main)
        public static void RunAllTaskQueries(List<Company> companies)
        {
            PrintAllCompanies(companies);
            PrintCompaniesWithNameFood(companies);
            PrintMarketingCompanies(companies);
            PrintMarketingOrItCompanies(companies);
            PrintCompaniesWithMoreThan100Employees(companies);
            PrintCompaniesWithEmployeesInRange100To300(companies);
            PrintLondonCompanies(companies);
            PrintCompaniesWithDirectorWhite(companies);
            PrintCompaniesFoundedMoreThanTwoYearsAgo(companies);
            PrintCompaniesOlderThan150Days(companies);
            PrintCompaniesDirectorBlackAndNameContainsWhite(companies);
        }
    }
}
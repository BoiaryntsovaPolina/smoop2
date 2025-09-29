using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2._1
{
    public static class CompanyFactory
    {
        // Створює колекцію тестових фірм використовуючи record syntax
        public static List<Company> CreateRandomCompanies(int count, int? seed = null, int startId = 1)
        {
            if (count <= 0) return new List<Company>();

            var rnd = seed.HasValue ? new Random(seed.Value) : new Random();

            var profiles = new[] { "IT", "Marketing", "Food Production", "Food Services", "Consulting", "Food Technology" };
            var cityNames = new[] { "London", "New York", "San Francisco", "Chicago", "Austin", "Seattle", "Boston", "Denver", "Miami", "Kyiv" };
            var companyPrefixes = new[] { "Solutions", "Services", "Innovations", "Experts", "Group", "Co", "Systems", "Labs" };
            var firstNames = new[] { "John", "Sarah", "Michael", "Emily", "Robert", "Jessica", "David", "Amanda", "Christopher", "Lisa", "James", "Maria" };
            var lastNames = new[] { "White", "Black", "Johnson", "Brown", "Wilson", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez" };
            var streetNames = new[] { "Main", "Broadway", "Oxford", "Regent", "Pine", "Harvard", "Innovation Drive", "Tech Avenue", "Ocean Drive", "Khreshchatyk" };

            var companies = new List<Company>(count);

            for (int i = 0; i < count; i++)
            {
                int id = startId + i;
                string profile = profiles[rnd.Next(profiles.Length)];

               
                string nameBase = rnd.NextDouble() < 0.25
                    ? "Food " + companyPrefixes[rnd.Next(companyPrefixes.Length)]
                    : $"{profile} {companyPrefixes[rnd.Next(companyPrefixes.Length)]}";

               
                if (rnd.NextDouble() < 0.3)
                    nameBase += $" {rnd.Next(1, 1000)}";

                string city = cityNames[rnd.Next(cityNames.Length)];
                string address = $"{city}, {streetNames[rnd.Next(streetNames.Length)]} {rnd.Next(1, 1000)}";

                // Дата заснування — між 1 місяцем і 13 роками (30 - 5000 днів)
                var foundation = DateTime.Now.AddDays(-rnd.Next(30, 5000));

                // Кількість співробітників — від 5 до 500
                int employees = rnd.Next(5, 501);

                // ПІБ директора
                string director = $"{firstNames[rnd.Next(firstNames.Length)]} {lastNames[rnd.Next(lastNames.Length)]}";

                var company = new Company(id, nameBase, foundation, profile, director, employees, address);
                companies.Add(company);
            }

            return companies;
        }

        // фіксований набір компаній для демонстрації
        public static List<Company> CreateSeedCompanies()
        {
            return new List<Company>
            {
                new Company(1, "Food Express", new DateTime(2020, 3, 15), "Marketing", "John White", 150, "London, Baker Street 221B"),
                new Company(2, "TechFood Solutions", new DateTime(2019, 7, 10), "IT", "Sarah Black", 75, "New York, Broadway 1234"),
                new Company(3, "Global Marketing", new DateTime(2021, 1, 20), "Marketing", "Michael Johnson", 250, "London, Oxford Street 45"),
                new Company(4, "IT Innovations", new DateTime(2018, 11, 5), "IT", "Emily White", 120, "San Francisco, Tech Avenue 789"),
                new Company(5, "Food Empire", new DateTime(2022, 5, 8), "Food Production", "Robert Wilson", 300, "Chicago, Main Street 567"),
                new Company(6, "Digital Marketing Pro", new DateTime(2017, 9, 12), "Marketing", "Jessica Brown", 85, "London, Regent Street 12"),
                new Company(7, "Software Solutions", new DateTime(2023, 2, 3), "IT", "David Miller", 45, "Seattle, Pine Street 890"),
                new Company(8, "White Consulting", new DateTime(2016, 6, 25), "Consulting", "Amanda Black", 180, "Boston, Harvard Avenue 456"),
                new Company(9, "Food Innovations", new DateTime(2019, 12, 14), "Food Technology", "Christopher Davis", 220, "Austin, Innovation Drive 123"),
                new Company(10, "Marketing Experts", new DateTime(2020, 8, 30), "Marketing", "Lisa Garcia", 95, "Miami, Ocean Drive 789"),
            };
        }

        // створює n випадкових + m фіксованих (seed) записів.
        public static List<Company> CreateMixedSample(int randomCount = 6, int? seed = null)
        {
            var seedList = CreateSeedCompanies();
            var randomList = CreateRandomCompanies(randomCount, seed, seedList.Count + 1);
            seedList.AddRange(randomList);
            return seedList;
        }


        // Демонстрація створення копій за допомогою with expression
        public static void DemonstrateRecordFeatures()
        {
            var originalCompany = new Company(
                1, "Test Company", DateTime.Now.AddYears(-1),
                "IT", "John Doe", 100, "Kyiv, Ukraine"
            );

            // Створення копії з змінами
            var expandedCompany = originalCompany.WithNewEmployeeCount(150);
            var relocatedCompany = originalCompany.WithNewAddress("Lviv, Ukraine");

            Console.WriteLine("=== ДЕМОНСТРАЦІЯ RECORD FEATURES ===");
            Console.WriteLine($"Оригінал: {originalCompany}");
            Console.WriteLine($"Розширена: {expandedCompany}");
            Console.WriteLine($"Переміщена: {relocatedCompany}");

            // Демонстрація equality
            var sameCompany = new Company(
                1, "Test Company", DateTime.Now.AddYears(-1),
                "IT", "John Doe", 100, "Kyiv, Ukraine"
            );

            Console.WriteLine($"Оригінал == Копія: {originalCompany == sameCompany}");
            Console.WriteLine($"Hash codes: {originalCompany.GetHashCode()} vs {sameCompany.GetHashCode()}");
        }
    }
}

using Lab2._2.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lab2._2
{
    public static class PhoneLinqService
    {
        public static int CountAllPhones(List<Phone> phones)
        {
            return phones.Count();
        }

        public static int CountPhonesAbovePrice(List<Phone> phones, decimal minPrice)
        {
            return phones.Count(p => p.Price > minPrice);
        }

        public static int CountPhonesInPriceRange(List<Phone> phones, decimal minPrice, decimal maxPrice)
        {
            return phones.Count(p => p.Price >= minPrice && p.Price <= maxPrice);
        }

        public static int CountPhonesByManufacturer(List<Phone> phones, string manufacturer)
        {
            return phones.Count(p => p.Manufacturer.Equals(manufacturer, StringComparison.OrdinalIgnoreCase));
        }

        public static Phone FindCheapestPhone(List<Phone> phones)
        {
            return phones.OrderBy(p => p.Price).First();
        }

        public static Phone FindMostExpensivePhone(List<Phone> phones)
        {
            return phones.OrderByDescending(p => p.Price).First();
        }


        public static Phone FindOldestPhone(List<Phone> phones)
        {
            return phones.OrderBy(p => p.ReleaseDate).First();
        }


        public static Phone FindNewestPhone(List<Phone> phones)
        {
            return phones.OrderByDescending(p => p.ReleaseDate).First();
        }


        public static decimal CalculateAveragePrice(List<Phone> phones)
        {
            return phones.Average(p => p.Price);
        }


        public static IEnumerable<Phone> GetTopExpensivePhones(List<Phone> phones, int count)
        {
            return phones.OrderByDescending(p => p.Price).Take(count);
        }

        public static IEnumerable<Phone> GetTopCheapestPhones(List<Phone> phones, int count)
        {
            return phones.OrderBy(p => p.Price).Take(count);
        }


        public static IEnumerable<Phone> GetOldestPhones(List<Phone> phones, int count)
        {
            return phones.OrderBy(p => p.ReleaseDate).Take(count);
        }

        public static IEnumerable<Phone> GetNewestPhones(List<Phone> phones, int count)
        {
            return phones.OrderByDescending(p => p.ReleaseDate).Take(count);
        }


        // Тепер повертаємо список об'єктів ManufacturerStat
        public static IEnumerable<ManufacturerStat> GetPhoneStatsByManufacturer(IEnumerable<Phone> phones)
        {
            if (phones == null) return Enumerable.Empty<ManufacturerStat>();

            return phones
                .GroupBy(p => p.Manufacturer)
                .Select(g => new ManufacturerStat(g.Key, g.Count()))
                .OrderByDescending(s => s.Count);
        }

        // Повертаємо список ModelStat
        public static IEnumerable<ModelStat> GetPhoneStatsByModel(IEnumerable<Phone> phones)
        {
            if (phones == null) return Enumerable.Empty<ModelStat>();

            return phones
                .GroupBy(p => p.Name)
                .Select(g => new ModelStat(g.Key, g.Count()))
                .OrderByDescending(s => s.Count);
        }

        // Повертаємо список YearStat
        public static IEnumerable<YearStat> GetPhoneStatsByYear(IEnumerable<Phone> phones)
        {
            if (phones == null) return Enumerable.Empty<YearStat>();

            return phones
                .GroupBy(p => p.ReleaseDate.Year)
                .Select(g => new YearStat(g.Key, g.Count()))
                .OrderByDescending(s => s.Year);
        }
    }
}

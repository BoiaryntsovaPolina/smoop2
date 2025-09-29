using Lab2._2.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2._2
{
    public static class PhoneDataService
    {
        public static List<Phone> GetPhones()
        {
            return new List<Phone>()
            {
                // Apple телефони
                new("iPhone 14", "Apple", 999m, new DateTime(2022, 9, 16)),
                new("iPhone 13", "Apple", 799m, new DateTime(2021, 9, 24)),
                new("iPhone 12", "Apple", 699m, new DateTime(2020, 10, 23)),
                new("iPhone SE", "Apple", 429m, new DateTime(2022, 3, 18)),
                
                // Samsung телефони
                new("Galaxy S23", "Samsung", 899m, new DateTime(2023, 2, 17)),
                new("Galaxy S22", "Samsung", 699m, new DateTime(2022, 2, 25)),
                new("Galaxy A54", "Samsung", 449m, new DateTime(2023, 3, 24)),
                new("Galaxy Note 20", "Samsung", 649m, new DateTime(2020, 8, 21)),
                
                // Sony телефони
                new("Xperia 1 IV", "Sony", 1299m, new DateTime(2022, 6, 1)),
                new("Xperia 5 III", "Sony", 999m, new DateTime(2021, 8, 27)),
                new("Xperia 10 IV", "Sony", 499m, new DateTime(2022, 5, 21)),
                
                // Google телефони
                new("Pixel 7", "Google", 599m, new DateTime(2022, 10, 13)),
                new("Pixel 6a", "Google", 449m, new DateTime(2022, 7, 21)),
                
                // Xiaomi телефони
                new("Mi 12", "Xiaomi", 749m, new DateTime(2022, 3, 15)),
                new("Redmi Note 11", "Xiaomi", 199m, new DateTime(2022, 1, 26)),
                new("Mi 11", "Xiaomi", 599m, new DateTime(2021, 2, 8)),
                
                // Huawei телефони
                new("P50 Pro", "Huawei", 1199m, new DateTime(2021, 7, 29)),
                new("Nova 9", "Huawei", 499m, new DateTime(2021, 10, 21)),
                
                // OnePlus телефони
                new("OnePlus 10 Pro", "OnePlus", 899m, new DateTime(2022, 3, 31)),
                new("Nord 2T", "OnePlus", 399m, new DateTime(2022, 5, 19))
            };
        }

        // Створює додаткові телефони для тестування (за потребою)
        public static List<Phone> GetSamplePhones()
        {
            return new List<Phone>()
            {
                new("iPhone 14", "Apple", 999m, new DateTime(2022, 9, 16)),
                new("Galaxy S23", "Samsung", 899m, new DateTime(2023, 2, 17)),
                new("Pixel 7", "Google", 599m, new DateTime(2022, 10, 13)),
                new("Xperia 1 IV", "Sony", 1299m, new DateTime(2022, 6, 1)),
                new("Redmi Note 11", "Xiaomi", 199m, new DateTime(2022, 1, 26))
            };
        }
    }
}

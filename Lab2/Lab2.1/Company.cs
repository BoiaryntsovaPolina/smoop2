using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2._1
{
    public record Company(
        int Id,
        string Name,
        DateTime FoundationDate,
        string BusinessProfile,
        string DirectorFullName,
        int EmployeesCount,
        string Address)
    {
        
        public int AgeInDays => (DateTime.Now - FoundationDate).Days;

        
        public double AgeInYears => (DateTime.Now - FoundationDate).Days / 365.25;

        
        public bool IsLargeCompany => EmployeesCount > 200;

        
        public string City => Address.Split(',')[0];

        
        public string DirectorLastName => DirectorFullName.Split(' ').LastOrDefault() ?? "";


        // (хоча record автоматично генерує гарний ToString, ми можемо його налаштувати)
        public override string ToString()
        {
            return $"{Name} | {FoundationDate:dd.MM.yyyy} | {BusinessProfile} | " +
                   $"{DirectorFullName} | {EmployeesCount} | {City} | " +
                   $"{AgeInDays} днів";
        }




        // Додаткові методи для демонстрації можливостей record

        // Створення копії з новою кількістю співробітників
        // Демонстрація with expression
        public Company WithNewEmployeeCount(int newCount) => this with { EmployeesCount = newCount };

        // Створення копії з новою адресою
        public Company WithNewAddress(string newAddress) => this with { Address = newAddress };
    }

    // Додатковий record для статистики (демонстрація nested records)
    public record CompanyStatistics(
        string BusinessProfile,
        int CompanyCount,
        int TotalEmployees,
        double AverageEmployees,
        int MinEmployees,
        int MaxEmployees)
    {
        public override string ToString()
        {
            return $"{BusinessProfile}: {CompanyCount} фірм, " +
                   $"всього {TotalEmployees} співроб., середньо {AverageEmployees:F1}";
        }
    }
}

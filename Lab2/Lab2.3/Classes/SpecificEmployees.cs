using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2._3.Classes
{
    // Record для Президента
    public record President(
        int Id,
        string FirstName,
        string LastName,
        DateTime BirthDate,
        DateTime HireDate,
        decimal Salary,
        EducationType Education,
        ProfessionalDirection Direction,
        decimal BonusPercentage = 50
    ) : Employer(Id, FirstName, LastName, BirthDate, HireDate, Salary, Education, Direction)
    {
        public decimal TotalSalaryWithBonus => Salary + Salary * BonusPercentage / 100;

        public decimal BonusPercentage { get; init; } = BonusPercentage >= 0 && BonusPercentage <= 100
            ? BonusPercentage
            : throw new ArgumentException("Відсоток бонусу повинен бути від 0 до 100");
    }

    // Record для Менеджера  
    public record Manager(
        int Id,
        string FirstName,
        string LastName,
        DateTime BirthDate,
        DateTime HireDate,
        decimal Salary,
        EducationType Education,
        ProfessionalDirection Direction,
        string Department,
        decimal BonusPercentage = 20
    ) : Employer(Id, FirstName, LastName, BirthDate, HireDate, Salary, Education, Direction)
    {
        public decimal TotalSalaryWithBonus => Salary + Salary * BonusPercentage / 100;


        public string Department { get; init; } = !string.IsNullOrWhiteSpace(Department)
            ? Department
            : throw new ArgumentException("Назва відділу не може бути пустою");

        public decimal BonusPercentage { get; init; } = BonusPercentage >= 0 && BonusPercentage <= 50
            ? BonusPercentage
            : throw new ArgumentException("Відсоток бонусу менеджера повинен бути від 0 до 50");

        public override string ToString()
        {
            return base.ToString() + $", Відділ: {Department}";
        }
    }

    // Record для Робітника
    public record Worker(
        int Id,
        string FirstName,
        string LastName,
        DateTime BirthDate,
        DateTime HireDate,
        decimal Salary,
        EducationType Education,
        ProfessionalDirection Direction,
        string Position,
        decimal HourlyRate = 0,
        int HoursPerMonth = 160
    ) : Employer(Id, FirstName, LastName, BirthDate, HireDate, Salary, Education, Direction)
    {
        public decimal MonthlyEarnings => HourlyRate > 0 ? HourlyRate * HoursPerMonth : Salary;


        public string Position { get; init; } = !string.IsNullOrWhiteSpace(Position)
            ? Position
            : throw new ArgumentException("Посада не може бути пустою");

        public decimal HourlyRate { get; init; } = HourlyRate >= 0
            ? HourlyRate
            : throw new ArgumentException("Годинна ставка не може бути від'ємною");

        public int HoursPerMonth { get; init; } = HoursPerMonth > 0 && HoursPerMonth <= 744 // максимум годин у місяці
            ? HoursPerMonth
            : throw new ArgumentException("Кількість годин на місяць повинна бути від 1 до 744");

        public override string ToString()
        {
            return base.ToString() + $", Посада: {Position}";
        }

        // Додатковий метод для створення копії з новою зарплатою
        public Worker WithNewSalary(decimal newSalary) => this with { Salary = newSalary };

        // Додатковий метод для підвищення на посаді
        public Worker WithPromotion(string newPosition, decimal newSalary) =>
            this with { Position = newPosition, Salary = newSalary };
    }

    // Додатковий record для статистики працівника (демонстрація nested records)
    public record EmployeeStatistics(
        Employer Employee,
        decimal AnnualSalary,
        int VacationDays,
        DateTime LastEvaluation
    )
    {
        // Обчислювана властивість
        public decimal MonthlyTax => AnnualSalary * 0.18m / 12; // 18% податок

        // Nested record для оцінки
        public record Evaluation(
            DateTime Date,
            int Score,
            string Comments
        );
    }
}

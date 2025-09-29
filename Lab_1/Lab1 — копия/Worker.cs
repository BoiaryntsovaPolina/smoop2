using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    [Serializable]
    public class Worker
    {
        public string FullName { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }


        public Worker() { }

        public Worker(string fullName, string position, decimal salary, DateTime hireDate)
        {
            FullName = fullName;
            Position = position;
            Salary = salary;
            HireDate = hireDate;
        }

        //Стаж
        public int GetWorkExperience()
        {
            int years = DateTime.Now.Year - HireDate.Year;

            if (DateTime.Now.DayOfYear < HireDate.DayOfYear)
            {
                years--;
            };

            return Math.Max(0, years);
        }


        //Збільшення окладу на 4%
        public void IncreaseSalary()
        {
            Salary = Salary * 1.04m;
        }

        public override string ToString()
        {
            return $"ПІБ: {FullName}, Посада: {Position}, Оклад: {Salary:C}, " +
                $"Дата найму: {HireDate:dd.MM.yyyy}, Стаж: {GetWorkExperience()} років";
        }

        public override bool Equals(object? obj)
        {
            if (obj is Worker other)
            {
                return FullName == other.FullName &&
                       Position == other.Position &&
                       HireDate == other.HireDate;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FullName, Position, HireDate);
        }
    }
}

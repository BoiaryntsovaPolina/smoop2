using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2._3.Classes
{
    
    public enum EducationType
    {
        Elementary,     
        Secondary,      // Середня
        Vocational,     // Професійна
        Higher          // Вища
    }

   
    public enum ProfessionalDirection
    {
        Technical,      
        Administrative, 
        Financial,      
        Marketing,      
        Production      
    }

    
    public abstract record Employer(
        int Id,
        string FirstName,
        string LastName,
        DateTime BirthDate,
        DateTime HireDate,
        decimal Salary,
        EducationType Education,
        ProfessionalDirection Direction)
    {
       
        public int Age => DateTime.Now.Year - BirthDate.Year -
                         (DateTime.Now.DayOfYear < BirthDate.DayOfYear ? 1 : 0);

        public int WorkExperience => DateTime.Now.Year - HireDate.Year -
                                   (DateTime.Now.DayOfYear < HireDate.DayOfYear ? 1 : 0);

        public string FullName => $"{FirstName} {LastName}";

        
        public override string ToString()
        {
            return $"{GetType().Name}: {FullName}, Вік: {Age}, Стаж: {WorkExperience}, Зарплата: {Salary:C}";
        }

       
        public int Id { get; init; } = Id > 0 ? Id : throw new ArgumentException("ID повинен бути більше 0");
        public string FirstName { get; init; } = !string.IsNullOrWhiteSpace(FirstName) ? FirstName : throw new ArgumentException("Ім'я не може бути пустим");
        public string LastName { get; init; } = !string.IsNullOrWhiteSpace(LastName) ? LastName : throw new ArgumentException("Прізвище не може бути пустим");
        public DateTime BirthDate { get; init; } = BirthDate < DateTime.Now ? BirthDate : throw new ArgumentException("Дата народження не може бути в майбутньому");
        public DateTime HireDate { get; init; } = HireDate <= DateTime.Now ? HireDate : throw new ArgumentException("Дата найму не може бути в майбутньому");
        public decimal Salary { get; init; } = Salary >= 0 ? Salary : throw new ArgumentException("Зарплата не може бути від'ємною");
    }
}

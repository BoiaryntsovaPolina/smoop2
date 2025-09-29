using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2._3.Classes
{
    internal class Company
    {
        public string Name { get; set; }
        public List<Employer> Employees { get; set; }

        public Company(string name)
        {
            Name = name;
            Employees = new List<Employer>();
        }

        public Company () { }

        // Додавання працівника
        public void AddEmployee(Employer employee)
        {
            Employees.Add(employee);
        }

        // Кількість робітників підприємства
        public int GetWorkerCount()
        {
            return Employees.OfType<Worker>().Count();
        }

        // Об'єм заробітної платні, що необхідно виплатити всім робітникам
        public decimal GetTotalSalaryForWorkers()
        {
            return Employees.OfType<Worker>()
                          .Sum(w => w.MonthlyEarnings);
        }

        // Загальний об'єм заробітної платні для всіх працівників
        public decimal GetTotalSalaryForAllEmployees()
        {
            decimal total = 0;

            total += Employees.OfType<Worker>().Sum(w => w.MonthlyEarnings);
            total += Employees.OfType<Manager>().Sum(m => m.TotalSalaryWithBonus);
            total += Employees.OfType<President>().Sum(p => p.TotalSalaryWithBonus);

            return total;
        }

        // 10 робітників з найбільшим стажем, серед яких найменший за віком з вищою освітою
        public Worker GetYoungestWorkerWithHigherEducationFromTop10Experienced()
        {
            return Employees.OfType<Worker>()
                          .OrderByDescending(w => w.WorkExperience)
                          .Take(10)
                          .Where(w => w.Education == EducationType.Higher)
                          .OrderBy(w => w.Age)
                          .FirstOrDefault();
        }

        // Наймолодший та найстарший менеджери
        public (Manager Youngest, Manager Oldest) GetYoungestAndOldestManagers()
        {
            var managers = Employees.OfType<Manager>().ToList();

            if (!managers.Any())
                return (null, null);

            var youngest = managers.OrderBy(m => m.Age).First();
            var oldest = managers.OrderByDescending(m => m.Age).First();

            return (youngest, oldest);
        }

        // Робітники, що народилися у жовтні, згруповані за професійним спрямуванням
        public IGrouping<ProfessionalDirection, Worker>[] GetWorkersGroupedByDirectionBornInOctober()
        {
            return Employees.OfType<Worker>()
                          .Where(w => w.BirthDate.Month == 10)
                          .GroupBy(w => w.Direction)
                          .ToArray();
        }

        // Всі Володимири на підприємстві
        public List<Employer> GetAllVolodymyrs()
        {
            return Employees.Where(e => e.FirstName.Equals("Володимир", StringComparison.OrdinalIgnoreCase))
                          .ToList();
        }

        // Наймолодший Володимир
        public Employer GetYoungestVolodymyr()
        {
            return GetAllVolodymyrs()
                  .OrderBy(v => v.Age)
                  .FirstOrDefault();
        }

        // Обчислення премії (третина від окладу)
        public decimal CalculateBonusForVolodymyr(Employer volodymyr)
        {
            if (volodymyr == null) return 0;
            return volodymyr.Salary / 3;
        }

        // Вивід всіх працівників
        public void DisplayAllEmployees()
        {
            Console.WriteLine($"\n=== Працівники підприємства '{Name}' ===");

            var groupedByType = Employees.GroupBy(e => e.GetType().Name);

            foreach (var group in groupedByType)
            {
                Console.WriteLine($"\n{group.Key}и:");
                foreach (var employee in group)
                {
                    Console.WriteLine($"  {employee}");
                }
            }
        }
    }
}

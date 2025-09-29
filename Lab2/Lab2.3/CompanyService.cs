using Lab2._3.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2._3
{
    internal class CompanyService
    {
        private readonly Company _company;

        public CompanyService(Company company)
        {
            _company = company ?? throw new ArgumentNullException(nameof(company));
        }

        public void ExecuteAllTasks()
        {
            DisplayCompanyInfo();
            ExecuteTask1();
            ExecuteTask2();
            ExecuteTask3();
            ExecuteTask4();
            ExecuteTask5();
            ExecuteTask6();
        }

        private void DisplayCompanyInfo()
        {
            Console.WriteLine($"\n{'=',-60}");
            Console.WriteLine($"ІНФОРМАЦІЯ ПРО ПІДПРИЄМСТВО '{_company.Name.ToUpper()}'");
            Console.WriteLine($"{'=',-60}");

            _company.DisplayAllEmployees();

            Console.WriteLine($"\nЗагальна статистика:");
            Console.WriteLine($"  Всього працівників: {_company.Employees.Count}");
            Console.WriteLine($"  Президентів: {_company.Employees.OfType<President>().Count()}");
            Console.WriteLine($"  Менеджерів: {_company.Employees.OfType<Manager>().Count()}");
            Console.WriteLine($"  Робітників: {_company.Employees.OfType<Worker>().Count()}");
        }

        // Завдання 1: Кількість робітників підприємства
        private void ExecuteTask1()
        {
            Console.WriteLine($"\n{'=',-60}");
            Console.WriteLine("ЗАВДАННЯ 1: КІЛЬКІСТЬ РОБІТНИКІВ");
            Console.WriteLine($"{'=',-60}");

            var workerCount = _company.GetWorkerCount();
            Console.WriteLine($"Кількість робітників на підприємстві: {workerCount}");

            // Додаткова інформація
            var totalEmployees = _company.Employees.Count;
            var percentage = totalEmployees > 0 ? (double)workerCount / totalEmployees * 100 : 0;
            Console.WriteLine($"Частка робітників від загальної кількості працівників: {percentage:F1}%");
        }

        // Завдання 2: Об'єм заробітної платні для робітників
        private void ExecuteTask2()
        {
            Console.WriteLine($"\n{'=',-60}");
            Console.WriteLine("ЗАВДАННЯ 2: ОБ'ЄМ ЗАРОБІТНОЇ ПЛАТНІ");
            Console.WriteLine($"{'=',-60}");

            var workersSalary = _company.GetTotalSalaryForWorkers();
            var totalSalary = _company.GetTotalSalaryForAllEmployees();

            Console.WriteLine($"Об'єм заробітної платні для робітників: {workersSalary:C}");
            Console.WriteLine($"Загальний об'єм заробітної платні (всі працівники): {totalSalary:C}");

            if (totalSalary > 0)
            {
                var percentage = (double)workersSalary / (double)totalSalary * 100;
                Console.WriteLine($"Частка зарплат робітників: {percentage:F1}%");
            }
        }

        // Завдання 3: Найменший за віком робітник з вищою освітою серед топ-10 за стажем
        private void ExecuteTask3()
        {
            Console.WriteLine($"\n{'=',-60}");
            Console.WriteLine("ЗАВДАННЯ 3: АНАЛІЗ ДОСВІДЧЕНИХ РОБІТНИКІВ");
            Console.WriteLine($"{'=',-60}");

            // Показуємо топ-10 робітників за стажем
            var top10Workers = _company.Employees.OfType<Worker>()
                .OrderByDescending(w => w.WorkExperience)
                .Take(10)
                .ToList();

            Console.WriteLine("Топ-10 робітників за стажем роботи:");
            for (int i = 0; i < top10Workers.Count; i++)
            {
                var worker = top10Workers[i];
                var mark = worker.Education == EducationType.Higher ? " (Вища освіта)" : "";
                Console.WriteLine($"  {i + 1,2}. {worker.FullName,-25} | Стаж: {worker.WorkExperience,2} р. | Вік: {worker.Age,2} р.{mark}");
            }

            // Знаходимо потрібного робітника
            var selectedWorker = _company.GetYoungestWorkerWithHigherEducationFromTop10Experienced();

            Console.WriteLine($"\nРезультат пошуку:");
            if (selectedWorker != null)
            {
                Console.WriteLine($"Найменший за віком робітник з вищою освітою серед топ-10:");
                Console.WriteLine($"   {selectedWorker}");
                Console.WriteLine($"   Освіта: {selectedWorker.Education}");
            }
            else
            {
                Console.WriteLine("Серед топ-10 найдосвідченіших робітників немає працівників з вищою освітою.");
            }
        }

        // Завдання 4: Наймолодший та найстарший менеджери
        private void ExecuteTask4()
        {
            Console.WriteLine($"\n{'=',-60}");
            Console.WriteLine("ЗАВДАННЯ 4: АНАЛІЗ МЕНЕДЖЕРІВ");
            Console.WriteLine($"{'=',-60}");

            var managers = _company.Employees.OfType<Manager>().ToList();

            if (!managers.Any())
            {
                Console.WriteLine("На підприємстві немає менеджерів.");
                return;
            }

            Console.WriteLine($"Всі менеджери підприємства ({managers.Count}):");
            foreach (var manager in managers.OrderBy(m => m.Age))
            {
                Console.WriteLine($"  {manager}");
            }

            var (youngest, oldest) = _company.GetYoungestAndOldestManagers();

            Console.WriteLine($"\nРезультат аналізу:");
            Console.WriteLine($"Наймолодший менеджер: {youngest.FullName} ({youngest.Age} років)");
            Console.WriteLine($"Найстарший менеджер: {oldest.FullName} ({oldest.Age} років)");
            Console.WriteLine($"Різниця у віці: {oldest.Age - youngest.Age} років");
        }

        // Завдання 5: Робітники, народжені у жовтні, згруповані за професійним спрямуванням
        private void ExecuteTask5()
        {
            Console.WriteLine($"\n{'=',-60}");
            Console.WriteLine("ЗАВДАННЯ 5: РОБІТНИКИ, НАРОДЖЕНІ У ЖОВТНІ");
            Console.WriteLine($"{'=',-60}");

            var octoberWorkers = _company.GetWorkersGroupedByDirectionBornInOctober();

            if (!octoberWorkers.Any())
            {
                Console.WriteLine("Не знайдено робітників, що народилися у жовтні.");
                return;
            }

            var totalOctoberWorkers = octoberWorkers.Sum(g => g.Count());
            Console.WriteLine($"Знайдено {totalOctoberWorkers} робітників, що народилися у жовтні:");

            foreach (var group in octoberWorkers.OrderBy(g => g.Key))
            {
                Console.WriteLine($"\n{group.Key} ({group.Count()} осіб):");
                foreach (var worker in group.OrderBy(w => w.BirthDate.Day))
                {
                    Console.WriteLine($"  {worker.FullName,-25} | {worker.BirthDate:dd.MM.yyyy} | {worker.Position}");
                }
            }
        }

        // Завдання 6: Всі Володимири та поздоровлення наймолодшого з премією
        private void ExecuteTask6()
        {
            Console.WriteLine($"\n{'=',-60}");
            Console.WriteLine("ЗАВДАННЯ 6: ВОЛОДИМИРИ НА ПІДПРИЄМСТВІ");
            Console.WriteLine($"{'=',-60}");

            var allVolodymyrs = _company.GetAllVolodymyrs();

            if (!allVolodymyrs.Any())
            {
                Console.WriteLine("На підприємстві немає працівників на ім'я Володимир.");
                return;
            }

            Console.WriteLine($"Знайдено {allVolodymyrs.Count} Володимирів на підприємстві:");

            foreach (var volodymyr in allVolodymyrs.OrderBy(v => v.Age))
            {
                var position = GetEmployeePosition(volodymyr);
                Console.WriteLine($"  {volodymyr.FullName,-25} | Вік: {volodymyr.Age,2} р. | {position}");
            }

            var youngestVolodymyr = _company.GetYoungestVolodymyr();
            var bonus = _company.CalculateBonusForVolodymyr(youngestVolodymyr);

            Console.WriteLine($"\nВІТАЄМО З ПРЕМІЄЮ!");
            Console.WriteLine($"{'─',-50}┐");
            Console.WriteLine($"Найменший за віком Володимир:{youngestVolodymyr.FullName,15} │");
            Console.WriteLine($"Вік: {youngestVolodymyr.Age,41} років │");
            Console.WriteLine($"Посадовий оклад: {youngestVolodymyr.Salary,26:C} │");
            Console.WriteLine($"Премія (1/3 окладу): {bonus,22:C} │");
            Console.WriteLine($"Загальна винагорода: {youngestVolodymyr.Salary + bonus,18:C} │");
            Console.WriteLine($"{'─',-50}");
        }

        // Отримує посаду працівника залежно від його типу
        private static string GetEmployeePosition(Employer employee)
        {
            return employee switch
            {
                President => "Президент",
                Manager manager => $"Менеджер ({manager.Department})",
                Worker worker => $"Робітник ({worker.Position})",
                _ => "Невідома посада"
            };
        }

        // Виводить підсумок виконання всіх завдань
        public void DisplaySummary()
        {
            Console.WriteLine($"\n{'=',-60}");
            Console.WriteLine("ПІДСУМОК ВИКОНАННЯ ЗАВДАНЬ");
            Console.WriteLine($"{'=',-60}");

            Console.WriteLine("Завдання 1: Підраховано кількість робітників");
            Console.WriteLine("Завдання 2: Обчислено загальну заробітну платню");
            Console.WriteLine("Завдання 3: Знайдено найменшого за віком досвідченого робітника з вищою освітою");
            Console.WriteLine("Завдання 4: Визначено наймолодшого та найстаршого менеджерів");
            Console.WriteLine("Завдання 5: Згруповано робітників за професійним спрямуванням (народжені у жовтні)");
            Console.WriteLine("Завдання 6: Знайдено всіх Володимирів та нагороджено наймолодшого");

            Console.WriteLine($"\nВсі завдання лабораторної роботи виконано успішно!");
        }
    }
}

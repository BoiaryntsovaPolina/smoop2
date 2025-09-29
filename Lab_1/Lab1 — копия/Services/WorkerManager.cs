using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lab1.Services
{
    internal class WorkerManager
    {
        private List<Worker> workers;

        public WorkerManager()
        {
            workers = new List<Worker>();
        }

        public void AddWorker(Worker worker) 
        {
            if (worker != null) 
            { 
                workers.Add(worker);
                Console.WriteLine($"Робітника '{worker.FullName}' додано до списку.");
            }
        }

        public void AddWorkers(List<Worker> newWorkers) 
        {
            if(newWorkers != null && newWorkers.Count > 0)
            {
                workers.AddRange(newWorkers);
                Console.WriteLine($"Додано {newWorkers.Count} робітників до списку.");
            }
        }

        public bool InsertWorker(int index, Worker worker)
        {
            if (index >= 0 && index <= workers.Count && worker != null)
            {
                workers.Insert(index, worker);
                Console.WriteLine($"Робітника '{worker.FullName}' вставлено на позицію {index}.");
                return true;
            }
            Console.WriteLine("Неправильний індекс для вставки робітника.");
            return false;
        }


        public bool RemoveWorkerAt(int index) 
        {
            if (index >= 0 && index < workers.Count)
            {
                string removedName = workers[index].FullName;
                workers.RemoveAt(index);
                Console.WriteLine($"Робітника '{removedName}' видалено з позиції {index}.");
                return true;
            }
            Console.WriteLine("Неправильний індекс для видалення робітника.");
            return false;
        }

        public bool RemoveWorker(Worker worker)
        {
            if(workers.Remove(worker))
            {
                Console.WriteLine($"Робітника '{worker.FullName}' видалено зі списку.");
                return true;
            }
            Console.WriteLine("Робітника не знайдено у списку.");
            return false;
        }

        // Видалення робітника за ПІБ
        public bool RemoveWorkerByName(string fullName)
        {
            for (int i = workers.Count - 1; i >= 0; i--)
            {
                if (workers[i].FullName.Equals(fullName, StringComparison.OrdinalIgnoreCase))
                {
                    workers.RemoveAt(i);
                    Console.WriteLine($"Робітника '{fullName}' видалено зі списку.");
                    return true;
                }
            }
            Console.WriteLine($"Робітника з ПІБ '{fullName}' не знайдено у списку.");
            return false;
        }

        // Видалення всіх робітників з певною посадою
        public int RemoveWorkersByPosition(string position)
        {
            int removedCount = 0;
            for (int i = workers.Count - 1; i >= 0; i--)
            {
                if (workers[i].Position.Equals(position, StringComparison.OrdinalIgnoreCase))
                {
                    workers.RemoveAt(i);
                    removedCount++;
                }
            }
            Console.WriteLine($"Видалено {removedCount} робітників з посадою '{position}'.");
            return removedCount;
        }

        // Редагування інформації про робітника
        public bool EditWorker(int index, string? fullName = null, string? position = null,
                              decimal? salary = null, DateTime? hireDate = null)
        {
            if (index >= 0 && index < workers.Count)
            {
                Worker worker = workers[index];

                if (fullName != null) worker.FullName = fullName;
                if (position != null) worker.Position = position;
                if (salary.HasValue) worker.Salary = salary.Value;
                if (hireDate.HasValue) worker.HireDate = hireDate.Value;

                Console.WriteLine($"Інформацію про робітника оновлено: {worker}");
                return true;
            }
            Console.WriteLine("Неправильний індекс для редагування робітника.");
            return false;
        }

        // Пошук робітника за ПІБ
        public int FindWorkerIndex(string fullName)
        {
            for (int i = 0; i < workers.Count; i++)
            {
                if (workers[i].FullName.Equals(fullName, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }
            return -1;
        }

        // Отримання робітника за індексом
        public Worker? GetWorker(int index)
        {
            if (index >= 0 && index < workers.Count)
            {
                return workers[index];
            }
            return null;
        }

        // Знаходження робітників зі стажем більше заданого значення
        public List<Worker> GetWorkersWithExperience(int minYears)
        {
            List<Worker> result = new List<Worker>();

            for (int i = 0; i < workers.Count; i++)
            {
                if (workers[i].GetWorkExperience() > minYears)
                {
                    result.Add(workers[i]);
                }
            }
            return result;
        }

        // Збільшення окладу для робітників зі стажем більше заданого значення
        public void IncreaseSalaryForExperiencedWorkers(int minYears)
        {
            List<Worker> experiencedWorkers = GetWorkersWithExperience(minYears);

            Console.WriteLine($"\nРобітники зі стажем більше {minYears} років:");
            Console.WriteLine(new string('-', 80));

            if (experiencedWorkers.Count == 0)
            {
                Console.WriteLine($"Немає робітників зі стажем більше {minYears} років.");
                return;
            }

            for (int i = 0; i < experiencedWorkers.Count; i++)
            {
                decimal oldSalary = experiencedWorkers[i].Salary;
                experiencedWorkers[i].IncreaseSalary();

                Console.WriteLine($"{i + 1}. {experiencedWorkers[i]}");
                Console.WriteLine($"   Оклад збільшено: {oldSalary:C} → {experiencedWorkers[i].Salary:C}");
                Console.WriteLine();
            }
        }

        // Отримання всіх робітників
        public List<Worker> GetAllWorkers()
        {
            return new List<Worker>(workers);
        }

        // Виведення всіх робітників на екран
        public void DisplayAllWorkers()
        {
            Console.WriteLine($"\nЗагальна кількість робітників: {workers.Count}");
            Console.WriteLine(new string('-', 80));

            if (workers.Count == 0)
            {
                Console.WriteLine("Список робітників порожній.");
                return;
            }

            for (int i = 0; i < workers.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {workers[i]}");
            }
        }

        // Збереження списку робітників у файл (текстовий формат)
        public void SaveToFile(string fileName)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.UTF8))
                {
                    writer.WriteLine($"СПИСОК РОБІТНИКІВ (станом на {DateTime.Now:dd.MM.yyyy HH:mm})");
                    writer.WriteLine(new string('=', 80));
                    writer.WriteLine($"Загальна кількість: {workers.Count}");
                    writer.WriteLine();

                    if (workers.Count == 0)
                    {
                        writer.WriteLine("Список робітників порожній.");
                    }
                    else
                    {
                        for (int i = 0; i < workers.Count; i++)
                        {
                            writer.WriteLine($"{i + 1}. {workers[i]}");
                        }
                    }
                }
                Console.WriteLine($"Дані збережено у файл '{fileName}'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при збереженні у файл: {ex.Message}");
            }
        }


        // JSON серіалізація списку робітників
        public async Task SaveToJsonFileAsync(string fileName)
        {
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    await JsonSerializer.SerializeAsync(fs, workers);
                }

                Console.WriteLine($"Список робітників серіалізовано у JSON файл: {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка JSON серіалізації: {ex.Message}");
            }
        }

        public async Task<bool> LoadFromJsonFileAsync(string fileName)
        {
            try
            {
                if (!File.Exists(fileName))
                {
                    Console.WriteLine($"JSON файл {fileName} не знайдено.");
                    return false;
                }

                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    List<Worker>? loadedWorkers = await JsonSerializer.DeserializeAsync<List<Worker>>(fs);

                    if (loadedWorkers != null)
                    {
                        workers.Clear();
                        workers.AddRange(loadedWorkers);
                        Console.WriteLine($"Завантажено {workers.Count} робітників з JSON файлу: {fileName}");
                        return true;
                    }
                }

                Console.WriteLine("Не вдалося десеріалізувати дані з JSON файлу.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка JSON десеріалізації: {ex.Message}");
                return false;
            }
        }

        // JSON серіалізація одного робітника
        public async Task SaveWorkerToJsonFileAsync(Worker worker, string fileName)
        {
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    await JsonSerializer.SerializeAsync(fs, worker);
                }

                Console.WriteLine($"Робітник серіалізований у JSON файл: {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка JSON серіалізації робітника: {ex.Message}");
            }
        }

        // JSON десеріалізація одного робітника
        public async Task<Worker?> LoadWorkerFromJsonFileAsync(string fileName)
        {
            try
            {
                if (!File.Exists(fileName))
                {
                    Console.WriteLine($"JSON файл {fileName} не знайдено.");
                    return null;
                }

                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    Worker? worker = await JsonSerializer.DeserializeAsync<Worker>(fs);

                    if (worker != null)
                    {
                        Console.WriteLine($"Робітник завантажений з JSON файлу: {fileName}");
                        return worker;
                    }
                }

                Console.WriteLine("Не вдалося десеріалізувати робітника з JSON файлу.");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка JSON десеріалізації робітника: {ex.Message}");
                return null;
            }
        }

        // Очищення списку
        public void Clear()
        {
            workers.Clear();
            Console.WriteLine("Список робітників очищено.");
        }

        // Отримання всіх робітників як Dictionary (для серіалізації)
        public Dictionary<string, string> GetWorkersDictionary()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            for (int i = 0; i < workers.Count; i++)
            {
                result.Add($"Worker_{i + 1}", workers[i].ToString());
            }

            return result;
        }

        // Отримання кількості робітників
        public int Count => workers.Count;

    }
}

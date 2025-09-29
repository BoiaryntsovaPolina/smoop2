using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Services
{
    internal class DemonstrationService
    {
        private readonly WorkerManager manager = new WorkerManager();


        public async Task RunDemoAsync()
        {

            DemonstrateAddOperations();
            DemonstrateRemoveOperations();
            DemonstrateEditOperations();


            await Task.CompletedTask;
        }

        private void DemonstrateAddOperations()
        {
            // Правильні типи:
            Worker newWorker = WorkerDataGenerator.CreateRandomWorker();
            manager.AddWorker(newWorker);

            List<Worker> two = WorkerDataGenerator.CreateRandomWorkers(2);
            manager.AddWorkers(two);

            Worker insertWorker = WorkerDataGenerator.CreateRandomWorker();
            bool inserted = manager.InsertWorker(2, insertWorker);
            Console.WriteLine(inserted ? "Вставлено робітника на позицію 2." : "Не вдалося вставити (індекс за межами списку).");

            manager.DisplayAllWorkers();
        }

        private void DemonstrateRemoveOperations()
        {
            if (manager.Count > 3)
            {
                bool removedAt = manager.RemoveWorkerAt(3);
                Console.WriteLine(removedAt ? "Видалено робітника на індексі 3." : "Не вдалося видалити за індексом 3.");
            }

            var first = manager.GetWorker(0);
            if (first != null)
            {
                bool removedByName = manager.RemoveWorkerByName(first.FullName);
                Console.WriteLine(removedByName ? $"Видалено робітника {first.FullName} за ім'ям." : "Не вдалося знайти робітника для видалення за іменем.");
            }

            int removedCount = manager.RemoveWorkersByPosition("QA Tester");
            Console.WriteLine($"Видалено {removedCount} робітників позиції 'QA Tester'.");

            manager.DisplayAllWorkers();
        }

        private void DemonstrateEditOperations()
        {
            var w = manager.GetWorker(0);
            if (w == null) return;

            bool edited = manager.EditWorker(0, fullName: null, position: "Senior " + w.Position, salary: w.Salary + 5000m);
            Console.WriteLine(edited ? "Робітника оновлено." : "Не вдалося оновити робітника.");

            manager.DisplayAllWorkers();
        }
    }
}

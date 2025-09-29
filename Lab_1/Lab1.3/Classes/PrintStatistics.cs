using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1._3.Classes
{
    internal class PrintStatistics
    {
        public string User { get; set; }
        public string DocumentName { get; set; }
        public DateTime PrintTime { get; set; }
        public int Pages { get; set; }
        public int Priority { get; set; }
        public DateTime CreationTime { get; set; }


        public PrintStatistics() { }

        public PrintStatistics(PrintJob job, DateTime printTime)
        {
            User = job.User;
            DocumentName = job.DocumentName;
            PrintTime = printTime;
            Pages = job.Pages;
            Priority = job.Priority;
            CreationTime = job.CreationTime;
        }

        public override string ToString()
        {
            // Рахуємо різницю у секундах між створенням і друком
            double waitSeconds = (PrintTime - CreationTime).TotalSeconds;

            return $"{User} | {DocumentName} | {Pages} стор. | " +
                   $"Пріоритет: {Priority} | Час друку: {PrintTime:dd.MM.yyyy HH:mm:ss} | " +
                   $"Час очікування: {waitSeconds:F1} сек.";
        }


        public override bool Equals(object obj)
        {
            if (obj is PrintStatistics other)
            {
                return User == other.User &&
                       DocumentName == other.DocumentName &&
                       PrintTime == other.PrintTime &&
                       Pages == other.Pages &&
                       Priority == other.Priority &&
                       CreationTime == other.CreationTime;

            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(User, DocumentName, PrintTime, Pages, Priority, CreationTime);
        }
    }
}

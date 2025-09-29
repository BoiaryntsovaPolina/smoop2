using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1._3.Classes
{
    internal class PrintJob
    {
        public string User { get; set; }
        public string DocumentName { get; set; }
        public int Priority { get; set; } // 1 - найвищий, 5 - найнижчий
        public DateTime CreationTime { get; set; }
        public int Pages { get; set; }


        // public getter, internal setter — щоб дозволити присвоювати лише зсередини збірки (наприклад, PrinterQueue).
        public long Sequence { get; internal set; } = 0;

        public PrintJob() { }

        public PrintJob(string user, string documentName, int priority, int pages)       
        {
            User = user;
            DocumentName = documentName;
            Priority = priority;
            Pages = pages;
            CreationTime = DateTime.Now;
        }

        public override string ToString()                               
        {
            return $"Користувач: {User}, Документ: {DocumentName}, " +
                   $"Пріоритет: {Priority}, Сторінок: {Pages}, " +
                   $"Час створення: {CreationTime:dd.MM.yyyy HH:mm:ss}";
        }

        public override bool Equals(object obj)
        {
            if (obj is PrintJob other)
            {
                return Sequence == other.Sequence &&
                       User == other.User &&
                       DocumentName == other.DocumentName &&
                       Priority == other.Priority;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Sequence, User, DocumentName, Priority);
        }
    }
}

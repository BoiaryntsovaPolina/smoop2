using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Lab8.Models
{
    public interface IStudentContainer : IEnumerable<Student>, ICollection<Student>, IEnumerable, INotifyCollectionChanged
    {
        Student this[int index] { get; set; }
        int IndexOf(Student item);
        void Insert(int index, Student item);
        void RemoveAt(int index);
    }
}

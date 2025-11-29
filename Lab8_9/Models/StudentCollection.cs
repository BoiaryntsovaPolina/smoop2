using System.Collections.ObjectModel;

namespace Lab8.Models
{
    public class StudentCollection : ObservableCollection<Student>, IStudentContainer
    {
        public StudentCollection() : base() { }
        public StudentCollection(System.Collections.Generic.IEnumerable<Student> collection) : base(collection) { }
    }
}

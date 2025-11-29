using Lab8.Helpers;
using Lab8.Models;
using Lab8.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Collections.Specialized;
using System.Xml.Serialization;

namespace Lab8.ViewModels
{
    public class StudentsViewModel : INotifyPropertyChanged
    {
        // ЗМІНА: Використовуємо .txt для зручного відкриття у Блокноті
        private const string DataFile = "Students.txt";

        public IStudentContainer Students { get; }
        private Student _selectedStudent;

        public Student SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                if (_selectedStudent == value) return;
                if (_selectedStudent != null) _selectedStudent.PropertyChanged -= SelectedStudent_PropertyChanged;
                _selectedStudent = value;
                if (_selectedStudent != null) _selectedStudent.PropertyChanged += SelectedStudent_PropertyChanged;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanEdit));
                OnPropertyChanged(nameof(SelectedStudentErrors));
                OnPropertyChanged(nameof(SelectedStudentHasErrors));
            }
        }

        private void SelectedStudent_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Student.FirstName) ||
                e.PropertyName == nameof(Student.LastName) ||
                e.PropertyName == nameof(Student.AgeText) ||
                e.PropertyName == nameof(Student.Gender) ||
                string.IsNullOrEmpty(e.PropertyName))
            {
                OnPropertyChanged(nameof(SelectedStudentErrors));
                OnPropertyChanged(nameof(SelectedStudentHasErrors));
            }
        }

        public IEnumerable<string> SelectedStudentErrors => SelectedStudent == null ? Enumerable.Empty<string>() : SelectedStudent.GetValidationErrors().Distinct().ToList();
        public bool SelectedStudentHasErrors => SelectedStudentErrors.Any();
        public bool HasStudents => Students != null && Students.Count > 0;
        public bool CanEdit => SelectedStudent != null;

        public bool HasInvalidEntries => Students.Cast<Student>().Any(s => !s.IsValid);
        public bool HasAnyDirty => Students.Cast<Student>().Any(s => s.IsDirty);

        public StudentsViewModel(IStudentContainer students)
        {
            Students = students ?? throw new ArgumentNullException(nameof(students));
            if (Students is INotifyCollectionChanged nc)
            {
                nc.CollectionChanged += (s, e) =>
                {
                    if (e.NewItems != null)
                    {
                        foreach (Student st in e.NewItems) st.PropertyChanged += Student_PropertyChanged;
                    }
                    if (e.OldItems != null)
                    {
                        foreach (Student st in e.OldItems) st.PropertyChanged -= Student_PropertyChanged;
                    }
                    UpdateIndices();
                    OnPropertyChanged(nameof(HasStudents));
                    OnPropertyChanged(nameof(HasInvalidEntries));
                    OnPropertyChanged(nameof(HasAnyDirty));
                    CommandManager.InvalidateRequerySuggested();
                };
            }
            LoadData();
            foreach (var s in Students) s.PropertyChanged += Student_PropertyChanged;
        }

        private void Student_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Student.IsDirty) || e.PropertyName == nameof(Student.IsValid))
            {
                OnPropertyChanged(nameof(HasInvalidEntries));
                OnPropertyChanged(nameof(HasAnyDirty));
            }
        }

        #region Commands
        private RelayCommand _addCommand;
        public ICommand AddCommand => _addCommand ??= new RelayCommand(o =>
        {
            var addWindow = new AddStudentWindow { Owner = Application.Current?.MainWindow };
            var result = addWindow.ShowDialog();
            if (result == true)
            {
                Students.Insert(0, addWindow.Student);
                UpdateIndices();
                SelectedStudent = addWindow.Student;
                SaveData();
                OnPropertyChanged(nameof(HasInvalidEntries));
                OnPropertyChanged(nameof(HasAnyDirty));
            }
        });

        private RelayCommand _deleteSelectedCommand;
        public ICommand DeleteSelectedCommand => _deleteSelectedCommand ??= new RelayCommand(o =>
        {
            var toDelete = Students.Where(x => x.IsSelected).ToList();
            if (!toDelete.Any())
            {
                MessageBox.Show("Будь ласка, відмітьте хоча б одного студента для видалення.", "Видалення", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var msg = toDelete.Count == 1 ? $"Видалити студента {toDelete[0].FullName}?" : $"Видалити {toDelete.Count} студентів?";
            var res = MessageBox.Show(msg, "Підтвердження видалення", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (res == MessageBoxResult.Yes)
            {
                foreach (var s in toDelete) Students.Remove(s);
                UpdateIndices();
                SelectedStudent = Students.FirstOrDefault();
                SaveData();
                OnPropertyChanged(nameof(HasInvalidEntries));
                OnPropertyChanged(nameof(HasAnyDirty));
            }
        }, o => Students.Any(x => x.IsSelected));

        private RelayCommand _deleteSelectedSingleCommand;
        public ICommand DeleteSelectedSingleCommand => _deleteSelectedSingleCommand ??= new RelayCommand(o =>
        {
            if (SelectedStudent == null) return;
            var res = MessageBox.Show($"Видалити студента {SelectedStudent.FullName}?", "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (res == MessageBoxResult.Yes)
            {
                Students.Remove(SelectedStudent);
                UpdateIndices();
                SelectedStudent = Students.FirstOrDefault();
                SaveData();
                OnPropertyChanged(nameof(HasInvalidEntries));
                OnPropertyChanged(nameof(HasAnyDirty));
            }
        }, o => SelectedStudent != null);

        private RelayCommand _saveCommand;
        public ICommand SaveCommand => _saveCommand ??= new RelayCommand(o =>
        {
            foreach (var s in Students)
            {
                var errs = s.GetValidationErrors();
                if (errs.Any())
                {
                    var list = string.Join(Environment.NewLine, errs);
                    MessageBox.Show("Є помилки валiдацiї в даних:" + Environment.NewLine + list, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            SaveData();
            MessageBox.Show("Збережено.", "Збереження", MessageBoxButton.OK, MessageBoxImage.Information);
            OnPropertyChanged(nameof(HasInvalidEntries));
            OnPropertyChanged(nameof(HasAnyDirty));
        });
        #endregion

        #region IO helpers
        public void UpdateIndices()
        {
            int i = 1;
            foreach (var s in Students)
            {
                s.Index = i++;
            }
            OnPropertyChanged(nameof(Students));
        }

        public void SaveData()
        {
            try
            {
                var serializer = new XmlSerializer(typeof(Student[]), new XmlRootAttribute("Students"));
                using (var fs = new FileStream(DataFile, FileMode.Create))
                {
                    serializer.Serialize(fs, Students.Cast<Student>().ToArray());
                }

                foreach (var s in Students.Cast<Student>()) s.MarkClean();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при збереженні даних: " + ex.Message);
            }
        }

        private void LoadData()
        {
            try
            {
                if (File.Exists(DataFile))
                {
                    var serializer = new XmlSerializer(typeof(Student[]), new XmlRootAttribute("Students"));
                    using (var fs = new FileStream(DataFile, FileMode.Open))
                    {
                        var arr = (Student[])serializer.Deserialize(fs);
                        while (Students.Count > 0) Students.RemoveAt(Students.Count - 1);
                        foreach (var s in arr) Students.Add(s);
                    }
                }
                else
                {
                    Students.Add(new Student { FirstName = "Ольга", LastName = "Іваненко", Age = 20, Gender = "Ж" });
                    Students.Add(new Student { FirstName = "Петро", LastName = "Коваль", Age = 22, Gender = "Ч" });
                    SaveData();
                }
            }
            catch (Exception ex) { MessageBox.Show("Помилка при завантаженні даних: " + ex.Message); }

            foreach (var s in Students.Cast<Student>()) s.MarkClean();
            UpdateIndices();
            OnPropertyChanged(nameof(HasStudents));
            OnPropertyChanged(nameof(HasInvalidEntries));
            OnPropertyChanged(nameof(HasAnyDirty));
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string prop = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
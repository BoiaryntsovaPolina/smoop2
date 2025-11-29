using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Lab7_WpfBinding.ViewModels;

namespace Lab7_WpfBinding.Views
{
    /// <summary>
    /// Логика взаимодействия для CandidateEditWindow.xaml
    /// </summary>
    public partial class CandidateEditWindow : Window
    {
        public CandidateViewModel ViewModel { get; }
        public CandidateEditWindow(CandidateViewModel vm)
        {
            InitializeComponent();
            ViewModel = vm;
            DataContext = ViewModel;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Валідація: перевіряємо обов'язкові поля
            if (string.IsNullOrWhiteSpace(ViewModel.FullName)) { MessageBox.Show("Введіть П.І.П.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            if (ViewModel.BirthDate.HasValue && ViewModel.BirthDate.Value > System.DateTime.Now) { MessageBox.Show("Дата народження не може бути в майбутньому.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            if (ViewModel.ExperienceYears.HasValue && ViewModel.ExperienceYears < 0) { MessageBox.Show("Стаж не може бути від'ємним.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

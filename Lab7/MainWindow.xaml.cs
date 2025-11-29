using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lab7_WpfBinding.ViewModels;
using Lab7_WpfBinding.Views;


namespace Lab7_WpfBinding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel VM => DataContext as MainViewModel ?? throw new System.InvalidOperationException();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var vm = new ViewModels.CandidateViewModel();
            var win = new CandidateEditWindow(vm) { Owner = this };
            if (win.ShowDialog() == true)
            {
                VM.Candidates.Add(vm);
                VM.SelectedCandidate = vm;
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var selected = VM.SelectedCandidate;
            if (selected == null) { MessageBox.Show("Оберіть кандидата", "Увага", MessageBoxButton.OK, MessageBoxImage.Information); return; }
            var win = new CandidateEditWindow(selected) { Owner = this };
            win.ShowDialog();
            // зміни автоматично відобразяться через INotifyPropertyChanged
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var selected = VM.SelectedCandidate;
            if (selected == null) { MessageBox.Show("Оберіть кандидата", "Увага", MessageBoxButton.OK, MessageBoxImage.Information); return; }
            if (MessageBox.Show("Видалити кандидата?", "Підтвердження", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                VM.DeleteCommand.Execute(null);
            }
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            VM.FilterCommand.Execute(FilterBox.Text);
        }

        private void ClearFilter_Click(object sender, RoutedEventArgs e)
        {
            VM.ClearFilterCommand.Execute(null);
            FilterBox.Text = string.Empty;
        }
    }
}

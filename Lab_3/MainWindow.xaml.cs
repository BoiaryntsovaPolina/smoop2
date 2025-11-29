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

namespace Lab_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenWindow(Window w)
        {
            if (ChkModal.IsChecked == true)
            {
                // модально
                w.Owner = this;
                w.ShowDialog();
            }
            else
            {
                // немодально
                w.Owner = this;
                w.Show();
            }
        }

        private void BtnTask1_Click(object sender, RoutedEventArgs e)
        {
            OpenWindow(new Task1Window());
        }

        private void BtnTask2_Click(object sender, RoutedEventArgs e)
        {
            OpenWindow(new Task2Window());
        }

        private void BtnTask3_Click(object sender, RoutedEventArgs e)
        {
            OpenWindow(new Task3Window());
        }

        private void BtnTask4_Click(object sender, RoutedEventArgs e)
        {
            OpenWindow(new Task4Window());
        }

        private void BtnTask5_Click(object sender, RoutedEventArgs e)
        {
            OpenWindow(new Task5Window());
        }

        private void BtnTask6_Click(object sender, RoutedEventArgs e)
        {
            OpenWindow(new Task6Window());
        }
    }
}
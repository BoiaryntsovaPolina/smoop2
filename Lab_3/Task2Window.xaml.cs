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

namespace Lab_3
{
    /// <summary>
    /// Логика взаимодействия для Task2Window.xaml
    /// </summary>
    public partial class Task2Window : Window
    {
        public Task2Window()
        {
            InitializeComponent();
        }

        private void Hide_Click(object sender, RoutedEventArgs e)
        {
            MyTextBlock.Visibility = Visibility.Collapsed;
        }

        private void Show_Click(object sender, RoutedEventArgs e)
        {
            MyTextBlock.Visibility = Visibility.Visible;
        }
    }
}

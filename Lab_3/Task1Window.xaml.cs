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
    /// Логика взаимодействия для Task1Window.xaml
    /// </summary>
    public partial class Task1Window : Window
    {
        public Task1Window()
        {
            InitializeComponent();
        }

        private void HiButton_Click(object sender, RoutedEventArgs e)
        {
            MainLabel.Content = "Привіт";
        }

        private void ByeButton_Click(object sender, RoutedEventArgs e)
        {
            MainLabel.Content = "До побачення";
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Логика взаимодействия для Task4Window.xaml
    /// </summary>
    public partial class Task4Window : Window
    {
        private int hiddenButtons = 0;
        private bool gameCompleted = false;

        public Task4Window()
        {
            InitializeComponent();
            this.Closing += Task4Window_Closing; // додаємо перевірку при закритті
        }

        // --- Логіка гри ---
        private void B1_Click(object sender, RoutedEventArgs e)
        {
            B2.Visibility = Toggle(B2);
            B3.Visibility = Toggle(B3);
            CheckWin();
        }

        private void B2_Click(object sender, RoutedEventArgs e)
        {
            B4.Visibility = Toggle(B4);
            CheckWin();
        }

        private void B3_Click(object sender, RoutedEventArgs e)
        {
            B5.Visibility = Toggle(B5);
            CheckWin();
        }

        private void B4_Click(object sender, RoutedEventArgs e)
        {
            B1.Visibility = Toggle(B1);
            CheckWin();
        }

        private void B5_Click(object sender, RoutedEventArgs e)
        {
            // B5 ховає B2, B3, B4 і саму себе
            B2.Visibility = Visibility.Collapsed;
            B3.Visibility = Visibility.Collapsed;
            B4.Visibility = Visibility.Collapsed;
            B5.Visibility = Visibility.Collapsed; // <- додано
            CheckWin();
        }

        private Visibility Toggle(Button b)
        {
            return b.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void CheckWin()
        {
            var allButtons = new[] { B1, B2, B3, B4, B5 };
            if (allButtons.All(b => b.Visibility != Visibility.Visible))
            {
                gameCompleted = true;
                MessageBox.Show("Вітаю — ви приховали всі кнопки!", "Перемога",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            B1.Visibility = B2.Visibility = B3.Visibility = B4.Visibility = B5.Visibility = Visibility.Visible;
            gameCompleted = false;
        }

        // --- Подія закриття ---
        private void Task4Window_Closing(object sender, CancelEventArgs e)
        {
            if (!gameCompleted)
            {
                var result = MessageBox.Show("Завдання не завершено. Вийти в головне меню?",
                    "Підтвердження виходу",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
            else
            {
                var result = MessageBox.Show("Завдання завершено. Вийти в головне меню?",
                    "Повернення",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Information);

                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}

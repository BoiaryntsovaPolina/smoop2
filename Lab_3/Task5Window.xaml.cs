using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
    /// Логика взаимодействия для Task5Window.xaml
    /// </summary>
    public partial class Task5Window : Window
    {
        private bool _isCompleted = false; // прапорець — чи виконано завдання
        public Task5Window()
        {
            InitializeComponent();
            // Прив'язати обробник закриття
            this.Closing += Task5Window_Closing;
        }

        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            var s = PoundsBox.Text.Trim();
            if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double pounds))
            {
                double kg = pounds * 0.45359237; 
                ResultText.Text = $"Кілограмів: {kg:F4} кг";

                // Вважаємо, що завдання виконане
                _isCompleted = true;
            }
            else
            {
                MessageBox.Show("Введіть коректне числове значення у фунтах.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            PoundsBox.Clear();
            ResultText.Text = "Кілограмів: ";
        }

        // Обробник події Closing — спрацьовує при будь-якій спробі закрити вікно
        private void Task5Window_Closing(object sender, CancelEventArgs e)
        {
            if (!_isCompleted)
            {
                // Якщо ще не виконано — пропонуємо підтвердження
                var r = MessageBox.Show("Завдання не завершено. Вийти в головне меню?", "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (r == MessageBoxResult.No)
                {
                    e.Cancel = true; // відміняємо закриття
                }
            }
            else
            {
                // Якщо вже виконано, можна додатково запропонувати інший текст
                var r = MessageBox.Show("Завдання завершено. Вийти в головне меню?", "Завершення", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (r == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
                // якщо Yes — дозволяємо закриття
            }
        }
    }
}

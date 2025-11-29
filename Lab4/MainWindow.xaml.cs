using System.Globalization;
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

namespace Lab4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string currentNumber = "0";         // поточне число як рядок
        private double? accumulator = null;         // результат попередніх обчислень
        private string currentOperator = null;      // "+", "-", "*", "/"
        private bool justCalculated = false;        // флаг: після =

        public MainWindow()
        {
            InitializeComponent();
            UpdateDisplays();
        }

        // UI Helpers
        private void UpdateDisplays()
        {
            CurrentNumberTextBox.Text = currentNumber;
            if (accumulator.HasValue && !string.IsNullOrEmpty(currentOperator))
                PreviousExpressionTextBox.Text = $"{FormatNumber(accumulator.Value)} {currentOperator} {(justCalculated ? "" : "")}";
            else if (accumulator.HasValue)
                PreviousExpressionTextBox.Text = $"{FormatNumber(accumulator.Value)}";
            else
                PreviousExpressionTextBox.Text = string.Empty;
        }

        private string FormatNumber(double value)
        {
            // Виводимо число компактно: якщо ціле — без .0
            if (Math.Abs(value % 1) < 1e-12)
                return ((long)Math.Round(value)).ToString(CultureInfo.InvariantCulture);
            else
                return value.ToString("G12", CultureInfo.InvariantCulture);
        }

        // Button Handlers
        private void Digit_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button btn)) return;
            string digit = btn.Content.ToString();

            if (justCalculated)
            {
                // після обчислення, при введенні цифри — почати нове число
                currentNumber = "0";
                accumulator = null;
                currentOperator = null;
                justCalculated = false;
            }

            // Правила для ведучих нулів:
            // Якщо currentNumber == "0" (без крапки), і вводимо "0" -> залишаємо "0"
            // Якщо currentNumber == "0" і вводимо [1-9] -> замінюємо на ту цифру
            // Якщо currentNumber починається з "-" та далі тільки "0" -> аналогічна логіка
            if (currentNumber == "0")
            {
                if (digit == "0")
                {
                    // нічого не змінюємо
                }
                else
                {
                    currentNumber = digit;
                }
            }
            else if (currentNumber == "-0")
            {
                if (digit == "0") { /* нічого */ }
                else currentNumber = "-" + digit;
            }
            else
            {
                currentNumber += digit;
            }

            UpdateDisplays();
        }

        private void Dot_Click(object sender, RoutedEventArgs e)
        {
            if (justCalculated)
            {
                // починаємо нове число "0."
                currentNumber = "0";
                accumulator = null;
                currentOperator = null;
                justCalculated = false;
            }

            if (currentNumber.Contains("."))
                return; // тільки одна точка дозволена

            currentNumber += ".";
            UpdateDisplays();
        }

        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button btn)) return;
            string op = btn.Tag?.ToString();
            if (string.IsNullOrEmpty(op)) return;

            // При встановленні оператора: якщо є accumulator — робимо проміжне обчислення
            double parsed;
            if (!double.TryParse(currentNumber, NumberStyles.Number, CultureInfo.InvariantCulture, out parsed))
            {
                MessageBox.Show("Неправильне число.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (accumulator == null)
            {
                accumulator = parsed;
            }
            else
            {
                // якщо вже був оператор — обчислити accumulator (operator) parsed
                accumulator = Compute(accumulator.Value, parsed, currentOperator);
            }

            currentOperator = op;
            // підготовка для введення наступного числа
            currentNumber = "0";
            justCalculated = false;
            UpdateDisplays();
        }

        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            double parsed;
            if (!double.TryParse(currentNumber, NumberStyles.Number, CultureInfo.InvariantCulture, out parsed))
            {
                MessageBox.Show("Неправильне число.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (accumulator == null && currentOperator == null)
            {
                // просто показати число
                // нічого не робимо
            }
            else if (accumulator != null && !string.IsNullOrEmpty(currentOperator))
            {
                double result = Compute(accumulator.Value, parsed, currentOperator);
                // Відображення результату
                currentNumber = FormatNumber(result);
                accumulator = null;
                currentOperator = null;
                justCalculated = true;
            }
            UpdateDisplays();
        }

        private void ClearEntry_Click(object sender, RoutedEventArgs e)
        {
            currentNumber = "0";
            UpdateDisplays();
        }

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            currentNumber = "0";
            accumulator = null;
            currentOperator = null;
            justCalculated = false;
            UpdateDisplays();
        }

        private void Backspace_Click(object sender, RoutedEventArgs e)
        {
            if (justCalculated)
            {
                // якщо щойно був =, backspace -> очистити до "0"
                currentNumber = "0";
                justCalculated = false;
                UpdateDisplays();
                return;
            }

            if (currentNumber.Length <= 1 || (currentNumber.Length == 2 && currentNumber.StartsWith("-")))
            {
                // залишаємо "0"
                currentNumber = "0";
            }
            else
            {
                currentNumber = currentNumber.Substring(0, currentNumber.Length - 1);
                // Якщо після видалення залишилась тільки "-" -> ставимо "0"
                if (currentNumber == "-" || string.IsNullOrEmpty(currentNumber))
                    currentNumber = "0";
            }
            UpdateDisplays();
        }

        //Compute / Utils
        private double Compute(double left, double right, string op)
        {
            try
            {
                switch (op)
                {
                    case "+": return left + right;
                    case "-": return left - right;
                    case "*": return left * right;
                    case "/":
                        if (Math.Abs(right) < 1e-15)
                        {
                            MessageBox.Show("Ділення на нуль неможливе.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return left; // або можна повернути 0
                        }
                        return left / right;
                    default: return right;
                }
            }
            catch (OverflowException)
            {
                MessageBox.Show("Переповнення числового діапазону.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return 0;
            }
        }
    }
}
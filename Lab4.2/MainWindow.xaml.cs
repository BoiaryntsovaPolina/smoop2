using System.ComponentModel;
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

namespace Lab4._2_chernovic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private List<Fuel> _fuelTypes = new List<Fuel>();
        private List<CafeItem> _cafeItems = new List<CafeItem>();

        private bool _isTotalCalculated = false;
        private double _totalDailySum = 0;

        public MainWindow()
        {
            InitializeComponent();
            InitializeData();
            InitializeCafeUI();

            if (LitersRadioButton != null)
            {
                FuelOption_Checked(LitersRadioButton, new RoutedEventArgs());
            }
        }

        private void InitializeData()
        {
            _fuelTypes.Add(new Fuel { Name = "A-92", Price = 50.50 });
            _fuelTypes.Add(new Fuel { Name = "A-95", Price = 52.00 });
            _fuelTypes.Add(new Fuel { Name = "ДП", Price = 54.50 });

            _cafeItems.Add(new CafeItem { Name = "Хот-дог", Price = 45.00 });
            _cafeItems.Add(new CafeItem { Name = "Гамбургер", Price = 75.00 });
            _cafeItems.Add(new CafeItem { Name = "Картопля фрі", Price = 35.00 });
            _cafeItems.Add(new CafeItem { Name = "Кока-кола", Price = 25.00 });

            FuelComboBox.ItemsSource = _fuelTypes;
            FuelComboBox.SelectedIndex = 0;
        }

        private void InitializeCafeUI()
        {
            if (CafeItemsPanel == null) return;
            CafeItemsPanel.Children.Clear();

            foreach (var item in _cafeItems)
            {
                var itemGrid = new Grid { Margin = new Thickness(0, 5, 0, 5) };

                itemGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
                itemGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });
                itemGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                var cb = new CheckBox { Content = item.Name, VerticalAlignment = VerticalAlignment.Center };
                cb.Checked += CafeItem_Checked;
                cb.Unchecked += CafeItem_Checked;
                Grid.SetColumn(cb, 0);
                itemGrid.Children.Add(cb);
                item.CheckBox = cb;

                var priceBlock = new TextBlock { Text = $"{item.Price:F2} грн", VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(5, 0, 15, 0), HorizontalAlignment = HorizontalAlignment.Right };
                Grid.SetColumn(priceBlock, 1);
                itemGrid.Children.Add(priceBlock);

                var tb = new TextBox { Width = 50, Text = "0", IsEnabled = false, HorizontalContentAlignment = HorizontalAlignment.Center };
                tb.TextChanged += CafeItem_QuantityChanged;
                Grid.SetColumn(tb, 2);
                itemGrid.Children.Add(tb);
                item.QuantityTextBox = tb;

                CafeItemsPanel.Children.Add(itemGrid);
            }
        }

        // --- Обробники подій ---

        private void FuelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FuelComboBox.SelectedItem is Fuel selectedFuel)
            {
                PriceTextBlock.Text = selectedFuel.Price.ToString("F2");
                UpdateIntermediateTotals();
            }
        }

        private void SumTextBox_Enable_Disable(TextBox textBox, bool isEnabled)
        {
            if (textBox == null) return;

            textBox.IsEnabled = isEnabled;

            if (!isEnabled)
            {
                textBox.Text = "0";
                textBox.Background = Brushes.LightGray;
            }
            else
            {
                textBox.Background = Brushes.White;
            }
        }

        private void FuelOption_Checked(object sender, RoutedEventArgs e)
        {
            if (LitersRadioButton == null || AmountRadioButton == null ||
                LitersTextBox == null || AmountTextBox == null ||
                FuelTotalGroupBox == null || FuelTotalUnitTextBlock == null)
            {
                return;
            }

            if (LitersRadioButton.IsChecked == true)
            {
                SumTextBox_Enable_Disable(LitersTextBox, true);
                SumTextBox_Enable_Disable(AmountTextBox, false);

                FuelTotalGroupBox.Header = "До оплати (АЗС)";
                FuelTotalUnitTextBlock.Text = "грн";
            }
            else if (AmountRadioButton.IsChecked == true)
            {
                SumTextBox_Enable_Disable(LitersTextBox, false);
                SumTextBox_Enable_Disable(AmountTextBox, true);

                FuelTotalGroupBox.Header = "До видачі (АЗС)";
                FuelTotalUnitTextBlock.Text = "л";
            }

            UpdateIntermediateTotals();
        }

        private void FuelInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (double.TryParse(textBox.Text, out double value) && value >= 0)
                {
                    textBox.Background = Brushes.White;
                }
                else
                {
                    textBox.Background = Brushes.LightCoral;
                }
            }
            UpdateIntermediateTotals();
        }

        private void CafeItem_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox cb)
            {
                var item = _cafeItems.FirstOrDefault(i => i.CheckBox == cb);
                if (item != null)
                {
                    item.QuantityTextBox.IsEnabled = cb.IsChecked == true;
                    if (cb.IsChecked == false)
                    {
                        item.QuantityTextBox.Text = "0";
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(item.QuantityTextBox.Text) || item.QuantityTextBox.Text == "0")
                        {
                            item.QuantityTextBox.Text = "1";
                        }
                    }
                }
                UpdateIntermediateTotals();
            }
        }

        private void CafeItem_QuantityChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (int.TryParse(textBox.Text, out int qty) && qty >= 0)
                {
                    textBox.Background = Brushes.White;
                }
                else
                {
                    textBox.Background = Brushes.LightCoral;
                }
            }
            UpdateIntermediateTotals();
        }

        // --- Логіка розрахунків ---

        private void UpdateIntermediateTotals()
        {
            if (TotalSumTextBlock == null || TotalGroupLabel == null || TotalUnitTextBlock == null ||
                FuelComboBox == null || FuelTotalTextBlock == null || CafeTotalTextBlock == null ||
                LitersRadioButton == null || AmountRadioButton == null || LitersTextBox == null || AmountTextBox == null)
            {
                return;
            }

            TotalSumTextBlock.Text = "0.00";
            TotalSumTextBlock.Foreground = Brushes.Gray;
            TotalGroupLabel.Content = "Загальна сума:";
            TotalUnitTextBlock.Text = "грн";

            double fuelTotalValue = 0;
            double cafeTotalValue = 0;

            if (_cafeItems.Count > 0)
            {
                cafeTotalValue = _cafeItems
                   .Where(item => item.CheckBox != null && item.CheckBox.IsChecked == true)
                   .Sum(item => item.TotalPrice);
            }
            CafeTotalTextBlock.Text = cafeTotalValue.ToString("F2");

            if (FuelComboBox.SelectedItem is Fuel selectedFuel)
            {
                if (LitersRadioButton.IsChecked == true && double.TryParse(LitersTextBox.Text, out double liters) && liters >= 0)
                {
                    fuelTotalValue = liters * selectedFuel.Price;
                    FuelTotalTextBlock.Text = fuelTotalValue.ToString("F2");
                }
                else if (AmountRadioButton.IsChecked == true && double.TryParse(AmountTextBox.Text, out double amount) && amount >= 0)
                {
                    fuelTotalValue = amount / selectedFuel.Price;
                    FuelTotalTextBlock.Text = fuelTotalValue.ToString("F3");
                }
                else
                {
                    FuelTotalTextBlock.Text = "0.00";
                }
            }
            else
            {
                FuelTotalTextBlock.Text = "0.00";
            }
        }

        private void CalculateTotal_Click(object sender, RoutedEventArgs e)
        {
            if (TotalSumTextBlock == null || TotalUnitTextBlock == null || TotalGroupLabel == null ||
                FuelComboBox == null || LitersRadioButton == null || AmountRadioButton == null ||
                LitersTextBox == null || AmountTextBox == null)
            {
                return;
            }

            UpdateIntermediateTotals();

            double fuelFinalSum = 0;
            double cafeFinalSum = 0;

            cafeFinalSum = _cafeItems
                .Where(item => item.CheckBox != null && item.CheckBox.IsChecked == true)
                .Sum(item => item.TotalPrice);

            if (FuelComboBox.SelectedItem is Fuel selectedFuel)
            {
                if (LitersRadioButton.IsChecked == true && double.TryParse(LitersTextBox.Text, out double liters) && liters >= 0)
                {
                    fuelFinalSum = liters * selectedFuel.Price;
                    double totalSum = fuelFinalSum + cafeFinalSum;
                    TotalSumTextBlock.Text = totalSum.ToString("F2");
                }
                else if (AmountRadioButton.IsChecked == true && double.TryParse(AmountTextBox.Text, out double amount) && amount >= 0)
                {
                    double totalSum = amount + cafeFinalSum;
                    TotalSumTextBlock.Text = totalSum.ToString("F2");
                }
                else
                {
                    TotalSumTextBlock.Text = cafeFinalSum.ToString("F2");
                }
            }
            else
            {
                TotalSumTextBlock.Text = cafeFinalSum.ToString("F2");
            }

            TotalSumTextBlock.Foreground = Brushes.Red;
            TotalGroupLabel.Content = "Загальна сума:";
            TotalUnitTextBlock.Text = "грн";
            _isTotalCalculated = true;
        }

        // --- Логіка перевірки та обнулення ---

        private double GetCurrentUncalculatedTotal()
        {
            double fuelSum = 0;
            double cafeSum = 0;

            // 1. Сума за Кафе
            if (_cafeItems.Count > 0)
            {
                cafeSum = _cafeItems
                   .Where(item => item.CheckBox != null && item.CheckBox.IsChecked == true)
                   .Sum(item => item.TotalPrice);
            }

            // 2. Сума за Пальне
            if (FuelComboBox != null && FuelComboBox.SelectedItem is Fuel selectedFuel)
            {
                if (LitersRadioButton.IsChecked == true && double.TryParse(LitersTextBox.Text, out double liters) && liters > 0)
                {
                    fuelSum = liters * selectedFuel.Price;
                }
                else if (AmountRadioButton.IsChecked == true && double.TryParse(AmountTextBox.Text, out double amount) && amount > 0)
                {
                    fuelSum = amount;
                }
            }

            return fuelSum + cafeSum;
        }

        // Кнопка "Новий клієнт / Обнулити"
        private void ResetAll_Click(object sender, RoutedEventArgs e)
        {
            double currentTotal = GetCurrentUncalculatedTotal();

            // 1. ПЕРЕВІРКА: чи є сума > 0, але вона не порахована
            if (currentTotal > 0 && !_isTotalCalculated)
            {
                MessageBox.Show("Не порахована загальна сума для поточного клієнта. Будь ласка, натисніть 'Обчислити загальну суму'.", "Помилка обнулення", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. ЗБЕРІГАННЯ: якщо сума порахована, додаємо її до денного підсумку
            if (_isTotalCalculated && double.TryParse(TotalSumTextBlock.Text, out double finalCalculatedTotal))
            {
                _totalDailySum += finalCalculatedTotal;
            }

            // 3. ОБНУЛЕННЯ:

            if (LitersRadioButton != null) LitersRadioButton.IsChecked = true;
            if (LitersTextBox != null) LitersTextBox.Text = "0";
            if (AmountTextBox != null) AmountTextBox.Text = "0";
            if (FuelComboBox != null) FuelComboBox.SelectedIndex = 0;

            foreach (var item in _cafeItems)
            {
                if (item.CheckBox != null) item.CheckBox.IsChecked = false;
                if (item.QuantityTextBox != null) item.QuantityTextBox.Text = "0";
            }

            _isTotalCalculated = false;

            UpdateIntermediateTotals();
            MessageBox.Show("Дані для нового клієнта очищені. Сума збережена.", "Обнулення", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // НОВИЙ МЕТОД: Обробка закриття вікна (натискання на хрестик)
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            double currentTotal = GetCurrentUncalculatedTotal();

            // 1. ПЕРЕВІРКА: якщо є сума > 0, але вона не порахована
            if (currentTotal > 0 && !_isTotalCalculated)
            {
                MessageBox.Show("Не порахована загальна сума для поточного клієнта. Розрахуйте суму або обнуліть дані.", "Помилка виходу", MessageBoxButton.OK, MessageBoxImage.Warning);
                e.Cancel = true; // Скасувати закриття вікна
                return;
            }

            // 2. ЗБЕРІГАННЯ: якщо сума порахована, додаємо її до денного підсумку 
            if (_isTotalCalculated && double.TryParse(TotalSumTextBlock.Text, out double finalCalculatedTotal))
            {
                _totalDailySum += finalCalculatedTotal;
            }

            // 3. ВИХІД: Виводимо підсумок. Закриття вікна відбудеться після натискання OK.
            MessageBox.Show($"Загальна сума за день: {_totalDailySum:F2} грн", "Підсумок дня", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
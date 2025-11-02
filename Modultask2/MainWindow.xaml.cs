using System.IO;
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

namespace Modultask2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Worker> _workers = new List<Worker>();
        private List<SalaryRecord> _salaries = new List<SalaryRecord>();
        private readonly string _workersPath = System.IO.Path.Combine("Data", "Workers.txt");
        private readonly string _salariesPath = System.IO.Path.Combine("Data", "Salaries.txt");

        public MainWindow()
        {
            InitializeComponent();
            AddMessage("Готово. Натисніть 'Згенерувати' або 'З файлів'.");
        }

        // Button 

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            ClearMessages();
            var sample = JsonDataService.GenerateSampleData();
            _workers = sample.workers;
            _salaries = sample.salaries;

            try
            {
                JsonDataService.SaveToTxt(_workersPath, _workers);
                JsonDataService.SaveToTxt(_salariesPath, _salaries);
                AddMessage("Згенеровано та збережено приклади.");
            }
            catch (Exception ex)
            {
                AddMessage("Помилка збереження: " + ex.Message);
            }
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            ClearMessages();
            try
            {
                _workers = JsonDataService.LoadFromTxt<Worker>(_workersPath);
                _salaries = JsonDataService.LoadFromTxt<SalaryRecord>(_salariesPath);
                AddMessage($"Завантажено: {_workers.Count} працівників, {_salaries.Count} зарплат.");
            }
            catch (Exception ex)
            {
                AddMessage("Помилка завантаження: " + ex.Message);
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            ClearMessages();
            try
            {
                JsonDataService.SaveToTxt(_workersPath, _workers);
                JsonDataService.SaveToTxt(_salariesPath, _salaries);
                AddMessage("Дані збережено.");
            }
            catch (Exception ex)
            {
                AddMessage("Помилка збереження: " + ex.Message);
            }
        }

        private void BtnRun_Click(object sender, RoutedEventArgs e)
        {
            ClearMessages();

            if (!ValidateThreshold(out decimal threshold))
            {
                TxtResults.Clear();
                return; // некоректне значення, запити не виконуються
            }

            ExecuteLinqQueries(threshold);
        }

        // Перевірка порогу
        private bool ValidateThreshold(out decimal threshold)
        {
            threshold = 0;

            bool isValid = decimal.TryParse(TxtThreshold.Text, out threshold);

            if (!isValid || threshold < 1)
            {
                TxtThreshold.Background = System.Windows.Media.Brushes.LightCoral;
                AddMessage("Помилка: введіть додатнє число більше або рівне 1 для порогу зарплати.");
                return false;
            }
            else
            {
                TxtThreshold.Background = System.Windows.Media.Brushes.White;
                return true;
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        // UI helpers

        private void AddMessage(string msg)
        {
            ErrorsList.Items.Add(msg);
        }

        private void ClearMessages()
        {
            ErrorsList.Items.Clear();
            TxtResults.Clear();
        }

        // LINQ queries

        private void ExecuteLinqQueries(decimal threshold)
        {
            if (_workers.Count == 0 || _salaries.Count == 0)
            {
                AddMessage("Немає даних для обробки.");
                return;
            }

            int now = DateTime.Now.Year;

            TxtResults.AppendText("1) Працівники старші за 35 років:\n");
            var olderThan35 =
                from w in _workers
                where (w.GetAge() is int age && age > 35)
                select w.ToString();
            foreach (var line in olderThan35)
                TxtResults.AppendText(" - " + line + "\n");

            TxtResults.AppendText("\n2) Працівник з найбільшою зарплатою за 2-ге півріччя:\n");
            var maxSecond = (from s in _salaries orderby s.SecondHalf descending select s).FirstOrDefault();
            if (maxSecond != null)
            {
                var w = _workers.FirstOrDefault(x => x.Id == maxSecond.Id);
                if (w != null)
                    TxtResults.AppendText($" - {w.Id} : {w.Specialty}\n");
            }

            TxtResults.AppendText("\n3) Працівники, чия річна зарплата нижча за середню:\n");
            var avgAnnual = _salaries.Average(s => s.Annual);
            var belowAvg =
                from w in _workers
                join s in _salaries on w.Id equals s.Id
                where s.Annual < avgAnnual
                select $"{w.FullName} — Освіта: {w.Education} — Річна: {s.Annual:F2}";
            foreach (var line in belowAvg)
                TxtResults.AppendText(" - " + line + "\n");

            TxtResults.AppendText("\n4) Працівники з вищою освітою та зарплатою не менше порогу:\n");
            var qualified =
                from w in _workers
                join s in _salaries on w.Id equals s.Id
                where string.Equals(w.Education, "Вища", StringComparison.OrdinalIgnoreCase)
                      && s.Annual >= threshold
                select $"{w.FullName} — Річна: {s.Annual:F2}";

            foreach (var line in qualified)
                TxtResults.AppendText(" - " + line + "\n");
        }
    }
}
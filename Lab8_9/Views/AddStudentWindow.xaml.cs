using Lab8.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Lab8.Helpers;

namespace Lab8.Views
{
    public partial class AddStudentWindow : Window
    {
        public Student Student { get; }

        public AddStudentWindow()
        {
            InitializeComponent();
            Student = new Student();
            DataContext = Student;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            var errs = Student.GetValidationErrors().ToList();
            if (errs.Any())
            {
                MessageBox.Show("Будь ласка виправте помилки перед додаванням:\n\n" + string.Join("\n", errs.Distinct()), "Невірні дані", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void NumberOnly_PreviewTextInput(object sender, TextCompositionEventArgs e) => e.Handled = InputValidationHelpers.NonDigitRegex.IsMatch(e.Text);

        private void NumberOnly_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                var pastedText = e.DataObject.GetData(DataFormats.Text) as string ?? "";
                if (InputValidationHelpers.NonDigitRegex.IsMatch(pastedText)) e.CancelCommand();
            }
            else e.CancelCommand();
        }

        private void Name_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == "-")
            {
                if (sender is TextBox tb)
                {
                    int pos = tb.CaretIndex;
                    if (pos == 0) { e.Handled = true; return; }
                    char prev = (pos - 1) >= 0 && pos - 1 < tb.Text.Length ? tb.Text[pos - 1] : '\0';
                    if (!char.IsLetter(prev)) { e.Handled = true; return; }
                    if (prev == '\'' || prev == '-') { e.Handled = true; return; }
                    e.Handled = false;
                    return;
                }
            }

            e.Handled = InputValidationHelpers.ForbiddenInName.IsMatch(e.Text);
        }

        private void Name_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                var pastedText = e.DataObject.GetData(DataFormats.Text) as string ?? "";
                if (string.IsNullOrWhiteSpace(pastedText) || InputValidationHelpers.ForbiddenInName.IsMatch(pastedText)) { e.CancelCommand(); return; }
                if (!InputValidationHelpers.IsHyphenPlacementValid(pastedText)) { e.CancelCommand(); return; }
            }
            else e.CancelCommand();
        }

        private void Gender_PreviewTextInput(object sender, TextCompositionEventArgs e) => e.Handled = !InputValidationHelpers.GenderSingleRegex.IsMatch(e.Text);

        private void Gender_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                var pastedText = e.DataObject.GetData(DataFormats.Text) as string ?? "";
                if (!InputValidationHelpers.GenderSingleRegex.IsMatch(pastedText)) e.CancelCommand();
            }
            else e.CancelCommand();
        }
    }
}

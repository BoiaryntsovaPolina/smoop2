using Lab8.Models;
using Lab8.ViewModels;
using Lab8.Helpers;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Lab8.Views
{
    public partial class MainWindow : Window
    {
        private StudentsViewModel _vm;

        public MainWindow()
        {
            InitializeComponent();
            var container = new StudentCollection();
            _vm = new StudentsViewModel(container);
            this.DataContext = _vm;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (_vm.SelectedStudent != null)
            {
                // Перевіряємо активне поле перед закриттям
                bool hasInvalidData = CheckActiveFieldForInvalidData();
                if (hasInvalidData)
                {
                    var result = MessageBox.Show(
                        "Зміни не будуть збережені. Було введено неправильні дані.\nПовернутися до редагування?",
                        "Невірні дані",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        e.Cancel = true; // скасовуємо закриття
                        return;
                    }
                }
            }

            // Зберігаємо файли тільки валідні дані
            _vm.SaveData();
            MessageBox.Show("Дані збережено", "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);

            base.OnClosing(e);
        }

        // Метод перевіряє активне поле і відкочує невалідні дані
        private bool CheckActiveFieldForInvalidData()
        {
            if (Keyboard.FocusedElement is TextBox tb && _vm.SelectedStudent != null)
            {
                string prev = tb.Tag as string ?? "";
                string current = tb.Text ?? "";

                if (prev != current)
                {
                    string propertyName = tb.Name switch
                    {
                        "FirstNameTextBox" => nameof(Student.FirstName),
                        "LastNameTextBox" => nameof(Student.LastName),
                        "GenderTextBox" => nameof(Student.Gender),
                        "AgeTextBox" => nameof(Student.AgeText),
                        _ => null
                    };

                    if (propertyName != null)
                    {
                        var err = _vm.SelectedStudent[propertyName];
                        if (!string.IsNullOrEmpty(err))
                        {
                            // Відкочуємо дані
                            tb.Text = prev;
                            _vm.SelectedStudent.IsDirty = false;

                            // Ставимо фокус на поле
                            tb.Focus();
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        // Збереження початкового тексту при вході в поле
        private void Field_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                tb.Tag = tb.Text ?? "";
            }
        }

        // Обробники втрати фокусу для кожного поля
        private void FirstName_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            HandleLostFocus(sender, nameof(Student.FirstName));
        }

        private void LastName_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            HandleLostFocus(sender, nameof(Student.LastName));
        }

        private void Gender_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            HandleLostFocus(sender, nameof(Student.Gender));
        }

        private void AgeText_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            HandleLostFocus(sender, nameof(Student.AgeText));
        }

        // Загальний метод для перевірки поля і відкочування
        private void HandleLostFocus(object sender, string propertyName)
        {
            if (_vm.SelectedStudent == null) return;

            var tb = sender as TextBox;
            if (tb == null) return;

            var prev = tb.Tag as string ?? "";
            var current = tb.Text ?? "";

            if (prev == current) return;

            var err = _vm.SelectedStudent[propertyName];
            if (!string.IsNullOrEmpty(err))
            {
                // Відкочуємо дані
                tb.Text = prev;
                _vm.SelectedStudent.IsDirty = false;
                tb.Focus();
                MessageBox.Show("Зміни не відбулися. Було введено неправильні дані.",
                    "Невірні дані", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                // Успішне редагування
                _vm.UpdateIndices();
                _vm.SaveData();
            }
        }

        // Обробники вводу (цифри, букви, гендер)
        private void NumberOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = InputValidationHelpers.NonDigitRegex.IsMatch(e.Text);
        }

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
                    if (!char.IsLetter(prev) || prev == '\'' || prev == '-') { e.Handled = true; return; }
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
                if (string.IsNullOrWhiteSpace(pastedText) || InputValidationHelpers.ForbiddenInName.IsMatch(pastedText)
                    || !InputValidationHelpers.IsHyphenPlacementValid(pastedText))
                {
                    e.CancelCommand();
                }
            }
            else e.CancelCommand();
        }

        private void Gender_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !InputValidationHelpers.GenderSingleRegex.IsMatch(e.Text);
        }

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

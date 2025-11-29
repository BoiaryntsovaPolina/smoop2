using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Lab8.Helpers;
using System.Linq;

namespace Lab8.Models
{
    public class Student : INotifyPropertyChanged, IDataErrorInfo
    {
        private string _firstName;
        private string _lastName;
        private int _age;
        private string _ageText;
        private string _gender;
        private bool _isSelected;
        // runtime-only index (для відображення порядкового номера)
        [XmlIgnore]
        public int Index { get; set; }

        // Нове: прапорець "змінено з моменту останнього збереження"
        private bool _isDirty;
        [XmlIgnore]
        public bool IsDirty
        {
            get => _isDirty;
            set
            {
                if (_isDirty == value) return;
                _isDirty = value;
                OnPropertyChanged();
            }
        }

        // Регекси
        private static readonly Regex NameRegex = new Regex(@"^[\p{L}\p{M}'\-\s]+$", RegexOptions.Compiled);
        private static readonly Regex UkrainianVowels = new Regex("[аеєиіїоуюяАЕЄИІЇОУЮЯ]", RegexOptions.Compiled);

        [XmlElement] public string FirstName { get => _firstName; set { if (_firstName == value) return; _firstName = value; MarkDirty(); OnPropertyChanged(); OnPropertyChanged(nameof(FullName)); } }
        [XmlElement] public string LastName { get => _lastName; set { if (_lastName == value) return; _lastName = value; MarkDirty(); OnPropertyChanged(); OnPropertyChanged(nameof(FullName)); } }
        [XmlElement] public int Age { get => _age; set { if (_age == value) return; _age = value; _ageText = _age.ToString(); MarkDirty(); OnPropertyChanged(); OnPropertyChanged(nameof(AgeText)); } }

        [XmlIgnore]
        public string AgeText
        {
            get => _ageText;
            set
            {
                if (_ageText == value) return;
                _ageText = value;
                // лише при успішному парсингу змінюємо Age (Age встановить MarkDirty)
                if (int.TryParse(_ageText, out int parsed)) Age = parsed;
                else OnPropertyChanged(nameof(AgeText));
                OnPropertyChanged(nameof(Age));
            }
        }
        [XmlElement] public string Gender { get => _gender; set { if (_gender == value) return; _gender = value; MarkDirty(); OnPropertyChanged(); } }
        [XmlIgnore] public bool IsSelected { get => _isSelected; set { _isSelected = value; OnPropertyChanged(); } }
        [XmlIgnore] public string FullName => $"{LastName} {FirstName}".Trim();

        public Student() { _firstName = ""; _lastName = ""; _gender = ""; _age = 16; _ageText = _age.ToString(); _isDirty = false; }

        // Викликати після завантаження/успішного збереження, щоб позначити як "чистий"
        public void MarkClean() => IsDirty = false;

        // Внутрішній хелпер: помічати як "брудний" при редагуванні
        private void MarkDirty()
        {
            // Помічати як "брудний" лише якщо уже було встановлено хоч щось (тобто не під час десеріалізації, але
            // десеріалізація в нашому випадку прохідною буде — ми явно скинемо прапор в StudentsViewModel.LoadData()).
            IsDirty = true;
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(FirstName):
                        if (string.IsNullOrWhiteSpace(FirstName)) return "Ім'я обов'язкове.";
                        var fn = FirstName.Trim();
                        if (fn.Length < 2) return "Ім'я має містити щонайменше 2 символи.";
                        if (fn.Length > 50) return "Ім'я занадто довге.";
                        if (!NameRegex.IsMatch(fn)) return "Ім'я має містити лише літери, пробіли, дефіси або апостроф.";
                        if (!InputValidationHelpers.IsHyphenPlacementValid(fn)) return "Дефіс може бути лише всередині слова (не на початку/кінці, не поруч з пробілом, не подвійний).";
                        if (!UkrainianVowels.IsMatch(fn)) return "Ім'я має містити щонайменше одну голосну (українську).";
                        break;
                    case nameof(LastName):
                        if (string.IsNullOrWhiteSpace(LastName)) return "Прізвище обов'язкове.";
                        var ln = LastName.Trim();
                        if (ln.Length < 2) return "Прізвище має містити щонайменше 2 символи.";
                        if (ln.Length > 50) return "Прізвище занадто довге.";
                        if (!NameRegex.IsMatch(ln)) return "Прізвище має містити лише літери, пробіли, дефіси або апостроф.";
                        if (!InputValidationHelpers.IsHyphenPlacementValid(ln)) return "Дефіс може бути лише всередині слова (не на початку/кінці, не поруч з пробілом, не подвійний).";
                        if (!UkrainianVowels.IsMatch(ln)) return "Прізвище має містити щонайменше одну голосну (українську).";
                        break;
                    case nameof(Gender):
                        if (string.IsNullOrWhiteSpace(Gender)) return "Стать обов'язкова (Ч або Ж).";
                        var g = Gender.Trim().ToUpper();
                        if (g != "Ч" && g != "Ж") return "Стать має бути: Ч або Ж.";
                        break;
                    case nameof(AgeText):
                        if (string.IsNullOrWhiteSpace(AgeText)) return "Вік обов'язковий.";
                        if (!int.TryParse(AgeText, out int parsed)) return "Вік має бути числом.";
                        if (parsed < 16 || parsed > 100) return "Вік повинен бути в діапазоні [16, 100].";
                        break;
                    case nameof(Age):
                        if (Age < 16 || Age > 100) return "Вік повинен бути в діапазоні [16, 100].";
                        break;
                }
                return null;
            }
        }
        public IEnumerable<string> GetValidationErrors()
        {
            var props = new[] { nameof(FirstName), nameof(LastName), nameof(Gender), nameof(AgeText) };
            foreach (var p in props)
            {
                var err = this[p];
                if (!string.IsNullOrEmpty(err)) yield return err;
            }
        }

        [XmlIgnore]
        public bool IsValid => !GetValidationErrors().Any();

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
            // завжди оновлюємо IsValid після зміни будь-якої властивості (щоб UI оновлював позначку)
            if (propName != nameof(IsValid))
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid)));
        }
    }
}


// 8.9 кінцевий результат (пропали вікна при закритті)!!!!!
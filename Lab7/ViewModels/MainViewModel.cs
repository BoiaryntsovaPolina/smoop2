using Lab7_WpfBinding.Helpers;
using Lab7_WpfBinding.Models;
using Lab7_WpfBinding.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Lab7_WpfBinding.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly CandidateService _service = new();

        public ObservableCollection<CandidateViewModel> Candidates { get; } = new();

        private CandidateViewModel? _selectedCandidate;
        public CandidateViewModel? SelectedCandidate
        {
            get => _selectedCandidate;
            set { if (_selectedCandidate != value) { _selectedCandidate = value; OnPropertyChanged(nameof(SelectedCandidate)); DeleteCommand.RaiseCanExecuteChanged(); } }
        }

        public MainViewModel()
        {
            // Приклади
            var c1 = new Candidate { FullName = "Іваненко І. І.", BirthDate = DateTime.Now.AddYears(-30), Education = "Вища", Languages = "English:Fluent", HasComputerSkills = true, ExperienceYears = 5, HasRecommendations = true };
            var c2 = new Candidate { FullName = "Петренко П. П.", BirthDate = DateTime.Now.AddYears(-25), Education = "Середня спеціальна", Languages = "German:Read", HasComputerSkills = false, ExperienceYears = 2, HasRecommendations = false };
            _service.Add(c1); _service.Add(c2);

            foreach (var c in _service.GetAll()) Candidates.Add(new CandidateViewModel(c));

            AddCommand = new RelayCommand(_ => AddCandidate());
            DeleteCommand = new RelayCommand(_ => DeleteSelected(), _ => SelectedCandidate != null);
            FilterCommand = new RelayCommand(param => ApplyFilter(param as string));
            ClearFilterCommand = new RelayCommand(_ => ClearFilter());
        }

        private ObservableCollection<CandidateViewModel>? _backup;

        public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(name));

        public RelayCommand AddCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand FilterCommand { get; }
        public RelayCommand ClearFilterCommand { get; }

        private void AddCandidate()
        {
            var vm = new CandidateViewModel();
            Candidates.Add(vm);
            _service.Add(vm.ToModel());
            SelectedCandidate = vm;
        }

        private void DeleteSelected()
        {
            if (SelectedCandidate == null) return;
            int idx = Candidates.IndexOf(SelectedCandidate);
            if (idx >= 0)
            {
                Candidates.RemoveAt(idx);
                _service.RemoveAt(idx);
                SelectedCandidate = Candidates.FirstOrDefault();
            }
        }

        private void ApplyFilter(string? text)
        {
            if (string.IsNullOrWhiteSpace(text)) return;
            if (_backup == null) _backup = new ObservableCollection<CandidateViewModel>(Candidates);
            var filtered = _backup.Where(c => (!string.IsNullOrEmpty(c.FullName) && c.FullName.Contains(text, StringComparison.OrdinalIgnoreCase))
                                            || (!string.IsNullOrEmpty(c.Education) && c.Education.Contains(text, StringComparison.OrdinalIgnoreCase))
                                            || (!string.IsNullOrEmpty(c.Languages) && c.Languages.Contains(text, StringComparison.OrdinalIgnoreCase))).ToList();
            Candidates.Clear();
            foreach (var f in filtered) Candidates.Add(f);
            SelectedCandidate = Candidates.FirstOrDefault();
        }

        private void ClearFilter()
        {
            if (_backup == null) return;
            Candidates.Clear();
            foreach (var c in _backup) Candidates.Add(c);
            _backup = null;
            SelectedCandidate = Candidates.FirstOrDefault();
        }
    }
}

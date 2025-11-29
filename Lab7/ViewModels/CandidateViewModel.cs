using Lab7_WpfBinding.Models;
using System;
using System.ComponentModel;

namespace Lab7_WpfBinding.ViewModels
{
    public class CandidateViewModel : INotifyPropertyChanged
    {
        private Candidate _model;

        public CandidateViewModel()
        {
            _model = new Candidate
            {
                BirthDate = null,
                Education = string.Empty,
                Languages = string.Empty,
                HasComputerSkills = null,
                ExperienceYears = null,
                HasRecommendations = null,
                FullName = string.Empty
            };
        }

        public CandidateViewModel(Candidate model)
        {
            _model = model ?? new Candidate();
        }

        public Candidate ToModel() => _model;

        public string FullName
        {
            get => _model.FullName;
            set { if (_model.FullName != value) { _model.FullName = value; OnPropertyChanged(nameof(FullName)); } }
        }

        public DateTime? BirthDate
        {
            get => _model.BirthDate;
            set { if (_model.BirthDate != value) { _model.BirthDate = value; OnPropertyChanged(nameof(BirthDate)); } }
        }

        public string Education
        {
            get => _model.Education;
            set { if (_model.Education != value) { _model.Education = value; OnPropertyChanged(nameof(Education)); } }
        }

        public string Languages
        {
            get => _model.Languages;
            set { if (_model.Languages != value) { _model.Languages = value; OnPropertyChanged(nameof(Languages)); } }
        }

        public bool? HasComputerSkills
        {
            get => _model.HasComputerSkills;
            set { if (_model.HasComputerSkills != value) { _model.HasComputerSkills = value; OnPropertyChanged(nameof(HasComputerSkills)); } }
        }

        public int? ExperienceYears
        {
            get => _model.ExperienceYears;
            set { if (_model.ExperienceYears != value) { _model.ExperienceYears = value; OnPropertyChanged(nameof(ExperienceYears)); } }
        }

        public bool? HasRecommendations
        {
            get => _model.HasRecommendations;
            set { if (_model.HasRecommendations != value) { _model.HasRecommendations = value; OnPropertyChanged(nameof(HasRecommendations)); } }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

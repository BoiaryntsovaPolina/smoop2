using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab7_WpfBinding.Models
{
    public class Candidate
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; } // може бути не вказана
        public string Education { get; set; } = string.Empty;
        public string Languages { get; set; } = string.Empty; // CSV формат "English:Fluent;German:Read"
        public bool? HasComputerSkills { get; set; } // може бути null (не вказано)
        public int? ExperienceYears { get; set; } // може бути null
        public bool? HasRecommendations { get; set; } // може бути null
    }
}

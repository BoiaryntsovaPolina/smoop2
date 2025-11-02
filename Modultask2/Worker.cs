using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modultask2
{
    public class Worker
    {
        public string? Id { get; set; }
        public string? FullName { get; set; }
        public string? Education { get; set; }
        public string? Specialty { get; set; }
        public int BirthYear { get; set; }

        public Worker() { }

        public Worker(string? id, string? fullName, string? education, string? specialty, int birthYear)
        {
            Id = id;
            FullName = fullName;
            Education = education;
            Specialty = specialty;
            BirthYear = birthYear;
        }
        public int? GetAge()
        {
            if (BirthYear <= 0) return null;
            var yearNow = DateTime.Now.Year;
            if (BirthYear > yearNow) return null;
            return yearNow - BirthYear;
        }

        public override string ToString()
        {
            return $"{FullName} ({Specialty}), Освіта: {Education}, Рік народження: {BirthYear}";
        }

        // об'єкти вважаються однаковими, якщо однаковий Id
        public override bool Equals(object? obj)
        {
            if (obj is Worker other)
            {
                return string.Equals(Id, other.Id, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id?.ToLower().GetHashCode() ?? 0;
        }
    }
}

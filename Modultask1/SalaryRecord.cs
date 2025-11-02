using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modultask1
{
    internal class SalaryRecord
    {
        public string? Id { get; set; }
        public decimal FirstHalf { get; set; }
        public decimal SecondHalf { get; set; }

        public SalaryRecord()
        {
        }

        public SalaryRecord(string? id, decimal firstHalf, decimal secondHalf)
        {
            Id = id;
            FirstHalf = firstHalf;
            SecondHalf = secondHalf;
        }

        // Річна зарплата
        public decimal Annual => FirstHalf + SecondHalf;

        public override string ToString()
        {
            return $"ID: {Id}, 1 півріччя: {FirstHalf}, 2 півріччя: {SecondHalf}, Разом: {Annual}";
        }

        public override bool Equals(object? obj)
        {
            if (obj is SalaryRecord other)
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

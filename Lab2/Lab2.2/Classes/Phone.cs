using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2._2.Classes
{
    public record Phone(string Name, string Manufacturer, decimal Price, DateTime ReleaseDate)
    {
        public override string ToString()
        {
            return $"{Name} ({Manufacturer}) - {Price:C} - {ReleaseDate:yyyy-MM-dd}";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2._2.Classes
{
    public class ManufacturerStat
    {
        public string Manufacturer { get; set; }
        public int Count { get; set; }

        public ManufacturerStat() { }

        public ManufacturerStat(string manufacturer, int count)
        {
            Manufacturer = manufacturer;
            Count = count;
        }

        public override string ToString()
        {
            return $"{Manufacturer}: {Count}";
        }
    }

    public class ModelStat
    {
        public string Model { get; set; }
        public int Count { get; set; }

        public ModelStat() { }

        public ModelStat(string model, int count)
        {
            Model = model;
            Count = count;
        }

        public override string ToString()
        {
            return $"{Model}: {Count}";
        }
    }

    public class YearStat
    {
        public int Year { get; set; }
        public int Count { get; set; }

        public YearStat() { }

        public YearStat(int year, int count)
        {
            Year = year;
            Count = count;
        }

        public override string ToString()
        {
            return $"{Year}: {Count}";
        }
    }
}

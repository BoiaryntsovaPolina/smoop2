using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4._2_chernovic
{
    public class Fuel
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public override string ToString() => Name;
    }
}

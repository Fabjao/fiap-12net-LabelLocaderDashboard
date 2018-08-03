using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Dashboard.Contract.Models
{
    public class Restriction
    {
        public string Type { get; set; }
        public int Users { get; set; }

        public Restriction()
        { }
    }
}

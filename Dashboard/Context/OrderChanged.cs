using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Context
{
    public class OrderChanged
    {
        public int OrderId { get; set; }
        public string State { get; set; }

        public double Value { get; set; }
        public string StoredId { get; set; }
    }
}

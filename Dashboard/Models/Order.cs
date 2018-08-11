using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Models
{
    public class Order
    {
        public Order() { }

        public int OrderId { get; set; }
        public int StoreId { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }

    }
}

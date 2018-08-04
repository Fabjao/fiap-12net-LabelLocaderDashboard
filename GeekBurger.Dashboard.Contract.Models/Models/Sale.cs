using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Dashboard.Contract.Models
{
    public class Sale
    {
        public int StoreId { get; set; }

        public int Total { get; set; }

        public decimal Value { get; set; }
    }
}

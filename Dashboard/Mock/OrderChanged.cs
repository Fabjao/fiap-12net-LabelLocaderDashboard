using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Mock
{
    public class OrderChanged
    {
        public OrderChanged() { }

        public int OrderId { get; set; }
        public int StoreId { get; set; }
        public GeekBurger.Orders.Contract.Enums.OrderState State { get; set; }
        public decimal Value { get; set; }

    }
}

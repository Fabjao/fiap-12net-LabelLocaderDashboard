using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Dashboard.Contract.Models
{
    public class User
    {
        public int Users { get; set; }

        public ICollection<Restriction> Restrictions { get; set; }

        public int Usage { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Models
{
    public class FoodRestriction
    {
        public FoodRestriction()
        { }

        public int ID { get; set; }
        public string Restrictions { get; set; }
        public int UserId { get; set; }
    }
}

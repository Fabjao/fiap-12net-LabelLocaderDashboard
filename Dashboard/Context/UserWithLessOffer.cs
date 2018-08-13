using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Context
{
    public class UserWithLessOffer
    {
        [Key]
        public int UserId { get; set; }
        public string restrictions { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.LabelLoader.Contract.Models
{
    public class Produto
    {
        public string ItemName { get; set; }
        public List<string> Ingredients { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using GeekBurger.Dashboard.Contract.Models;

namespace Dashboard.Controllers
{
    [Route("api/dashboard")]
    public class DashboardController : Controller
    {
        [Route("sales")]
        [HttpGet]
        public IActionResult Sales()
        {
            var list = new List<Sale>();
            list.Add(new Sale { StoreId = 1111, Total = 1000, Value = 59385.00m });

            return Ok(list);
        }

        [Route("sales/{Per}/{Value:int}")]
        [HttpGet]
        public IActionResult Sales(string Per, int Value)
        {
            var list = new List<Sale>();
            list.Add(new Sale { StoreId = 1111, Total = 10, Value = 4092.00m });

            return Ok(list);
        }

        [Route("UsersWithLessOffer")]
        [HttpGet]
        public IActionResult UsersWithLessOffer()
        {
            var list = new List<Restriction>();
            list.Add(new Restriction { Users = 2, Type = "soy, diary" });
            list.Add(new Restriction { Users = 8, Type = "gluten" });
            
            return Ok(list);
        }
    }
}
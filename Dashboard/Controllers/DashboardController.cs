using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Dashboard.Models;

namespace Dashboard.Controllers
{
    [Route("api/dashboard")]
    public class DashboardController : Controller
    {
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("sales")]
        public IActionResult Sales()
        {
            var list = new List<Sale>();
            list.Add(new Sale { StoreId = 1111, Total = 1000, Value = 59385.00m });

            return Ok(list);
        }

        [Route("sales/{Per}/{Value:int}")]
        public IActionResult Sales(string Per, int Value)
        {
            var list = new List<Sale>();
            list.Add(new Sale { StoreId = 1111, Total = 10, Value = 4092.00m });

            return Ok(list);
        }

        [Route("UsersWithLessOffer")]
        public IActionResult UsersWithLessOffer()
        {
            List<Restriction> restrictions = new List<Restriction>();
            restrictions.Add(new Restriction { Type = "soy,diary", Users = 2 });
            restrictions.Add(new Restriction { Type = "gluten", Users = 8 });

            User user = new User()
            {
                Users = 10,
                Restrictions = restrictions,
                Usage = 50
            };

            return Ok(user);
        }
    }
}
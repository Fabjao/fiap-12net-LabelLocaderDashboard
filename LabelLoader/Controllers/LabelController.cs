using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabelLoader.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LabelLoader.Controllers
{
    [Route("api/labelLoader")]
    public class LabelController : Controller
    {
        [Route("LabelImageAdded")]
        public IActionResult LabelImageAdded()
        {
            List<Produto> produto2 = new List<Produto>();
            produto2.Add(new Produto()
            {
                ItemName = "meat",
                Ingredients = new string[] { "diary", "gluten", "soy" }
            });
            produto2.Add(new Produto()
            {
                ItemName = "bread",
                Ingredients = new string[] { "peanut", "gluten"}
            });

            return Ok(JsonConvert.SerializeObject(produto2));
        }
    }
}
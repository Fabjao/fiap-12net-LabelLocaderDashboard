using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabelLoader.Models;
using LabelLoader.Negocio;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LabelLoader.Controllers
{
    /// <summary>
    /// Api label Loader
    /// </summary>
    [Route("api/labelLoader")]
    public class LabelController : Controller
    {
        private ProdutoNegocio _produtoNegocio;

        /// <summary>
        /// Criar um novo objeto do produtoNegogio
        /// </summary>
        public LabelController()
        {
            _produtoNegocio = new ProdutoNegocio();
        }

        /// <summary>
        /// Lista de produtos com os ingredientes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("LabelImageAdded")]
        public IActionResult LabelImageAdded()
        {

            //Dados Mokado
            //List<Produto> produto2 = new List<Produto>();
            //produto2.Add(new Produto()
            //{
            //    ItemName = "Beef",
            //    Ingredients = new List<string>() { "Beef","Water","Sugar","Soy","Sauce","Wheat","Soybeans","Salt","Aooke Cider Vinegar","Natural Flavorings","Ppaprika","Natural Smole Flavoring" }
            //});
            //produto2.Add(new Produto()
            //{
            //    ItemName = "meat",
            //    Ingredients = new List<string>() { "diary", "gluten", "soy" }
            //});
            //produto2.Add(new Produto()
            //{
            //    ItemName = "bread",
            //    Ingredients = new List<string>() { "peanut", "gluten"}
            //});

            return Ok(_produtoNegocio.ListaDeProduto());
        }

    }
}
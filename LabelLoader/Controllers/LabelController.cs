using GeekBurger.LabelLocader.Contract.Models;
using LabelLoader.Logger;
using LabelLoader.Negocio;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LabelLoader.Controllers
{
    /// <summary>
    /// Api label Loader
    /// </summary>
    [Route("api/labelLoader")]
    public class LabelController : Controller
    {
        private IProdutoNegocio _produtoNegocio;
        private IServiceBus _serviceBus;
        private ILogServiceBus _logServiceBus;
        
        /// <summary>
        /// Criar um novo objeto do produtoNegogio
        /// </summary>
        public LabelController(IProdutoNegocio produtoNegocio, IServiceBus serviceBus, ILogServiceBus logServiceBus)
        {
            _produtoNegocio = produtoNegocio;
            _serviceBus = serviceBus;
            _logServiceBus = logServiceBus;            
        }

        [HttpGet]
        [Route("testeenviarparafila")]
        public async Task<IActionResult> EnviarParaFila()
        {
            await _logServiceBus.SendMessagesAsync("Iniciando dados mokados");
            var produtos = new List<Produto>();
            var json = "[{'ItemName':'beef','Ingredients':['BEEF','WATER','SUGAR','SOY SAUCE','WHEAT','SOYBEANS','SALT','APPLE CIDER VINEGAR','NATURAL FLAVORINGS','PAPRIKA','NATURAL SMOKE FLAVORING','WHEAT SOY']},{'ItemName':'bread','Ingredients':['ENRICHED WHEAT FLOUR','FLOUR','MALTED BARLEY FLOUR','REDUCED IRON','NIACIN','THIAMIN MONONITRATE','VITAMIN 81','RIBOFLAVIN','VITAMIN 82','ACID','WATER','WHOLE WHEAT FLOUR','SUGAR','YEAST','WHEAT GLUTEN','SOYBEAN OIL','SALT','CALCIUM PROPIONATE','PRESERVATIVE','SODIUM STEAROYL LACTYLATE/OR DATEM','CALCIUM SULFATE','MONO- DIGLYCERIDES','CELLULOSE GUM','GRAIN MONOCALCIUM PHOSPHATE','SOY LECITHIN','CORNSTARCH','POTASSIUM IODATE']},{'ItemName':'chicken','Ingredients':['BONELESS SKINLESS CHICKEN BREAST','CANNED ARTICHOKE HEARTS','RINSED','HERBED RED WINE VINAIGRETTE','RED WINE VINEGAR','PURE OLIVE OIL','OREGANO','BLACK PEPPER','BASIL','SALT','PARSLEY','RED PEPPER FLAKES','RAW ZUCCHINI','GRAPE TOMATOES','SLICED BLACK OLIVES','LEMON','FETA CHEESE','RED ONION']},{'ItemName':'ketchup','Ingredients':['ORGANIC TOMATO CONCENTRATE FROM RED RIPE ORGANIC TOMATOES','ORGANIC DISTILLED VINEGAR','ORGANIC SUGAR','SALT','ORGANIC ONION POWDER','ORGANIC SPICE','NATURAL FLAVORING']},{'ItemName':'mustard','Ingredients':['DISTILLED WHITE VINEGAR','MUSTARD SEED','WATER','SALT','TURMERIC','NATURAL FLAVOR SPICES']},{'ItemName':'onion-rings','Ingredients':['CORN MEAL','SOYA8EAN OIL/OR SUNFLOWER OIL','RICE FLOUR','SALT','WHEY','MONOSODIUM GLUTAMATE','BUTIERMILK PWDER','CARAMEL COLOR','NATURAL FLAVORS','CORN SYRUP MOS','WITODEX1RLN','SODIUM CASEINATE','LACTIC ACID DISODIUM DISODUM COLORS','YELLOW 5 LAKE','RED 40 FLAVOR']},{'ItemName':'pork','Ingredients':['KIDNEY BEANS','SAUSAGE','PORK','VENISON','SALT','DEXTROSE','SPICES','GARLIC','SODIUM ERTHYORBATE','SODIUM NITRITE','PRECOOKED PARBOILED LONG GRAIN BROWN RICE','MESQUITE SMOKE POWDER','ONIONS','BELL PEPPERS','RED PEPPER SEASONING','SEA SALT','BAY LEAF','BELL PEPPER','CELERY','ONION POWDER','RED PEPPER FLAKES PROCESSED IN A FACILITY THAT HANDLES WHEAT','EGG','MILK','FISH','SHELLFISH','SOYBEAN','PEANUT TREE NUT PRODUCTS','SOY','WHEAT']},{'ItemName':'salmon','Ingredients':['SALMON','SALMO SALAR','FISH','97%','SALT']},{'ItemName':'whole-bread','Ingredients':['WHOLE WHEAT FLOUR','WATER','SUGAR','WHEAT GLUTEN','SOYBEAN OIL','WHEAT BRAN','SALT','YEAST','CALCIUM PROPIONATE','PRESERVATIVE','MONO- ANO DIGLYCERIDES','DATEM','CALCIUM SULFATE','CITRIC ACID','POTASSIUM IODATE','SOY LECITHIN']}]";
            await _logServiceBus.SendMessagesAsync("Deserializando json mokado e colocando no Objeto");
            produtos = JsonConvert.DeserializeObject<List<Produto>>(json);
            //var produto = new Produto();
            //produto.ItemName = "YYYYY";
            //produto.Ingredients = new List<string>() { "xxxx", "dadssdsd" };
            //produtos.Add(produto);
            await _logServiceBus.SendMessagesAsync("Enviando para a fila LabelImageAdded");
            var retorno = _serviceBus.SendMessageAsync(produtos);
            return Ok(retorno);
        }

        /// <summary>
        /// Lista de produtos com os ingredientes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("produtosIngredientes")]
        public async Task<IActionResult> LabelImageAdded()
        {
            await _logServiceBus.SendMessagesAsync("Iniciando api cognitive");
            var operacao = _produtoNegocio.ListaDeProduto();
            if (operacao.Result.Sucesso)
                return Ok(operacao);
            else
                return NotFound(operacao);
        }
    }
}
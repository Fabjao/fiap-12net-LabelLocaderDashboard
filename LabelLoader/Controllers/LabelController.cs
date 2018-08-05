using GeekBurger.LabelLocader.Contract.Models;
using LabelLoader.Negocio;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text;

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


        /// <summary>
        /// Criar um novo objeto do produtoNegogio
        /// </summary>
        public LabelController(IProdutoNegocio produtoNegocio, IServiceBus serviceBus)
        {
            _produtoNegocio = produtoNegocio;
            _serviceBus = serviceBus;

        }

        [HttpGet]
        [Route("testeenviarparafila")]
        public IActionResult EnviarParaFila()
        {
            var produtos = new List<Produto>();
            var produto = new Produto();
            produto.ItemName = "YYYYY";
            produto.Ingredients = new List<string>() { "xxxx", "dadssdsd" };
            produtos.Add(produto);
            var retorno = _serviceBus.SendMessageAsync(produtos);
            return Ok(retorno);
        }

        /// <summary>
        /// Lista de produtos com os ingredientes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("produtosIngredientes")]
        public IActionResult LabelImageAdded()
        {
            var operacao = _produtoNegocio.ListaDeProduto();
            if (operacao.Result.Sucesso)
                return Ok(operacao);
            else
                return NotFound(operacao);
        }
    }
}
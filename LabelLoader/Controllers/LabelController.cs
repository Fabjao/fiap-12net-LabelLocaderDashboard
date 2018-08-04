using LabelLoader.Negocio;
using Microsoft.AspNetCore.Mvc;
using System.Text;

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
        [Route("produtosIngredientes")]
        public IActionResult LabelImageAdded()
        {
            

            var operacao = _produtoNegocio.ListaDeProdutoAsync();
            if (operacao.Sucesso)
                return Ok(operacao);
            else
                return NotFound(operacao);
        }
    }
}
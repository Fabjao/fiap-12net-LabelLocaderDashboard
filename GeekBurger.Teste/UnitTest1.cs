using LabelLoader.Negocio;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeekBurger.Teste
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var produtoNegocio = new ProdutoNegocio();
            var operacao = produtoNegocio.ListaDeProduto();
            Assert.IsTrue(operacao.Sucesso);
        }
    }
}

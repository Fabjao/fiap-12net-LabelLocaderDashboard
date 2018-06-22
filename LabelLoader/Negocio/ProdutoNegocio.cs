using LabelLoader.Models;
using System.Collections.Generic;
using System.IO;

namespace LabelLoader.Negocio
{
    public class ProdutoNegocio
    {
        public List<Produto> ListaDeProduto()
        {
            List<Produto> produtos = new List<Produto>();
            string path = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\Imagens"}";

            //var visionServiceClient =new VisionServiceClient("VisionAPIKey","https://westcentralus.api.cognitive.microsoft.com/vision/v1.0/");


            return produtos;
        }

    }
}

using LabelLoader.Models;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LabelLoader.Negocio
{
    /// <summary>
    /// Classe onde vai ser implementado as regras de negocio
    /// </summary>
    public class ProdutoNegocio
    {

        /// <summary>
        /// Vai passar pelo diretorio da aplicação e usar o cognitive vision para tratar as imagens
        /// </summary>
        /// <returns></returns>
        public List<Produto> ListaDeProduto()
        {
            var visionServiceClient = new VisionServiceClient("474e19bae7db4e97b5e226a61c2d841b", "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0/");
            List<Produto> produtos = new List<Produto>();
            DirectoryInfo dir = new DirectoryInfo($"{Directory.GetCurrentDirectory()}{@"\wwwroot\Imagens"}");
            OcrResults results;

            foreach (FileInfo file in dir.GetFiles())
            {                   
                using (Stream imageFileStream = File.OpenRead(file.FullName))
                {
                    results = visionServiceClient.RecognizeTextAsync(imageFileStream).Result;
                }

                var lines = results.Regions.SelectMany(region => region.Lines);
                var words = lines.SelectMany(line => line.Words);
                var wordsText = words.Select(word => word.Text.ToUpper());
            }
                                    

            return produtos;
        }


    }
}

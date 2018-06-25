using LabelLoader.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LabelLoader.Negocio
{
    /// <summary>
    /// Classe onde vai ser implementado as regras de negocio
    /// </summary>
    public class ProdutoNegocio
    {

        public static IConfiguration Configuration { get; set; }

        /// <summary>
        /// Vai passar pelo diretorio da aplicação e usar o cognitive vision para tratar as imagens
        /// # A leitura das imagens só seram feitas nos arquivos do tipo: JPEG, PNG, GIF, BMP
        /// </summary>
        /// <returns></returns>
        public List<Produto> ListaDeProduto()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            var chave = Configuration["ChaveAPIVIsion1.0"];

            var visionServiceClient = new VisionServiceClient( chave,
                "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0/");
           
            List<Produto> produtos = new List<Produto>();
            DirectoryInfo dir = new DirectoryInfo($"{Directory.GetCurrentDirectory()}{@"\wwwroot\Imagens"}");
            var tipoImagens = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            OcrResults results;
            var blacklist = new string[] { "INGREDIENTS", "ALLERGENS", "CONTAINS" };

            //Filtro para pegar apenas imagens
            var files = dir.GetFiles().Where(file => tipoImagens.Any(file.ToString().ToLower().EndsWith)).ToList();

            foreach (FileInfo file in files)
            {
                using (Stream imageFileStream = File.OpenRead(file.FullName))
                {
                    results = visionServiceClient.RecognizeTextAsync(imageFileStream).Result;
                }

                if (results.Regions.Length > 0)//Encontrou algo escrito na imagem
                {

                    var lines = results.Regions.SelectMany(region => region.Lines);
                    var words = lines.SelectMany(line => line.Words);
                    var wordsText = words.Select(word => Regex.Replace(word.Text.ToUpper(), @"[^A-Z]+", string.Empty));

                    List<string> ingredientes = new List<string>();
                    foreach (var item in wordsText)
                    {
                        if (!ingredientes.Contains(item))
                            ingredientes.Add(item);
                    }

                    produtos.Add(new Produto()
                    {
                        ItemName = file.Name.Replace(file.Extension, ""),
                        Ingredients = ingredientes
                    });
                }
            }
            return produtos;
        }
                
    }
}

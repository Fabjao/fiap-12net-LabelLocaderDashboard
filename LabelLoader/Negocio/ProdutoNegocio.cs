using GeekBurger.LabelLocader.Contract.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using Newtonsoft.Json;
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
        public Operacao ListaDeProduto()
        {
            var operacao = new Operacao() { Sucesso = true };

            try
            {

                List<Produto> produtos = new List<Produto>();
                var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
                Configuration = builder.Build();
                var chave = Configuration["ChaveAPIVIsion1.0"];
                var urlVision = Configuration["URLAPIVision"];

                var visionServiceClient = new VisionServiceClient(chave, urlVision);


                DirectoryInfo dir = new DirectoryInfo($"{Directory.GetCurrentDirectory()}{@"\wwwroot\Imagens"}");
                var tipoImagens = Configuration["TipoImagem"].Split(',');
                OcrResults results;
                var blacklist = Configuration["BlackList"].Split(',');

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
                        var wordsText = words.Select(word => word.Text.ToUpper());

                        string todasPalvras = "";
                        foreach (var item in wordsText)
                        {
                            todasPalvras += $"{item} ";
                        }

                        var substituiCarcteresPorVirgula = Configuration["substituiCarcteresPorVirgula"].Split(',');
                        foreach (var item in substituiCarcteresPorVirgula)
                        {
                            todasPalvras = todasPalvras.Replace(item, ",");
                        }

                        var sepadarasPorVirgula = todasPalvras.Split(',');
                        List<string> ingredientes = new List<string>();
                        foreach (var item in sepadarasPorVirgula)
                        {
                            var palavra = item;
                            // && !blacklist.Contains(item.Trim())
                            for (int i = 0; i < blacklist.Length - 1; i++)
                            {
                                palavra = palavra.Replace(blacklist[i], "").ToString();
                            }
                            if (!ingredientes.Contains(palavra.Trim()) && !string.IsNullOrWhiteSpace(palavra))
                                ingredientes.Add(palavra.Trim());
                        }

                        produtos.Add(new Produto()
                        {
                            ItemName = file.Name.Replace(file.Extension, ""),
                            Ingredients = ingredientes
                        });
                    }
                }
                operacao.Mensagem = JsonConvert.SerializeObject(produtos);
                return operacao;
            }
            catch (System.Exception ex)
            {
                operacao.Sucesso = false;
                operacao.Mensagem = "Erro ao processar as Imagens, tente novamente após 1 minuto";                
                return operacao;
            }
        }

    }
}

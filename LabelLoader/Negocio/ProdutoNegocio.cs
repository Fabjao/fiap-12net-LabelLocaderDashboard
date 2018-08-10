using GeekBurger.LabelLocader.Contract.Models;
using LabelLoader.Logger;
using Microsoft.Extensions.Configuration;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LabelLoader.Negocio
{
    /// <summary>
    /// Classe onde vai ser implementado as regras de negocio
    /// </summary>
    public class ProdutoNegocio : IProdutoNegocio
    {
        private IServiceBus _serviceBus;
        private IConfiguration _configuration;
        private ILogServiceBus _logServiceBus;

        public ProdutoNegocio(IServiceBus serviceBus, IConfiguration configuration, ILogServiceBus logServiceBus)
        {
            _serviceBus = serviceBus;
            _configuration = configuration;
            _logServiceBus = logServiceBus;
        }

        /// <summary>
        /// Vai passar pelo diretorio da aplicação e usar o cognitive vision para tratar as imagens
        /// # A leitura das imagens só seram feitas nos arquivos do tipo: JPEG, PNG, GIF, BMP
        /// </summary>
        /// <returns></returns>
        public async Task<Operacao> ListaDeProduto()
        {
            var operacao = new Operacao() { Sucesso = true };

            try
            {

                List<Produto> produtos = new List<Produto>();

                var chave = _configuration.GetValue<string>("ChaveAPIVIsion1.0");
                var urlVision = _configuration.GetValue<string>("URLAPIVision");

                var visionServiceClient = new VisionServiceClient(chave, urlVision);

                await _logServiceBus.SendMessagesAsync("Buscando as imagens no diretorio");
                DirectoryInfo dir = new DirectoryInfo($"{Directory.GetCurrentDirectory()}{@"\wwwroot\Imagens"}");
                var tipoImagens = _configuration.GetValue<string>("TipoImagem").Split(',');
                OcrResults results;
                var blacklist = _configuration.GetValue<string>("BlackList").Split(',');

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
                        await _logServiceBus.SendMessagesAsync($"Encontrou algo escrito na imagem{file.Name}");
                        var lines = results.Regions.SelectMany(region => region.Lines);
                        var words = lines.SelectMany(line => line.Words);
                        var wordsText = words.Select(word => word.Text.ToUpper());

                        string todasPalvras = "";
                        foreach (var item in wordsText)
                        {
                            todasPalvras += $"{item} ";
                        }

                        var substituiCarcteresPorVirgula = _configuration.GetValue<string>("substituiCarcteresPorVirgula").Split(',');
                        foreach (var item in substituiCarcteresPorVirgula)
                        {
                            todasPalvras = todasPalvras.Replace(item, ",");
                        }

                        var sepadarasPorVirgula = todasPalvras.Split(',');
                        List<string> ingredientes = new List<string>();
                        await _logServiceBus.SendMessagesAsync("Removendo palvras indesejadas");
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

                        await _logServiceBus.SendMessagesAsync("Adicionando produto na lista de item");
                        produtos.Add(new Produto()
                        {
                            ItemName = file.Name.Replace(file.Extension, ""),
                            Ingredients = ingredientes
                        });
                    }
                }
                await _logServiceBus.SendMessagesAsync("Enviando para a fila a lista de produtos");
                await _serviceBus.SendMessageAsync(produtos);
                operacao.Mensagem = JsonConvert.SerializeObject(produtos);
                return operacao;
            }
            catch (System.Exception ex)
            {
                await _logServiceBus.SendMessagesAsync($"Deu algo errado{ex.Message}");
                operacao.Sucesso = false;
                operacao.Mensagem = "Erro ao processar as Imagens, tente novamente após 1 minuto";                
                return operacao;
            }
        }

    }
}

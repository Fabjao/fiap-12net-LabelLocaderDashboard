using GeekBurger.LabelLocader.Contract.Models;
using LabelLoader.Logger;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelLoader.Negocio
{
    public class ServiceBus : IServiceBus
    {
        private readonly IConfiguration _configuration;
        private readonly ILogServiceBus _logServiceBus;

        public ServiceBus(IConfiguration configuration, ILogServiceBus logServiceBus)
        {
            _configuration = configuration;
            _logServiceBus = logServiceBus;
        }

        public async Task<bool> SendMessageAsync(List<Produto> produtos)
        {

            var config = _configuration.GetSection("serviceBus").Get<Model.ServiceBusConfiguration>();

            var topicClient = new TopicClient(config.ConnectionString, "LabelImageAdded");

            Message message = new Message();
            
            var json = JsonConvert.SerializeObject(produtos);
            message.Body = Encoding.UTF8.GetBytes(json);
      
            await topicClient.SendAsync(message);
            await _logServiceBus.SendMessagesAsync("mensagem enviada para a fila");
            await topicClient.CloseAsync();

            return true;
        }

    }
}

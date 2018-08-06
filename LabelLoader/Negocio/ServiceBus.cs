using GeekBurger.LabelLocader.Contract.Models;
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
        public ServiceBus(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendMessageAsync(List<Produto> produtos)
        {

            var config = _configuration.GetSection("serviceBus").Get<Model.ServiceBusConfiguration>();

            var topicClient = new TopicClient(config.ConnectionString, "meuprimeirotopico");

            Message message = new Message();
            
            var json = JsonConvert.SerializeObject(produtos);
            message.Body = Encoding.UTF8.GetBytes(json);
      
            await topicClient.SendAsync(message);

            await topicClient.CloseAsync();

            return true;
        }

    }
}

using GeekBurger.LabelLoader.Contract.Models;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ServiceBus.Fluent;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelLoader.Negocio
{
    public class ServiceBus
    {
        private static IConfiguration _configuration;
        public ServiceBus()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public IServiceBusNamespace GetServiceBusNamespace(IConfiguration configuration)
        {
            var config = configuration.GetSection("serviceBus").Get<ServiceBusConfiguration>();
            var credentials = SdkContext.AzureCredentialsFactory
            .FromServicePrincipal(config.ClientId,
            config.ClientSecret,
            config.TenantId,
            AzureEnvironment.AzureGlobalCloud);
            var serviceBusManager = ServiceBusManager
            .Authenticate(credentials, config.SubscriptionId);
            return serviceBusManager.Namespaces
            .GetByResourceGroup(config.ResourceGroup,
            config.NamespaceName);
        }

        public async Task SendMessagesAsync(List<Produto> produtos)
        {

            var connectionString = _configuration["serviceBus:connectionString"];
            var topicClient = new TopicClient(connectionString, "LabelImageAdded");

            //var labelsLidos = new List<Produto>() {
            //     new Produto{ Ingredients = new List<string> { "xxx", "xxx"} , ItemName = "xxx" }
            //};

            var lblsSerializados = JsonConvert.SerializeObject(produtos);

            var lblByteArray = Encoding.UTF8.GetBytes(lblsSerializados);

            var msg = new Message();
            msg.Body = lblByteArray;
            await topicClient.SendAsync(msg);

            await topicClient.CloseAsync();
        }


    }
}

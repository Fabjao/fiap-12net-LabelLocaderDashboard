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
using System.Threading;
using System.Threading.Tasks;

namespace Dashboard.Services
{
    public class ServiceBusReceive
    {
        private static Microsoft.Azure.ServiceBus.SubscriptionClient _subscriptionClient { get; set; }
        private static string _queueConnectionString { get; set; }
        private static string _storeId { get; set; }

        public void ReceiveAsync(string storeId)
        {
            _storeId = storeId;

            var serviceBusNamespace = Services.MyConfigurationRoot.GetByTag("namespaceName");


            _queueConnectionString = Services.MyConfigurationRoot.GetByTag("connectionString");

            var _configuration = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json")
                            .Build();

            var ret = GetServiceBusNamespace(_configuration);

            if(!ret.Topics.GetByName("ProductChanged").Subscriptions.List().Any(_ => _.Name == "DashBoard"))
            {
                ret.Topics.GetByName("ProductChanged").Subscriptions.Define("Dashboard").Create();
            }

            _subscriptionClient = new Microsoft.Azure.ServiceBus.SubscriptionClient(_queueConnectionString, "ProductChanged", "DashBoard");

            var handlerOptions = new MessageHandlerOptions(ExceptionHandler)
            {
                AutoComplete = false,
                MaxConcurrentCalls = 3
            };

            _subscriptionClient.RegisterMessageHandler(MessageHandler, handlerOptions);
        }

        private static Task ExceptionHandler(ExceptionReceivedEventArgs arg)
        {
            Console.WriteLine($"Message handler encountered an exception { arg.Exception}.");
            var context = arg.ExceptionReceivedContext;
            Console.WriteLine($"- Endpoint: {context.Endpoint}, Path: { context.EntityPath}, Action: { context.Action}");
            return Task.CompletedTask;
        }

        private static async Task MessageHandler(Message message, CancellationToken cancellationToken)
        {
            if (message.Label != _storeId)
            {
                Console.WriteLine($"Message From Store:{message.Label} with id {message.MessageId} not processed");
                return;
            }

            var productChangesString = Encoding.UTF8.GetString(message.Body);
            var productChanges = JsonConvert.DeserializeObject<GeekBurger.Users.Contract.UserFoodRestriction>(productChangesString);

            //here message is actually processed
            Thread.Sleep(1500);
            Console.WriteLine($"Message Processed:{productChangesString}");

            await _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
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
    }

    internal class ServiceBusConfiguration
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string TenantId { get; set; }
        public string SubscriptionId { get; set; }
        public string ResourceGroup { get; set; }
        public string NamespaceName { get; set; }
        public string ConnectionString { get; set; }
    }
}

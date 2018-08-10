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
using Dashboard.ViewModel;
using Microsoft.EntityFrameworkCore;
using Dashboard.Context;


namespace Dashboard.Services
{
   

    public class ServiceBusReceive
    {
        private static Microsoft.Azure.ServiceBus.SubscriptionClient _subscriptionClient { get; set; }
        private static Microsoft.Azure.ServiceBus.SubscriptionClient _subscriptionClient1 { get; set; }
        private static string _queueConnectionString { get; set; }
        private static string _storeId { get; set; }


        //ublic static Contexto _context;
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

            if(!ret.Topics.GetByName("OrderChanged").Subscriptions.List().Any(_ => _.Name == "DashBoard"))
            {
                ret.Topics.GetByName("OrderChanged").Subscriptions.Define("Dashboard").Create();
            }

            //if (!ret.Topics.GetByName("UserWithLessOffer").Subscriptions.List().Any(_ => _.Name == "DashBoard"))
            //{
            //    ret.Topics.GetByName("UserWithLessOffer").Subscriptions.Define("Dashboard").Create();
            //}

            _subscriptionClient = new Microsoft.Azure.ServiceBus.SubscriptionClient(_queueConnectionString, "OrderChanged", "DashBoard");

            //_subscriptionClient1 = new Microsoft.Azure.ServiceBus.SubscriptionClient(_queueConnectionString, "UserWithLessOffer", "DashBoard");

            var handlerOptions = new MessageHandlerOptions(ExceptionHandler)
            {
                AutoComplete = false,
                MaxConcurrentCalls = 3
            };

            _subscriptionClient.RegisterMessageHandler(MessageHandler, handlerOptions);
            //_subscriptionClient1.RegisterMessageHandler(MessageHandler, handlerOptions);
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
            DbContextOptions<Contexto> options;

            var builder = new DbContextOptionsBuilder<Contexto>();
            builder.UseInMemoryDatabase();
            options = builder.Options;

            List<OrderChangedViewModel> listaOrderChanged = new List<OrderChangedViewModel>();
            OrderChangedViewModel teste = new OrderChangedViewModel();
            if (message.Label != _storeId)
            {
                var orderChange =  Encoding.UTF8.GetString(message.Body);

               OrderChangedViewModel teste1 = JsonConvert.DeserializeObject<OrderChangedViewModel>(orderChange);

                if(teste1.OrderId != 0)
                {
                   

                    OrderChanged OrderChangedContext = new OrderChanged();

                    OrderChangedContext.OrderId = teste1.OrderId;
                    OrderChangedContext.State = teste1.State;
                    OrderChangedContext.StoredId = teste1.StoredId;
                    OrderChangedContext.Value = teste1.Value;


                   // var author = new OrderChanged { Id = 1, FirstName = "Joydip", LastName = "Kanjilal" };
                    using (var context = new Contexto(options))
                    {
                        context.Authors.Add(author);
                        context.SaveChanges();
                    }

                }

                Console.WriteLine($"Message From Store:{message.Label} with id {message.MessageId} not processed {orderChange.ToString()}");
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

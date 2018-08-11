using Dashboard.Context;
using GeekBurger.Orders.Contract.Enums;
using GeekBurger.Orders.Contract.Messages;
using Microsoft.Azure.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Dashboard.Services
{
    public class OrderChangedSBR
    {
        private const string topicName = "OrderChanged";

        private static SubscriptionClient _subscriptionClient { get; set; }

        public static async void ReceiveAsync()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json")
                            .Build();

            var serviceBusConfiguration = configuration.GetSection("serviceBus").Get<ServiceBusConfiguration>();

            //Subsciption
            var serviceBusNameSpace = serviceBusConfiguration.GetServiceBusNamespace();
            if (!serviceBusNameSpace.Topics.GetByName(topicName).Subscriptions.List().Any(_ => _.Name == "DashBoard"))
            {
                serviceBusNameSpace.Topics.GetByName(topicName).Subscriptions.Define("Dashboard").Create();
            }

            _subscriptionClient = new SubscriptionClient(serviceBusConfiguration.ConnectionString, topicName, "DashBoard");

            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 3,
                AutoComplete = false
            };

            _subscriptionClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        private static async Task ProcessMessagesAsync(Message message, CancellationToken cancellationToken)
        {
            var json = Encoding.UTF8.GetString(message.Body);

            var order = JsonConvert.DeserializeObject<OrderChangedMessage>(json);

            if (order.State == OrderState.Paid)
            {
                DbContextOptionsBuilder DbContextOptionsBuilder = new DbContextOptionsBuilder<DashboardDB>();

                using (var context = new DashboardDB(DbContextOptionsBuilder.Options))
                {
                    //context.Orders.Add(new Models.Order { StoreId = order.StoreId, OrderId = order.OrderId, Value = order.Value = DateTime.Now }); #FIX - Order API
                    context.Orders.Add(new Models.Order { StoreId = new Random().Next(1, 3), OrderId = new Random().Next(1, 99999), Value = new Random().Next(1, 100), Date = DateTime.Now });
                    context.SaveChanges();
                }
            }

            Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");

            await _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        {
            Console.WriteLine($"Message handler encountered an exception {arg.Exception}.");

            var context = arg.ExceptionReceivedContext;

            Console.WriteLine($"- Endpoint: {context.Endpoint}, Path: {context.EntityPath}, Action: {context.Action}");

            return Task.CompletedTask;
        }
    }
}

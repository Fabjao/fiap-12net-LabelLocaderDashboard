using Dashboard.Context;
using GeekBurger.Users.Contract;
using Microsoft.Azure.ServiceBus;
using Microsoft.EntityFrameworkCore;
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
    public class UserWithLessOffer
    {
        private const string queueName = "UserWithLessOffer";

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
            if (!serviceBusNameSpace.Topics.GetByName(queueName).Subscriptions.List().Any(_ => _.Name == "DashBoard"))
            {
                serviceBusNameSpace.Topics.GetByName(queueName).Subscriptions.Define("DashBoard").Create();
            }

            _subscriptionClient = new SubscriptionClient(serviceBusConfiguration.ConnectionString, queueName, "DashBoard");

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

            var userFoodRestriction = JsonConvert.DeserializeObject<UserFoodRestriction>(json);

            if (userFoodRestriction.Restrictions.Count() > 0)
            {
                DbContextOptionsBuilder DbContextOptionsBuilder = new DbContextOptionsBuilder<DashboardDB>();

                using (var context = new DashboardDB(DbContextOptionsBuilder.Options))
                {
                    //context.FoodRestrictions.Add(new Models.FoodRestriction { UserId = userFoodRestriction.UserId, Restrictions = string.Join(", ", userFoodRestriction.Restrictions.OrderBy(p => p)) }); #FIX - User API
                    context.FoodRestrictions.Add(new Models.FoodRestriction { UserId = new Random().Next(1, 99999), Restrictions = string.Join(", ", "soja,leite,pimenta,mel,carne".Split(",").OrderBy(p => p )) });
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

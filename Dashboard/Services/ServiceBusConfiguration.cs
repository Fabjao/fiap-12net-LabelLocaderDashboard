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
    public class ServiceBusConfiguration
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string TenantId { get; set; }
        public string SubscriptionId { get; set; }
        public string ResourceGroup { get; set; }
        public string NamespaceName { get; set; }
        public string ConnectionString { get; set; }

        public IServiceBusNamespace GetServiceBusNamespace()
        {
            var credentials = SdkContext.AzureCredentialsFactory.FromServicePrincipal(
                this.ClientId, 
                this.ClientSecret, 
                this.TenantId,
                AzureEnvironment.AzureGlobalCloud);

            var serviceBusManager = ServiceBusManager.Authenticate(credentials, this.SubscriptionId);

            return serviceBusManager.Namespaces.GetByResourceGroup(this.ResourceGroup, this.NamespaceName);
        }
    }
}

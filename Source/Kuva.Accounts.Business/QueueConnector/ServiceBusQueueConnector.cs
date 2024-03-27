using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Kuva.Accounts.Business.Exceptions;
using Kuva.Accounts.Business.QueueConnector.Domain;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

#nullable enable
namespace Kuva.Accounts.Business.QueueConnector
{
    internal class ServiceBusQueueConnector : IServiceQueueConnector
    {
        private readonly ServiceBusOptions _serviceBusOptions;
        private readonly IConfiguration _configuration;
        
        public ServiceBusQueueConnector(ServiceBusOptions serviceBusOptions, IConfiguration configuration)
        {
            _serviceBusOptions = serviceBusOptions;
            _configuration = configuration;
        }
        
        public async Task SendMessageToQueueAsync(string queueName, object? body)
        {
            CheckConnectionString();
            await using var client = new ServiceBusClient(_serviceBusOptions.ConnectionString);
            var sender = client.CreateSender(queueName);
            var message = CreateQueueMessage(body);
            await sender.SendMessageAsync(message);
        }
        
        private ServiceBusMessage CreateQueueMessage(object? body)
        {
            var bodyJson = JsonConvert.SerializeObject(body ?? new { });
            return new ServiceBusMessage(bodyJson);
        }
        
        private void CheckConnectionString()
        {
            if (string.IsNullOrEmpty(_serviceBusOptions.ConnectionString) || 
                string.IsNullOrWhiteSpace(_serviceBusOptions.ConnectionString))
                throw new ServiceQueueConnectorException(AccountsErrors.ServiceBusQueueConfigurationNull);

            if (string.IsNullOrEmpty(_serviceBusOptions.Key)
                || string.IsNullOrWhiteSpace(_serviceBusOptions.Key))
                throw new ServiceQueueConnectorException(AccountsErrors.ServiceBusQueueConfigurationNull);

            var connectionString = _serviceBusOptions.ConnectionString.Replace("{Azure:BusKey}", _serviceBusOptions.Key);
            _serviceBusOptions.ConnectionString = connectionString;
        }
    }
}
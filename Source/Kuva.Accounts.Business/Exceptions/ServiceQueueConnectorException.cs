using System.Collections.Generic;
using Azure.Messaging.ServiceBus;

#nullable enable
namespace Kuva.Accounts.Business.Exceptions
{
    public class ServiceQueueConnectorException : AccountsBusinessException
    {
        public ServiceQueueConnectorException(AccountsErrors accountsErrors,
            Queue<ServiceBusMessage>? messages = null, 
            ServiceBusMessage? currentMessage = null) : base(accountsErrors)
        {
            Messages = messages;
            CurrentMessage = currentMessage;
        }

        public Queue<ServiceBusMessage>? Messages { get; }
        public ServiceBusMessage? CurrentMessage { get; }
    }
}
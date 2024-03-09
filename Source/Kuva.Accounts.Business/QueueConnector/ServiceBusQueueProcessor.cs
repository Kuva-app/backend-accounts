using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Kuva.Accounts.Business.Exceptions;
using Kuva.Accounts.Business.QueueConnector.Domain;

#nullable enable
namespace Kuva.Accounts.Business.QueueConnector
{
    public class ServiceBusQueueProcessor : IProcessorConnector
    {
        public ServiceBusQueueProcessor(ServiceBusOptions serviceBusOptions)
        {
            _client = new ServiceBusClient(serviceBusOptions.ConnectionString);
        }

        private readonly ServiceBusClient _client;
        private ServiceBusProcessor? _processor;
        
        public event OnProcessMessageEventHandler? OnProcessMessage;
        public event OnErrorProcessMessageEventHandler? OnErrorProcessMessage;

        public async Task StartProcessMessageQueue(string queueName)
        {
            if (_processor != null && _processor.IsProcessing)
                return;

            CreateProcessor(queueName, () => new ServiceBusProcessorOptions
            {
                AutoCompleteMessages = true,
                MaxConcurrentCalls = 2
            });
            
            if (_processor == null)
                throw new ProcessorConnectorException(AccountsErrors.ProcessorQueueNotInstantiate);

            await _processor.StartProcessingAsync();
        }

        public async Task StopProcessMessageQueue()
        {
            if (_processor == null)
                return;
            await _processor.StopProcessingAsync();
            await _processor.DisposeAsync();
        }

        private void CreateProcessor(string queueName, Func<ServiceBusProcessorOptions> options)
        {
            if (_processor != null)
                return;
            var opt = options();
            _processor = _client.CreateProcessor(queueName, opt);
            _processor.ProcessMessageAsync += ProcessorOnProcessMessageAsync;
            _processor.ProcessErrorAsync += ProcessorOnProcessErrorAsync;
        }
        
        #region private
        private void InvokeProcessMessageEvent(string body)
        {
            var handler = OnProcessMessage;
            handler?.Invoke(body);
        }

        private void InvokeProcessErrorMessageEvent()
        {
            var handler = OnErrorProcessMessage;
            handler?.Invoke();
        }

        private async Task ProcessorOnProcessMessageAsync(ProcessMessageEventArgs arg)
        {
            string body = arg.Message.Body.ToString();
            InvokeProcessMessageEvent(body);
            await arg.CompleteMessageAsync(arg.Message);
        }

        private Task ProcessorOnProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            InvokeProcessErrorMessageEvent();
            return Task.CompletedTask;
        }
        #endregion
    }
}
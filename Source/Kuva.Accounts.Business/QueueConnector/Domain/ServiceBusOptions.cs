namespace Kuva.Accounts.Business.QueueConnector.Domain
{
    public class ServiceBusOptions
    {
        public const string OptionKey = "ServiceBus";

        public string ConnectionString { get; set; }

        public string Key { get;set;}
    }
}
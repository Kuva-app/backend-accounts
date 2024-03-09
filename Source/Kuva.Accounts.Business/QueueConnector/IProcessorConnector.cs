using System.Threading.Tasks;

namespace Kuva.Accounts.Business.QueueConnector
{
    public delegate void OnProcessMessageEventHandler(string body);

    public delegate void OnErrorProcessMessageEventHandler();
    
    public interface IProcessorConnector
    {
        Task StartProcessMessageQueue(string queueName);
        Task StopProcessMessageQueue();
        event OnProcessMessageEventHandler OnProcessMessage;
        event OnErrorProcessMessageEventHandler OnErrorProcessMessage;
    }
}
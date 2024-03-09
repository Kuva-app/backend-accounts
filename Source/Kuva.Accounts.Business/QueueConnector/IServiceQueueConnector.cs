using System.Threading.Tasks;

namespace Kuva.Accounts.Business.QueueConnector
{
    public interface IServiceQueueConnector
    {
        Task SendMessageToQueueAsync(string queueName, object body);
    }
}
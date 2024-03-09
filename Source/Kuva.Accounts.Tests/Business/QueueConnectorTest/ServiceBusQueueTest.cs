using System.Linq;
using System.Threading.Tasks;
using Kuva.Accounts.Business.QueueConnector;
using Kuva.Accounts.Entities;
using Kuva.Accounts.Tests.Fixtures;
using Newtonsoft.Json;
using Xunit;

namespace Kuva.Accounts.Tests.Business.QueueConnectorTest
{
    public class ServiceBusQueueTest
    {
        public ServiceBusQueueTest(IProcessorConnector processorConnector, IServiceQueueConnector serviceQueueConnector)
        {
            _processorConnector = processorConnector;
            _serviceQueueConnector = serviceQueueConnector;
        }

        private readonly IProcessorConnector _processorConnector;
        private readonly IServiceQueueConnector _serviceQueueConnector;
        
        [Fact]
        public async Task ProcessMessageTest()
        {
            var expected = UserFixture.GetUnregisteredUsers().First();
            
            _processorConnector.OnProcessMessage += async (body) =>
            {
                if (string.IsNullOrEmpty(body)) return;
                Assert.NotNull(body);
                Assert.NotEmpty(body);
                
                var actual = JsonConvert.DeserializeObject<UserEntity>(body);
                
                Assert.Equal(expected.Active, actual.Active);
                Assert.Equal(expected.Email, actual.Email);
                Assert.Equal(expected.Id, actual.Id);
                Assert.Equal(expected.Name, actual.Name);
                Assert.Equal(expected.Passcode, actual.Passcode);
                Assert.Equal(expected.CreateAt, actual.CreateAt);
                Assert.Equal(expected.UserLevelId, actual.UserLevelId);
                
                await _processorConnector.StopProcessMessageQueue();
            };
            
            _processorConnector.OnErrorProcessMessage += () =>
            {
                Assert.True(false);
            };
            
            await _processorConnector.StartProcessMessageQueue(ServiceBusConnectorFixture.QueueTestName);
            
            await _serviceQueueConnector.SendMessageToQueueAsync(ServiceBusConnectorFixture.QueueTestName, expected);
        }
    }
}
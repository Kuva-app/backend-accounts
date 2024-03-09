namespace Kuva.Accounts.Tests.Fixtures
{
    public static class ServiceBusConnectorFixture
    {
        public const string TopicTestName = "testeTopic";
        public const string QueueTestName = "testequeue";
        public const string ExpectedExceptionTopic = "Configuração do Azure Service Bus não está presente";
        public const string ExpectedObjectNullException = "UserVos.Shared.CommonUsers[0]";
    }
}
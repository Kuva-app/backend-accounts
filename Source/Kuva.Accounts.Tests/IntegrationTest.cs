using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Kuva.Accounts.Service.Models;
using Kuva.Accounts.Tests.Factory;
using Kuva.Accounts.Tests.Fixtures;

#nullable enable
namespace Kuva.Accounts.Tests
{
    public class IntegrationTest : IClassFixture<KuvaWebApplicationFactory>
    {
        private readonly KuvaWebApplicationFactory _factory;

        public IntegrationTest(KuvaWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/api/v1/version")]
        [InlineData("/api/v1/accounts/client/1")]
        [InlineData("/api/v1/accounts/client?mail=teste@teste.com.br")]
        public async Task GetEndpointTest(string path)
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync(path);
            Assert.Equal(GeneralFixture.ContentType, response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("/api/v1/accounts/password/request?mail=teste@teste.com.br", null)]
        [InlineData("/api/v1/accounts/client", typeof(RegisterClientModel))]
        public async Task PostEndpointTest(string path, Type? type = null)
        {
            var client = _factory.CreateClient();

            var body = GetDataOf(type);
            HttpContent content = JsonContent.Create(body, 
                new MediaTypeHeaderValue("application/json"),
                new JsonSerializerOptions(JsonSerializerDefaults.Web));
            
            var response = await client.PostAsync(path, content);
            
            Assert.Equal(GeneralFixture.ContentType, response.Content.Headers.ContentType?.ToString());
        }

        private static RegisterClientModel? GetDataOf(Type? type)
        {
            if (type == typeof(RegisterClientModel))
            {
                return RegisterClientModelFixture.GetDefaultValues();
            }
            return default;
        }
    }
}
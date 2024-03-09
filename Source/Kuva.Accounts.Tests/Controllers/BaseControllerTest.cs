using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace Kuva.Accounts.Tests.Controllers
{
    public abstract class BaseControllerTest
    {
        protected readonly HttpClient Client;

        protected BaseControllerTest()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseStartup<Service.Startup>());
            
            Client = server.CreateClient();
        }
    }
}
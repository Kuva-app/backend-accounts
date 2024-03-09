using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Kuva.Accounts.Tests
{
    public interface IStartup
    {
        void ConfigureServices(IServiceCollection services);
    }
}

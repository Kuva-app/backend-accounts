using Kuva.Accounts.Business;
using Kuva.Accounts.Entities;
using Kuva.Accounts.Service.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kuva.Accounts.Tests
{
    public class Startup : IStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile(Fixtures.GeneralFixture.AppSettingsFileName)
                .AddUserSecrets<Startup>()
                .Build();

            services.UseAccountsBusiness(configuration);

            InjectMapper(services);
        }

        private static void InjectMapper(IServiceCollection services)
        {
            services.AddAutoMapper(config =>
            {
                config.CreateMap<UserEntity, ClientModel>().ReverseMap();
                config.CreateMap<UserEntity, RegisterClientModel>().ReverseMap();
            });
        }
    }
}
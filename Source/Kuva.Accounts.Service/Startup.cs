using Kuva.Accounts.Business;
using Kuva.Accounts.Entities;
using Kuva.Accounts.Service.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Kuva.Accounts.Service
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Startup>()
                .Build();

            services.AddSingleton(configuration);

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Kuva.Accounts.Service", Version = "v1" });
            });
            InjectMapper(services);
            InjectServices(services, configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kuva.Accounts.Service v1"));
            }

            loggerFactory.AddLog4Net();
            loggerFactory.AddKuvaAccountsLogging();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private static void InjectServices(IServiceCollection services, IConfiguration configuration)
        {
            services.UseAccountsBusiness(configuration);
        }

        private static void InjectMapper(IServiceCollection services)
        {
            services.AddAutoMapper(config =>
            {
                config.CreateMap<ClientModel, UserEntity>().ReverseMap();
                config.CreateMap<RegisterClientModel, UserEntity>().ReverseMap();
            });
        }
    }
}
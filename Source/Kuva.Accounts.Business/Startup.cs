using System;
using Elastic.Apm.SerilogEnricher;
using Kuva.Accounts.Business.Interfaces;
using Kuva.Accounts.Business.Logging;
using Kuva.Accounts.Business.Logging.Interfaces;
using Kuva.Accounts.Business.QueueConnector;
using Kuva.Accounts.Business.UseCases;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Kuva.Accounts.Repository;
using Kuva.Accounts.Business.QueueConnector.Domain;
using Kuva.Accounts.Business.UseCases.Interfaces;
using Kuva.Accounts.Business.Operations;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace Kuva.Accounts.Business
{
    public static class Startup
    {
        public static void UseAccountsBusiness(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRepository(configuration);
            services.InjectUseCases();
            services.InjectBusiness();
            services.InjectServices(configuration);
            services.InjectOperations();
            services.InjectLogging(configuration);
        }

        private static void InjectUseCases(this IServiceCollection services)
        {
            services.AddTransient<IChangePasswordUseCase, ChangePasswordUseCase>();
            services.AddTransient<IRequestChangePasswordUseCase, RequestChangePasswordUseCase>();
            services.AddTransient<IClientUseCase, ClientUseCase>();
        }

        private static void InjectBusiness(this IServiceCollection services)
        {
            services.AddTransient<ISearchUserBusiness, SearchUserBusiness>();
            services.AddTransient<IChangePasswordBusiness, ChangePasswordBusiness>();
            services.AddTransient<IRequestChangePasswordBusiness, RequestChangePasswordBusiness>();
            services.AddTransient<IClientBusiness, ClientBusiness>();
        }

        private static void InjectServices(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceBusOptions = new ServiceBusOptions();
            configuration.GetSection(ServiceBusOptions.OptionKey).Bind(serviceBusOptions);
            services.AddSingleton(serviceBusOptions);

            services.AddTransient<IProcessorConnector, ServiceBusQueueProcessor>();
            services.AddTransient<IServiceQueueConnector, ServiceBusQueueConnector>();
        }

        private static void InjectOperations(this IServiceCollection services)
        {
            services.AddTransient<AdministratorOperations>();
            services.AddTransient<UserOperations>();
            services.AddTransient<ClientOperations>();
            services.AddTransient<GuestOperations>();

        }

        private static void InjectLogging(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IKuvaLogger, KuvaElasticSearchLogger>();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithElasticApmCorrelationInfo()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithProperty("version", Constants.GetAssemblyFileVersion())
                .WriteTo.Console()
                .WriteTo.Debug()
                .WriteTo.Elasticsearch(ConfigureElasticSink(configuration))
                .CreateLogger();

            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: false));
        }

        private static ElasticsearchSinkOptions ConfigureElasticSink(IConfiguration configuration)
        {
            var elasticSearchUriString = configuration[Constants.SettingsElasticUrlString];
            var index = Constants.GetElasticIndex(configuration);
            return new ElasticsearchSinkOptions(new Uri(elasticSearchUriString))
            {
                AutoRegisterTemplate = true,
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv8,
                IndexFormat = $"{index}"
            };
        }

        public static void AddKuvaAccountsLogging(this ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();
        }
    }
}

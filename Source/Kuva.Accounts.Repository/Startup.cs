using System;
using Kuva.Accounts.Entities;
using Kuva.Accounts.Repository.Data;
using Kuva.Accounts.Repository.Data.Interfaces;
using Kuva.Accounts.Repository.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kuva.Accounts.Repository
{
    public static class Startup
    {
        public static void AddRepository(this IServiceCollection services, IConfiguration configuration, string migrationAssemblyName = Constants.MigrationAssemblyName)
        {
            services?.AddDbContextFactory<AccountsContext>(opt =>
            {
                string connectionString = string.Empty;
                
                #if DEBUG
                connectionString = configuration["ConnectionStrings:kuvaConnection"];
                var user = configuration["DB:User"];
                var password = configuration["DB:Password"];
                var host = configuration["DB:Host"];
                connectionString = connectionString.Replace("{user}", user).Replace("{password}", password).Replace("{host}", host);
                #else
                connectionString = configuration.GetConnectionString(Constants.ConnectionStringName);
                #endif

                opt.UseSqlServer(connectionString, options =>
                {
                    options.MigrationsAssembly(migrationAssemblyName);
                });
            });
            services?.AddData();
            services?.AddMapper();
        }

        private static void AddData(this IServiceCollection services)
        {
            services.AddTransient<IUserData, UserData>();
            services.AddTransient<IUserLevelData, UserLevelData>();
            services.AddTransient<IUserTokenData, UserTokenData>();
        }

        private static void AddMapper(this IServiceCollection services)
        {
            services?.AddAutoMapper(config =>
            {
                config.CreateMap<UserLevelEntity, UserLevelDomain>().ReverseMap();
                config.CreateMap<UserEntity, UserDomain>()
                .ForMember(dest => dest.UserLevelId, opt => opt.MapFrom(src => src.UserLevelId))
                .ReverseMap();
                config.CreateMap<UserTokenEntity, UserTokenDomain>().ReverseMap();
            });
        }
    }
}
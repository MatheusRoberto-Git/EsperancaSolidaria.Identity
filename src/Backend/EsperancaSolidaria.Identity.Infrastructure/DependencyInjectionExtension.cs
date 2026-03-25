using EsperancaSolidaria.Identity.Domain.Repositories;
using EsperancaSolidaria.Identity.Domain.Repositories.Token;
using EsperancaSolidaria.Identity.Domain.Repositories.User;
using EsperancaSolidaria.Identity.Domain.Security.Criptography;
using EsperancaSolidaria.Identity.Domain.Security.Tokens;
using EsperancaSolidaria.Identity.Domain.Services.LoggedUser;
using EsperancaSolidaria.Identity.Infrastructure.DataAccess;
using EsperancaSolidaria.Identity.Infrastructure.DataAccess.Repositories;
using EsperancaSolidaria.Identity.Infrastructure.Extensions;
using EsperancaSolidaria.Identity.Infrastructure.Security.Criptography;
using EsperancaSolidaria.Identity.Infrastructure.Security.Tokens.Access.Generator;
using EsperancaSolidaria.Identity.Infrastructure.Security.Tokens.Access.Validator;
using EsperancaSolidaria.Identity.Infrastructure.Security.Tokens.Refresh;
using EsperancaSolidaria.Identity.Infrastructure.Services.LoggedUser;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EsperancaSolidaria.Identity.Infrastructure
{
    public static class DependencyInjectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddPasswordEncripter(services);
            AddRepositories(services);
            AddLoggedUser(services);
            AddTokens(services, configuration);

            AddDbContext(services, configuration);
            AddFluentMigrator(services, configuration);
        }

        private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.ConnectionString();

            services.AddDbContext<EsperancaSolidariaIdentityDbContext>(dbContextOptions =>
            {
                dbContextOptions.UseSqlServer(connectionString);
            });
        }

        private static void AddFluentMigrator(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.ConnectionString();

            services.AddFluentMigratorCore().ConfigureRunner(options =>
            {
                options
                .AddSqlServer()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("EsperancaSolidaria.Identity.Infrastructure")).For.All();
            });
        }

        private static void AddPasswordEncripter(IServiceCollection services)
        {
            services.AddScoped<IPasswordEncripter, BCryptNet>();
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
            services.AddScoped<IUserReadOnlyRepository, UserRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
        }

        private static void AddLoggedUser(IServiceCollection services) => services.AddScoped<ILoggedUser, LoggedUser>();

        private static void AddTokens(IServiceCollection services, IConfiguration configuration)
        {
            var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
            var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

            services.AddScoped<IAccessTokenGenerator>(options => new JwtTokenGenerator(expirationTimeMinutes, signingKey!));
            services.AddScoped<IAccessTokenValidator>(options => new JwtTokenValidator(signingKey!));

            services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
        }
    }
}
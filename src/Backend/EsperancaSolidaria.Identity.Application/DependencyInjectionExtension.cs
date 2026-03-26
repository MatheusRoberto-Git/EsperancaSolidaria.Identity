using EsperancaSolidaria.Identity.Application.Services.Mapster;
using EsperancaSolidaria.Identity.Application.UseCases.Login.DoLogin;
using EsperancaSolidaria.Identity.Application.UseCases.Token.RefreshToken;
using EsperancaSolidaria.Identity.Application.UseCases.User.Register;
using EsperancaSolidaria.Identity.Application.UseCases.User.UpdateRole;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sqids;

namespace EsperancaSolidaria.Identity.Application
{
    public static class DependencyInjectionExtension
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            AddMapsterConfiguration();
            AddIdEncoder(services, configuration);
            AddUseCases(services);
        }
        
        private static void AddMapsterConfiguration()
        {
            MapConfigs.Configure();
        }

        private static void AddIdEncoder(IServiceCollection services, IConfiguration configuration)
        {
            var sqids = new SqidsEncoder<long>(new()
            {
                MinLength = 3,
                Alphabet = configuration.GetValue<string>("Settings:IdCryptographyAlphabet")!
            });

            services.AddSingleton(sqids);
        }

        private static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
            services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
            services.AddScoped<IUseRefreshTokenUseCase, UseRefreshTokenUseCase>();
            services.AddScoped<IUpdateRoleUserUseCase, UpdateRoleUserUseCase>();
        }
    }
}
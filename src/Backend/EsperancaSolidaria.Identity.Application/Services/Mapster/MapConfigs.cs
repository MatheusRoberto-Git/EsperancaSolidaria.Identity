using EsperancaSolidaria.Identity.Communication.Requests;
using EsperancaSolidaria.Identity.Domain.Entities;
using Mapster;

namespace EsperancaSolidaria.Identity.Application.Services.Mapster
{
    public static class MapConfigs
    {
        public static void Configure()
        {
            TypeAdapterConfig<RequestRegisterUserJson, User>
                .NewConfig()
                .Ignore(dest => dest.Password);
        }
    }
}
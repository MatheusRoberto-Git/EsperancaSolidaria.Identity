using EsperancaSolidaria.Identity.Domain.Enums;

namespace EsperancaSolidaria.Identity.Domain.Security.Tokens
{
    public interface IAccessTokenGenerator
    {
        public string Generate(Guid userIdentifier, UserRole role);
    }
}
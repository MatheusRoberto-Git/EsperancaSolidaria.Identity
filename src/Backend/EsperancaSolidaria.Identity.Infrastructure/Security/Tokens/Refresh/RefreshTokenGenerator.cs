using EsperancaSolidaria.Identity.Domain.Security.Tokens;

namespace EsperancaSolidaria.Identity.Infrastructure.Security.Tokens.Refresh
{
    public class RefreshTokenGenerator : IRefreshTokenGenerator
    {
        public string Generate() => Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    }
}
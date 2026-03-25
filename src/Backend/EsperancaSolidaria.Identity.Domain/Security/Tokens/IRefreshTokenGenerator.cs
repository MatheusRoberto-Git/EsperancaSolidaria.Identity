namespace EsperancaSolidaria.Identity.Domain.Security.Tokens
{
    public interface IRefreshTokenGenerator
    {
        public string Generate();
    }
}
using System.Net;

namespace EsperancaSolidaria.Identity.Exceptions.ExceptionsBase
{
    public class RefreshTokenExpiredException : EsperancaSolidariaIdentityException
    {
        public RefreshTokenExpiredException() : base(ResourceMessagesException.INVALID_SESSION) { }

        public override IList<string> GetErrorMessages() => [Message];

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.Forbidden;
    }
}
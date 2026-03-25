using System.Net;

namespace EsperancaSolidaria.Identity.Exceptions.ExceptionsBase
{
    public class RefreshTokenNotFoundException : EsperancaSolidariaIdentityException
    {
        public RefreshTokenNotFoundException() : base(ResourceMessagesException.EXPIRED_SESSION) { }

        public override IList<string> GetErrorMessages() => [Message];

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
    }
}
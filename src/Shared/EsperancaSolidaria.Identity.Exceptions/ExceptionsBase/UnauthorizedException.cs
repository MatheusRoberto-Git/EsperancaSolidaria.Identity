using System.Net;

namespace EsperancaSolidaria.Identity.Exceptions.ExceptionsBase
{
    public class UnauthorizedException : EsperancaSolidariaIdentityException
    {
        public UnauthorizedException(string message) : base(message) { }

        public override IList<string> GetErrorMessages() => [Message];

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
    }
}
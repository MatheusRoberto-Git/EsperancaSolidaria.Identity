using EsperancaSolidaria.Identity.Exceptions.ExceptionsBase;
using System.Net;

namespace MyRecipeBook.Exceptions.ExceptionsBase
{
    public class NotFoundException : EsperancaSolidariaIdentityException
    {
        public NotFoundException(string message) : base(message) { }

        public override IList<string> GetErrorMessages() => [Message];

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.NotFound;
    }
}
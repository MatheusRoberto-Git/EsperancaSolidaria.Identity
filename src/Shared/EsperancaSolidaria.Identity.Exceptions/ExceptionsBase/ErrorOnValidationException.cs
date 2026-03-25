using System.Net;

namespace EsperancaSolidaria.Identity.Exceptions.ExceptionsBase
{
    public class ErrorOnValidationException : EsperancaSolidariaIdentityException
    {
        private readonly IList<string> _errorMessages;

        public ErrorOnValidationException(IList<string> errorMessages) : base(string.Empty)
        {
            _errorMessages = errorMessages;
        }

        public override IList<string> GetErrorMessages() => _errorMessages;

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;        
    }
}
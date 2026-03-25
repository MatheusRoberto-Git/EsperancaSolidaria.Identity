using System.Net;

namespace EsperancaSolidaria.Identity.Exceptions.ExceptionsBase
{
    public abstract class EsperancaSolidariaIdentityException : SystemException
    {
        protected EsperancaSolidariaIdentityException(string message) : base(message) { }

        public abstract IList<string> GetErrorMessages();

        public abstract HttpStatusCode GetStatusCode();
    }
}
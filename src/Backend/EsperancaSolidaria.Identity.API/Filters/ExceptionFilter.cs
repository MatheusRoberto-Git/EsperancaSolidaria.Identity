using EsperancaSolidaria.Identity.Communication.Responses;
using EsperancaSolidaria.Identity.Exceptions;
using EsperancaSolidaria.Identity.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EsperancaSolidaria.Identity.API.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if(context.Exception is EsperancaSolidariaIdentityException esperancaSolidariaIdentityException)
            {
                HandleProjectException(esperancaSolidariaIdentityException, context);
            }
            else
            {
                ThrowUnknowException(context);
            }
        }        

        private static void HandleProjectException(EsperancaSolidariaIdentityException esperancaSolidariaIdentityException, ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)esperancaSolidariaIdentityException.GetStatusCode();
            context.Result = new ObjectResult(new ResponseErrorJson(esperancaSolidariaIdentityException.GetErrorMessages()));
        }

        private void ThrowUnknowException(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new ObjectResult(new ResponseErrorJson(ResourceMessagesException.UNKNOW_ERROR));
        }
    }
}

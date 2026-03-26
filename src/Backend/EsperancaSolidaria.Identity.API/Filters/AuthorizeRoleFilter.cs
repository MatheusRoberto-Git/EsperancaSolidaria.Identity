using EsperancaSolidaria.Identity.Communication.Responses;
using EsperancaSolidaria.Identity.Domain.Enums;
using EsperancaSolidaria.Identity.Domain.Services.LoggedUser;
using EsperancaSolidaria.Identity.Exceptions;
using EsperancaSolidaria.Identity.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EsperancaSolidaria.Identity.API.Filters
{
    public class AuthorizeRoleFilter : IAsyncAuthorizationFilter
    {
        private readonly ILoggedUser _loggedUser;
        private readonly UserRole[] _roles;

        public AuthorizeRoleFilter(ILoggedUser loggedUser, UserRole[] roles)
        {
            _loggedUser = loggedUser;
            _roles = roles;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {
                var user = await _loggedUser.User();

                if(!_roles.Contains(user.Role))
                {
                    throw new UnauthorizedException(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE);
                }
            }
            catch(EsperancaSolidariaIdentityException ex)
            {
                context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(ex.GetErrorMessages()));
            }
            catch
            {
                context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE));
            }
        }
    }
}
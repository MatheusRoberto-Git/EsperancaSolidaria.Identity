using EsperancaSolidaria.Identity.Communication.Responses;
using EsperancaSolidaria.Identity.Domain.Repositories.User;
using EsperancaSolidaria.Identity.Domain.Security.Tokens;
using EsperancaSolidaria.Identity.Exceptions;
using EsperancaSolidaria.Identity.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace EsperancaSolidaria.Identity.API.Filters
{
    public class AuthenticatedUserFilter : IAsyncAuthorizationFilter
    {
        private readonly IAccessTokenValidator _accessTokenValidator;
        private readonly IUserReadOnlyRepository _repository;

        public AuthenticatedUserFilter(IAccessTokenValidator accessTokenValidator, IUserReadOnlyRepository repository)
        {
            _accessTokenValidator = accessTokenValidator;
            _repository = repository;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {
                var token = TokenOnRequest(context);
                var userIdentifier = _accessTokenValidator.ValidateAndGetUserIdentifier(token);
                var exist = await _repository.ExistActiveUserWithIdentifier(userIdentifier);

                if(!exist)
                {
                    throw new UnauthorizedException(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE);
                }
            }
            catch(SecurityTokenExpiredException)
            {
                context.Result = new UnauthorizedObjectResult(new ResponseErrorJson("TokenIsExpired")
                {
                    TokenExpired = true
                });
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

        private static string TokenOnRequest(AuthorizationFilterContext context)
        {
            var authentication = context.HttpContext.Request.Headers.Authorization.ToString();

            if(string.IsNullOrEmpty(authentication))
            {
                throw new UnauthorizedException(ResourceMessagesException.NO_TOKEN);
            }

            return authentication["Bearer ".Length..].Trim();
        }
    }
}
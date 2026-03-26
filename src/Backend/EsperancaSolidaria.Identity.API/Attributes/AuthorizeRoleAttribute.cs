using EsperancaSolidaria.Identity.API.Filters;
using EsperancaSolidaria.Identity.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace EsperancaSolidaria.Identity.API.Attributes
{
    public class AuthorizeRoleAttribute : TypeFilterAttribute
    {
        public AuthorizeRoleAttribute(params UserRole[] roles) : base(typeof(AuthorizeRoleFilter))
        {
            Arguments = [roles];
        }
    }
}
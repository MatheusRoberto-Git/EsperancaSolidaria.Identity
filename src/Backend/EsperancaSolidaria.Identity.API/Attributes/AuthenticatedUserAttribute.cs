using EsperancaSolidaria.Identity.API.Filters;
using Microsoft.AspNetCore.Mvc;

namespace EsperancaSolidaria.Identity.API.Attributes
{
    public class AuthenticatedUserAttribute : TypeFilterAttribute
    {
        public AuthenticatedUserAttribute() : base(typeof(AuthenticatedUserFilter)) { }
    }
}
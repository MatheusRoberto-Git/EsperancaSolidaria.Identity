using EsperancaSolidaria.Identity.Domain.Entities;

namespace EsperancaSolidaria.Identity.Domain.Services.LoggedUser
{
    public interface ILoggedUser
    {
        public Task<User> User();
    }
}
using EsperancaSolidaria.Identity.Communication.Requests;

namespace EsperancaSolidaria.Identity.Application.UseCases.User.UpdateRole
{
    public interface IUpdateRoleUserUseCase
    {
        public Task Execute(RequestUpdateRoleUserJson request);
    }
}
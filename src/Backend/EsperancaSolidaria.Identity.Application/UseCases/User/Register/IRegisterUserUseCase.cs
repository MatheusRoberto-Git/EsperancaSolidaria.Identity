using EsperancaSolidaria.Identity.Communication.Requests;
using EsperancaSolidaria.Identity.Communication.Responses;

namespace EsperancaSolidaria.Identity.Application.UseCases.User.Register
{
    public interface IRegisterUserUseCase
    {
        public Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request);
    }
}
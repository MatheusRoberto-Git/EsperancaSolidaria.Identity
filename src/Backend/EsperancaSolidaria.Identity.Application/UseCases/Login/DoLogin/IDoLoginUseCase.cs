using EsperancaSolidaria.Identity.Communication.Requests;
using EsperancaSolidaria.Identity.Communication.Responses;

namespace EsperancaSolidaria.Identity.Application.UseCases.Login.DoLogin
{
    public interface IDoLoginUseCase
    {
        public Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request);
    }
}
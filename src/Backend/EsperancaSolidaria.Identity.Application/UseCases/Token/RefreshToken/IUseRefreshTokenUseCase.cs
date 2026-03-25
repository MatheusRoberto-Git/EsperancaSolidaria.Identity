using EsperancaSolidaria.Identity.Communication.Requests;
using EsperancaSolidaria.Identity.Communication.Responses;

namespace EsperancaSolidaria.Identity.Application.UseCases.Token.RefreshToken
{
    public interface IUseRefreshTokenUseCase
    {
        Task<ResponseTokensJson> Execute(RequestNewTokenJson request);
    }
}
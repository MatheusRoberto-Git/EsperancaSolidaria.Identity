using EsperancaSolidaria.Identity.Communication.Requests;
using EsperancaSolidaria.Identity.Communication.Responses;
using EsperancaSolidaria.Identity.Domain.Repositories;
using EsperancaSolidaria.Identity.Domain.Repositories.Token;
using EsperancaSolidaria.Identity.Domain.Security.Tokens;
using EsperancaSolidaria.Identity.Domain.ValueObjects;
using EsperancaSolidaria.Identity.Exceptions.ExceptionsBase;

namespace EsperancaSolidaria.Identity.Application.UseCases.Token.RefreshToken
{
    public class UseRefreshTokenUseCase : IUseRefreshTokenUseCase
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccessTokenGenerator _accessTokenGenerator;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;

        public UseRefreshTokenUseCase(ITokenRepository tokenRepository, IUnitOfWork unitOfWork, IAccessTokenGenerator accessTokenGenerator, IRefreshTokenGenerator refreshTokenGenerator)
        {
            _tokenRepository = tokenRepository;
            _unitOfWork = unitOfWork;
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
        }

        public async Task<ResponseTokensJson> Execute(RequestNewTokenJson request)
        {
            var refreshToken = await _tokenRepository.Get(request.RefreshToken);

            if(refreshToken is null)
            {
                throw new RefreshTokenNotFoundException();
            }

            var refreshTokenValidUntil = refreshToken.CreatedOn.AddDays(EsperancaSolidariaIdentityRuleConstants.REFRESH_TOKEN_EXPIRATION_DAYS);

            if(DateTime.Compare(refreshTokenValidUntil, DateTime.UtcNow) < 0)
            {
                throw new RefreshTokenExpiredException();
            }

            var newRefreshToken = new Domain.Entities.RefreshToken
            {
                Value = _refreshTokenGenerator.Generate(),
                UserId = refreshToken.UserId
            };

            await _tokenRepository.SaveNewRefreshToken(newRefreshToken);
            await _unitOfWork.Commit();

            return new ResponseTokensJson
            {
                AccessToken = _accessTokenGenerator.Generate(refreshToken.User.UserIdentifier, refreshToken.User.Role),
                RefreshToken = newRefreshToken.Value
            };
        }
    }
}
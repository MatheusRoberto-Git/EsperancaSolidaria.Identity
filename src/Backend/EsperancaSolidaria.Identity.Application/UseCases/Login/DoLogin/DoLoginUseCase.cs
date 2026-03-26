using EsperancaSolidaria.Identity.Communication.Requests;
using EsperancaSolidaria.Identity.Communication.Responses;
using EsperancaSolidaria.Identity.Domain.Extensions;
using EsperancaSolidaria.Identity.Domain.Repositories;
using EsperancaSolidaria.Identity.Domain.Repositories.Token;
using EsperancaSolidaria.Identity.Domain.Repositories.User;
using EsperancaSolidaria.Identity.Domain.Security.Criptography;
using EsperancaSolidaria.Identity.Domain.Security.Tokens;
using EsperancaSolidaria.Identity.Exceptions.ExceptionsBase;

namespace EsperancaSolidaria.Identity.Application.UseCases.Login.DoLogin
{
    public class DoLoginUseCase : IDoLoginUseCase
    {
        private readonly IUserReadOnlyRepository _repository;
        private readonly IPasswordEncripter _passwordEncripter;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly ITokenRepository _tokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccessTokenGenerator _accessTokenGenerator;

        public DoLoginUseCase(IUserReadOnlyRepository repository, IPasswordEncripter passwordEncripter, IRefreshTokenGenerator refreshTokenGenerator, ITokenRepository tokenRepository, IUnitOfWork unitOfWork, IAccessTokenGenerator accessTokenGenerator)
        {
            _repository = repository;
            _passwordEncripter = passwordEncripter;
            _refreshTokenGenerator = refreshTokenGenerator;
            _tokenRepository = tokenRepository;
            _unitOfWork = unitOfWork;
            _accessTokenGenerator = accessTokenGenerator;
        }

        public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
        {
            var user = await _repository.GetByEmail(request.Email);

            if(user is null || _passwordEncripter.IsValid(request.Password, user.Password).IsFalse())
            {
                throw new InvalidLoginException();
            }

            var refreshToken = await CreateAndSaveRefreshToken(user);

            return new ResponseRegisteredUserJson
            {
                Name = user.Name,
                Tokens = new ResponseTokensJson
                {
                    AccessToken = _accessTokenGenerator.Generate(user.UserIdentifier, user.Role),
                    RefreshToken = refreshToken
                }
            };
        }

        private async Task<string> CreateAndSaveRefreshToken(Domain.Entities.User user)
        {
            var refreshToken = new Domain.Entities.RefreshToken
            {
                Value = _refreshTokenGenerator.Generate(),
                UserId = user.Id,
            };

            await _tokenRepository.SaveNewRefreshToken(refreshToken);
            await _unitOfWork.Commit();

            return refreshToken.Value;
        }
    }
}
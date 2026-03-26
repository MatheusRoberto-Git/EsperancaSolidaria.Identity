using EsperancaSolidaria.Identity.Communication.Requests;
using EsperancaSolidaria.Identity.Communication.Responses;
using EsperancaSolidaria.Identity.Domain.Entities;
using EsperancaSolidaria.Identity.Domain.Extensions;
using EsperancaSolidaria.Identity.Domain.Repositories;
using EsperancaSolidaria.Identity.Domain.Repositories.Token;
using EsperancaSolidaria.Identity.Domain.Repositories.User;
using EsperancaSolidaria.Identity.Domain.Security.Criptography;
using EsperancaSolidaria.Identity.Domain.Security.Tokens;
using EsperancaSolidaria.Identity.Exceptions;
using EsperancaSolidaria.Identity.Exceptions.ExceptionsBase;
using FluentValidation.Results;
using Mapster;

namespace EsperancaSolidaria.Identity.Application.UseCases.User.Register
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IUserWriteOnlyRepository _writeOnlyRepository;
        private readonly IUserReadOnlyRepository _readOnlyRepository;
        private readonly IPasswordEncripter _passwordEncripter;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly ITokenRepository _tokenRepository;
        private readonly IAccessTokenGenerator _accessTokenGenerator;

        public RegisterUserUseCase(IUserWriteOnlyRepository writeOnlyRepository, IUserReadOnlyRepository readOnlyRepository, IPasswordEncripter passwordEncripter, IUnitOfWork unitOfWork, IRefreshTokenGenerator refreshTokenGenerator, ITokenRepository tokenRepository, IAccessTokenGenerator accessTokenGenerator)
        {
            _writeOnlyRepository = writeOnlyRepository;
            _readOnlyRepository = readOnlyRepository;
            _passwordEncripter = passwordEncripter;
            _unitOfWork = unitOfWork;
            _refreshTokenGenerator = refreshTokenGenerator;
            _tokenRepository = tokenRepository;
            _accessTokenGenerator = accessTokenGenerator;
        }

        public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
        {
            //Normalizar a request
            NormalizeRequest(request);

            // Validar a request
            await Validate(request);            

            // Mapear a request em uma entidade
            var user = request.Adapt<Domain.Entities.User>();

            // Criptografia da senha
            user.Password = _passwordEncripter.Encrypt(request.Password);

            // Salvar no BD
            await _writeOnlyRepository.Add(user);
            await _unitOfWork.Commit();

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

        private async Task Validate(RequestRegisterUserJson request)
        {
            var validator = new RegisterUserValidator();
            var result = await validator.ValidateAsync(request);
            var emailExist = await _readOnlyRepository.ExistActiveUserWithEmail(request.Email);
            var cpfExist = await _readOnlyRepository.ExistActiveUserWithCPF(request.CPF);

            if(emailExist)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
            }

            if(cpfExist)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.CPF_ALREADY_REGISTERED));
            }

            if(result.IsValid.IsFalse())
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }

        private async Task<string> CreateAndSaveRefreshToken(Domain.Entities.User user)
        {
            var refreshToken = _refreshTokenGenerator.Generate();

            await _tokenRepository.SaveNewRefreshToken(new RefreshToken
            {
                Value = refreshToken,
                UserId = user.Id
            });

            await _unitOfWork.Commit();

            return refreshToken;
        }

        private void NormalizeRequest(RequestRegisterUserJson request)
        {
            request.CPF = request.CPF.Trim().Replace(".", "").Replace("-", "");
            request.Email = request.Email.Trim().ToLower();
            request.Name = request.Name.Trim();
        }
    }
}
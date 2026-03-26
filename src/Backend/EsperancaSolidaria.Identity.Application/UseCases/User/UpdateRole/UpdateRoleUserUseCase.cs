using EsperancaSolidaria.Identity.Communication.Requests;
using EsperancaSolidaria.Identity.Domain.Enums;
using EsperancaSolidaria.Identity.Domain.Repositories;
using EsperancaSolidaria.Identity.Domain.Repositories.User;
using EsperancaSolidaria.Identity.Domain.Services.LoggedUser;
using EsperancaSolidaria.Identity.Exceptions;
using EsperancaSolidaria.Identity.Exceptions.ExceptionsBase;
using FluentValidation.Results;

namespace EsperancaSolidaria.Identity.Application.UseCases.User.UpdateRole
{
    public class UpdateRoleUserUseCase : IUpdateRoleUserUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IUserUpdateRoleOnlyRepository _repository;
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateRoleUserUseCase(ILoggedUser loggedUser, IUserUpdateRoleOnlyRepository repository, IUserReadOnlyRepository userReadOnlyRepository, IUnitOfWork unitOfWork)
        {
            _loggedUser = loggedUser;
            _repository = repository;
            _userReadOnlyRepository = userReadOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(RequestUpdateRoleUserJson request)
        {
            var loggedUser = await _loggedUser.User();
            var targetUser = await _userReadOnlyRepository.GetByEmail(request.Email);

            if(targetUser is null)
            {
                throw new NotFoundException(ResourceMessagesException.USER_NOT_FOUND);
            }

            if(loggedUser.Id == targetUser.Id)
            {
                throw new UnauthorizedException(ResourceMessagesException.CANNOT_CHANGE_OWN_ROLE);
            }

            await Validate(request, targetUser);

            targetUser.Role = (UserRole)request.Role;

            _repository.Update(targetUser);
            await _unitOfWork.Commit();
        }

        private async Task Validate(RequestUpdateRoleUserJson request, Domain.Entities.User targetUser)
        {
            var validator = new UpdateRoleUserValidator();
            var result = await validator.ValidateAsync(request);

            if(targetUser.Role == (UserRole)request.Role)
            {
                result.Errors.Add(new ValidationFailure("role", ResourceMessagesException.ROLE_ALREADY_REGISTERED));
            }

            if(result.IsValid is false)
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
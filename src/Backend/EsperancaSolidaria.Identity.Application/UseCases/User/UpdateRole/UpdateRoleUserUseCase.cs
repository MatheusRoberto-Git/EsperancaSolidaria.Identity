using EsperancaSolidaria.Identity.Communication.Requests;
using EsperancaSolidaria.Identity.Domain.Enums;
using EsperancaSolidaria.Identity.Domain.Repositories;
using EsperancaSolidaria.Identity.Domain.Repositories.User;
using EsperancaSolidaria.Identity.Exceptions;
using EsperancaSolidaria.Identity.Exceptions.ExceptionsBase;
using FluentValidation.Results;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace EsperancaSolidaria.Identity.Application.UseCases.User.UpdateRole
{
    public class UpdateRoleUserUseCase : IUpdateRoleUserUseCase
    {
        private readonly IUserUpdateRoleOnlyRepository _repository;
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateRoleUserUseCase(IUserUpdateRoleOnlyRepository repository, IUserReadOnlyRepository userReadOnlyRepository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _userReadOnlyRepository = userReadOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(RequestUpdateRoleUserJson request)
        {
            var targetUser = await _userReadOnlyRepository.GetByEmail(request.Email);

            if(targetUser is null)
            {
                throw new NotFoundException(ResourceMessagesException.USER_NOT_FOUND);
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
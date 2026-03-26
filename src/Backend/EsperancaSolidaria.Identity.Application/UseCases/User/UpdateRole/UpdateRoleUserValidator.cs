using EsperancaSolidaria.Identity.Communication.Requests;
using EsperancaSolidaria.Identity.Exceptions;
using FluentValidation;

namespace EsperancaSolidaria.Identity.Application.UseCases.User.UpdateRole
{
    public class UpdateRoleUserValidator : AbstractValidator<RequestUpdateRoleUserJson>
    {
        public UpdateRoleUserValidator()
        {
            RuleFor(request => request.Email).NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY);
            RuleFor(request => request.Role).NotEmpty().WithMessage(ResourceMessagesException.ROLE_EMPTY);
        }
    }
}
using EsperancaSolidaria.Identity.Application.SharedValidators;
using EsperancaSolidaria.Identity.Communication.Requests;
using EsperancaSolidaria.Identity.Exceptions;
using FluentValidation;

namespace EsperancaSolidaria.Identity.Application.UseCases.User.Register
{
    public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
    {
        public RegisterUserValidator()
        {
            RuleFor(user => user.Name)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.NAME_EMPTY);

            RuleFor(user => user.Email)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.EMAIL_EMPTY);
            When(user => !string.IsNullOrEmpty(user.Email), () =>
            {
                RuleFor(user => user.Email)
                    .EmailAddress()
                    .WithMessage(ResourceMessagesException.EMAIL_ADDRESS);
            });

            RuleFor(user => user.CPF)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.CPF_EMPTY);
            When(user => !string.IsNullOrEmpty(user.CPF), () =>
            {
                RuleFor(user => user.CPF)
                    .IsValidCPF()
                    .WithMessage(ResourceMessagesException.CPF_FORMAT);
            });

            RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestRegisterUserJson>());
        }
    }
}
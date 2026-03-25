using EsperancaSolidaria.Identity.Exceptions;
using FluentValidation;
using FluentValidation.Validators;

namespace EsperancaSolidaria.Identity.Application.SharedValidators
{
    public class PasswordValidator<T> : PropertyValidator<T, string>
    {
        public override bool IsValid(ValidationContext<T> context, string password)
        {
            if(string.IsNullOrWhiteSpace(password))
            {
                context.MessageFormatter.AppendArgument("ErrorMessage", ResourceMessagesException.PASSWORD_EMPTY);

                return false;
            }

            if(password.Length < 8)
            {
                context.MessageFormatter.AppendArgument("ErrorMessage", ResourceMessagesException.PASSWORD_LENGHT);

                return false;
            }

            if(password.Count(char.IsDigit) < 1)
            {
                context.MessageFormatter.AppendArgument("ErrorMessage", ResourceMessagesException.PASSWORD_FORMAT);

                return false;
            }            

            if(password.Count(char.IsUpper) < 1)
            {
                context.MessageFormatter.AppendArgument("ErrorMessage", ResourceMessagesException.PASSWORD_FORMAT);

                return false;
            }

            if(password.Count(char.IsLower) < 1)
            {
                context.MessageFormatter.AppendArgument("ErrorMessage", ResourceMessagesException.PASSWORD_FORMAT);

                return false;
            }

            if(!password.Any(c => char.IsSymbol(c) || char.IsPunctuation(c)))
            {
                context.MessageFormatter.AppendArgument("ErrorMessage", ResourceMessagesException.PASSWORD_FORMAT);

                return false;
            }

            return true;
        }

        public override string Name => "PasswordValidator";

        protected override string GetDefaultMessageTemplate(string errorCode) => "{ErrorMessage}";
    }
}
using FluentValidation;

namespace Application.Features.Client.Commands.CreateClientCommand
{
    public sealed class CreateClientValidator : AbstractValidator<CreateClientRequest>
    {
        public CreateClientValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName cannot be empty.");

            RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName cannot be empty.");

            RuleFor(x => x.Email).NotEmpty().WithMessage("Email cannot be empty.")
                                 .EmailAddress().WithMessage("Invalid email address.");

            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("PhoneNumber cannot be empty.");
        }
    }
}

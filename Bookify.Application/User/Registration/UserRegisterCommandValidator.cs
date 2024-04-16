using FluentValidation;

namespace Bookify.Application.User.Registration;

internal class UserRegisterCommandValidator : AbstractValidator<UserRegisterCommand>
{
    public UserRegisterCommandValidator()
    {
        //Take a look if can query DB for validation?
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(10);
    }
}
using FluentValidation;
using JobPortalApi.DTOs;

namespace JobPortalApi.Validators;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(100);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6).MaximumLength(64);
        RuleFor(x => x.Role).Must(r => r is "User" or "Recruiter")
            .WithMessage("Role must be 'User' or 'Recruiter'.");
    }
}

using FluentValidation;
using JobPortalApi.DTOs;

namespace JobPortalApi.Validators;

public class CreateJobDtoValidator : AbstractValidator<CreateJobDto>
{
    public CreateJobDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(120);
        RuleFor(x => x.Company).NotEmpty().MaximumLength(120);
        RuleFor(x => x.Location).NotEmpty().MaximumLength(120);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.SalaryMin).GreaterThanOrEqualTo(0);
        RuleFor(x => x.SalaryMax).GreaterThanOrEqualTo(x => x.SalaryMin);
    }
}

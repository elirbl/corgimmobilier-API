using FluentValidation;
using YmmoApi.Dtos.Agencies;

namespace YmmoApi.Validation;

public class AgencyCreateDtoValidator : AbstractValidator<AgencyCreateDto>
{
    public AgencyCreateDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.City).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Address).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Phone).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(150);
    }
}

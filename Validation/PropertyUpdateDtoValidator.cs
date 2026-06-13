using FluentValidation;
using YmmoApi.Dtos.Properties;

namespace YmmoApi.Validation;

public class PropertyUpdateDtoValidator : AbstractValidator<PropertyUpdateDto>
{
    public PropertyUpdateDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(4000);
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.Status).IsInEnum();
        RuleFor(x => x.AgencyId).GreaterThan(0);
        RuleFor(x => x.City).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Bedrooms).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Area).GreaterThan(0);
        RuleFor(x => x.DpeRating).IsInEnum().When(x => x.DpeRating.HasValue);
        RuleFor(x => x.AgentId).GreaterThan(0).When(x => x.AgentId.HasValue);
    }
}

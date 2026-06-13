using FluentValidation;
using YmmoApi.Dtos.Properties;

namespace YmmoApi.Validation;

public class PropertyStatusUpdateDtoValidator : AbstractValidator<PropertyStatusUpdateDto>
{
    public PropertyStatusUpdateDtoValidator()
    {
        RuleFor(x => x.Status).IsInEnum();
    }
}

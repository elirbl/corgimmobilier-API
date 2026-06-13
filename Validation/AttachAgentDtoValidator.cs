using FluentValidation;
using YmmoApi.Dtos.Agencies;

namespace YmmoApi.Validation;

public class AttachAgentDtoValidator : AbstractValidator<AttachAgentDto>
{
    public AttachAgentDtoValidator()
    {
        RuleFor(x => x.AgentId).GreaterThan(0);
    }
}

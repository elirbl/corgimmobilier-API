using FluentValidation;
using YmmoApi.Dtos.Visits;

namespace YmmoApi.Validation;

public class VisitCreateDtoValidator : AbstractValidator<VisitCreateDto>
{
    public VisitCreateDtoValidator()
    {
        RuleFor(x => x.PropertyId).GreaterThan(0);
        RuleFor(x => x.AgentId).GreaterThan(0).When(x => x.AgentId.HasValue);
        RuleFor(x => x.ScheduledAt).GreaterThan(DateTime.UtcNow).WithMessage("La date de visite doit être dans le futur.");
        RuleFor(x => x.Notes).MaximumLength(1000);
    }
}

using FluentValidation;
using YmmoApi.Dtos.Visits;
using YmmoApi.Models;

namespace YmmoApi.Validation;

public class VisitStatusUpdateDtoValidator : AbstractValidator<VisitStatusUpdateDto>
{
    public VisitStatusUpdateDtoValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum()
            .Must(s => s is VisitStatus.Completed or VisitStatus.Cancelled)
            .WithMessage("Le statut doit être 'Completed' ou 'Cancelled'.");
    }
}

using FluentValidation;
using YmmoApi.Dtos.Transactions;

namespace YmmoApi.Validation;

public class TransactionStageUpdateDtoValidator : AbstractValidator<TransactionStageUpdateDto>
{
    public TransactionStageUpdateDtoValidator()
    {
        RuleFor(x => x.Notes).MaximumLength(1000);
    }
}

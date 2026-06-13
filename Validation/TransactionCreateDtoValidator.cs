using FluentValidation;
using YmmoApi.Dtos.Transactions;

namespace YmmoApi.Validation;

public class TransactionCreateDtoValidator : AbstractValidator<TransactionCreateDto>
{
    public TransactionCreateDtoValidator()
    {
        RuleFor(x => x.PropertyId).GreaterThan(0);
        RuleFor(x => x.Notes).MaximumLength(1000);
    }
}

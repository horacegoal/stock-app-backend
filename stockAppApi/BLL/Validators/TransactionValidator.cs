using FluentValidation;
using stockAppApi.Models.Api.Request;

namespace stockAppApi.BLL.Validators
{
    public class AddTransactionValidator : AbstractValidator<AddTransactionRequest>
    {
        public AddTransactionValidator()
        {
            RuleFor(x => x.StockId).NotEmpty();
            RuleFor(x => x.Type).NotEmpty();
            RuleFor(x => x.Quantity).NotEmpty();
            RuleFor(x => x.Price).NotEmpty();
            RuleFor(x => x.TransactionDate).NotEmpty();
        }
    }
}

using System;
using FluentValidation;
using stockAppApi.Entities;
using stockAppApi.Models.Api.Request;

namespace stockAppApi.BLL.Validators
{
    public class UpsertStockValidator : AbstractValidator<UpsertStockRequest>
    {
        public UpsertStockValidator()
        {
            RuleFor(x => x.Symbol).NotEmpty().WithMessage("Symbol is required.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Price is required.").GreaterThan(0).WithMessage("Price must be greater than 0.");
            // RuleFor(x => x.Date).NotEmpty().WithMessage("Date is required.").LessThan(DateTime.Now).WithMessage("Date must be less than today.");
            RuleFor(x => x.MarketCap).NotEmpty().WithMessage("MarketCap is required.").WithMessage("MarketCap is required");
            RuleFor(x => x.PERatio).NotEmpty().WithMessage("PERatio is required.").WithMessage("PERatio is required");
            RuleFor(x => x.Volume).NotEmpty().WithMessage("Volume is required.").WithMessage("Volume is required");
            RuleFor(x => x.PercentageChange).NotEmpty().WithMessage("PercentageChange is required.").WithMessage("PercentageChange is required");
            RuleFor(x => x.ShareFloat).NotEmpty().WithMessage("ShareFloat is required.").WithMessage("ShareFloat is required");



        }
    }
}

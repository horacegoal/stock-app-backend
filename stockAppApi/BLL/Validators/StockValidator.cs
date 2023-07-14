using System;
using FluentValidation;
using stockAppApi.Entities;
using stockAppApi.Models.Api.Request;

namespace stockAppApi.BLL.Validators
{
    public class InsertStockValidator : AbstractValidator<InsertStockRequest>
    {
        public InsertStockValidator()
        {
            RuleFor(x => x.Symbol).NotEmpty().WithMessage("Symbol is required.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Date).NotEmpty().WithMessage("Date is required.");
            RuleFor(x => x.Open).NotEmpty().WithMessage("Open is required.").GreaterThan(0).WithMessage("Open must be greater than 0.");
            RuleFor(x => x.High).NotEmpty().WithMessage("High is required.").GreaterThan(0).WithMessage("High must be greater than 0.");
            RuleFor(x => x.Low).NotEmpty().WithMessage("Low is required.").GreaterThan(0).WithMessage("Low must be greater than 0.");
            RuleFor(x => x.Close).NotEmpty().WithMessage("Close is required.").GreaterThan(0).WithMessage("Close must be greater than 0.");
            RuleFor(x => x.Volume).NotEmpty().WithMessage("Volume is required.").GreaterThan(0).WithMessage("Volume must be greater than 0.");
        }
    }
}

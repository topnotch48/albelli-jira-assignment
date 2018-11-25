using Albelli.Orders.Contracts.Models;
using FluentValidation;

namespace Albelli.Common.Web.Validators
{
    public class OrderIdQueryValidator : AbstractValidator<OrderIdQuery>
    {
        public OrderIdQueryValidator()
        {
            RuleFor(query => query.OrderId)
                .NotEmpty()
                .Matches(OrderConstants.OrderId.ValueFormat)
                .WithMessage("Invalid OrderId Format.");
        }
    }
}

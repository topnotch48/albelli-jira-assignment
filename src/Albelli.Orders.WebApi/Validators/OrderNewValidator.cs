using Albelli.Orders.Contracts;
using Albelli.Orders.Contracts.Models;
using FluentValidation;

namespace Albelli.Orders.WebApi.Validators
{
	public class OrderNewValidator : AbstractValidator<OrderNew>
	{
		public OrderNewValidator()
		{
			RuleFor(order => order.OrderId)
				.NotEmpty()
				.Matches(OrderConstants.OrderId.ValueFormat)
				.WithMessage("Invalid OrderId Format.");

			RuleFor(order => order.CustomerName)
				.NotEmpty()
				.MaximumLength(OrderConstants.CustomerName.MaxLength)
				.MinimumLength(OrderConstants.CustomerName.MinLength)
				.WithMessage("Invalid CustomerName.");

			RuleFor(order => order.HouseNumber)
				.NotEmpty()
				.InclusiveBetween(OrderConstants.HouseNumber.Min, OrderConstants.HouseNumber.Max)
				.WithMessage("Invalid HouseNumber.");

			RuleFor(order => order.Price)
				.NotEmpty()
				.GreaterThan(0m)
				.WithMessage("Invalid Price.");

			RuleFor(order => order.PostCode)
				.NotEmpty()
			    .Matches(OrderConstants.PostCode.ValueFormat)
				.WithMessage("Invalid PostCode.");
		}
	}
}

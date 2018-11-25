using Albelli.Orders.Contracts;
using Albelli.Orders.Contracts.Models;
using FluentValidation;

namespace Albelli.Orders.WebApi.Validators
{
	public class OrderUpdateValidator : AbstractValidator<OrderUpdate>
	{
		public OrderUpdateValidator()
		{
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

			RuleFor(order => order.RowVersion)
				.NotEmpty()
				.WithMessage("Indalid RowVersion.");
		}
	}
}

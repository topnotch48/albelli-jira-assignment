using System;
using System.Text.RegularExpressions;
using Albelli.Orders.Contracts.Properties;

namespace Albelli.Orders.Contracts.Models
{
    public class OrderId
    {
	    public string Value { get; }

		public OrderId(string orderIdValue)
	    {
			if (string.IsNullOrEmpty(orderIdValue))
				throw new ArgumentNullException(nameof(orderIdValue));

		    if(!IsValidOrderId(orderIdValue))
				throw new ArgumentException(string.Format(Resources.InvalidOrderIdFormat, OrderConstants.OrderId.ValueFormat), nameof(orderIdValue));

		    this.Value = orderIdValue;
	    }

	    public override string ToString()
	    {
			return this.Value;
	    }

	    public static OrderId FromString(string orderId)
	    {
			return new OrderId(orderId);
	    }

	    public static bool IsValidOrderId(string orderIdValue) => Regex.IsMatch(orderIdValue, OrderConstants.OrderId.ValueFormat);
	}
}

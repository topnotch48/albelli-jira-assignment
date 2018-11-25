using System;
using Albelli.Orders.Persistence.Contracts.Properties;

namespace Albelli.Orders.Persistence.Contracts
{
	public class OrderExistsException : OrderAwareException
	{
		public OrderExistsException(string orderId, Exception ex) : base(string.Format(Resources.OrderExists, orderId), ex)
		{
			this.OrderId = orderId;
		}

		public OrderExistsException(string orderId) : base(string.Format(Resources.OrderExists, orderId))
		{
			this.OrderId = orderId;
		}
	}
}

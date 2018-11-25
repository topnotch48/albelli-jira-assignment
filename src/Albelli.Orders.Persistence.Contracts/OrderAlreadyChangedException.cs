using System;
using Albelli.Orders.Persistence.Contracts.Properties;

namespace Albelli.Orders.Persistence.Contracts
{
	public class OrderAlreadyChangedException : OrderAwareException
	{
		public OrderAlreadyChangedException(string orderId, Exception ex) : base(string.Format(Resources.OrderChanged, orderId), ex)
		{
			this.OrderId = orderId;
		}

		public OrderAlreadyChangedException(string orderId) : base(string.Format(Resources.OrderChanged, orderId))
		{
			this.OrderId = orderId;
		}
	}
}

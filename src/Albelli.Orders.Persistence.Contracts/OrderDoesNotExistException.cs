using System;
using Albelli.Orders.Persistence.Contracts.Properties;

namespace Albelli.Orders.Persistence.Contracts
{
    public class OrderDoesNotExistException : OrderAwareException
	{ 
		public OrderDoesNotExistException(string orderId, Exception ex) : base(string.Format(Resources.OrderDoesNotExist, orderId), ex)
		{
			this.OrderId = orderId;
		}

		public OrderDoesNotExistException(string orderId) : base(string.Format(Resources.OrderDoesNotExist, orderId))
		{
			this.OrderId = orderId;
		}
	}
}

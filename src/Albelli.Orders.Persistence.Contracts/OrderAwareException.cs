using System;
using Albelli.Core.Contracts;

namespace Albelli.Orders.Persistence.Contracts
{
	public abstract class OrderAwareException : ExposableApiException
	{
		public string OrderId { get; set; }

		protected OrderAwareException(string reason) : base(reason)
		{
		}

		protected OrderAwareException(string reason, Exception ex) : base(reason, ex)
		{
		}
	}
}

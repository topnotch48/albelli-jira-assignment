using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Albelli.Orders.Contracts;
using Albelli.Orders.Contracts.Models;
using Xunit;
using Xunit.Sdk;

namespace Albelli.Orders.Tests.Integration.Orders.Data
{
	public class InvalidOrderUpdates : DataAttribute
	{
		public override IEnumerable<object[]> GetData(MethodInfo testMethod)
		{
			var validOrder = OrdersSeed.Orders.Value.FirstOrDefault();

			Assert.NotNull(validOrder);

			var orders = new[]
			{
				new [] { OrderUpdate.From(validOrder, order => { order.CustomerName = new String('A', OrderConstants.CustomerName.MinLength - 1); }) },
				new [] { OrderUpdate.From(validOrder, order => { order.CustomerName = new String('A', OrderConstants.CustomerName.MaxLength + 1); }) },
				new [] { OrderUpdate.From(validOrder, order => { order.PostCode = "24124124"; }) },
				new [] { OrderUpdate.From(validOrder, order => { order.HouseNumber = 0; }) },
				new [] { OrderUpdate.From(validOrder, order => { order.Price = 0m; }) },
				new [] { OrderUpdate.From(validOrder, order => { order.RowVersion = new byte[] { }; }) },
				new IOrderUpdate[] { new OrderUpdate() },
			};

			return orders;
		}
	}
}
using System;
using System.Collections.Generic;
using System.IO;
using Albelli.Orders.Contracts.Models;
using Newtonsoft.Json;
using Xunit;

namespace Albelli.Orders.Tests.Integration.Orders
{
    public class OrdersSeed
    {
		public static Lazy<IList<Order>> Orders = new Lazy<IList<Order>>(() =>
		{
			var ordersFile = Path.Combine(Directory.GetCurrentDirectory(), "orders.json");

			Assert.True(File.Exists(ordersFile), "Missing orders seed file.");

			var content = File.ReadAllText(ordersFile);

			var orders = JsonConvert.DeserializeObject<IList<Order>>(content);

			return orders;
		}, isThreadSafe: true);
    }
}

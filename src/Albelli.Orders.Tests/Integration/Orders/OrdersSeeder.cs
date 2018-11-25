using System;
using System.Linq;
using Albelli.Orders.Persistence;
using Albelli.Orders.Persistence.Enitities;

namespace Albelli.Orders.Tests.Integration.Orders
{
	public class OrdersSeeder
    {
	    private readonly OrdersContext _context;

	    public OrdersSeeder(OrdersContext context)
	    {
		    this._context = context ?? throw new ArgumentNullException(nameof(context));
	    }

	    public void Seed()
	    {
			var orders = OrdersSeed.Orders.Value;

		    var entitites = orders.Select(order => new Order
		    {
				OrderId = order.OrderId,
				RowVersion = order.RowVersion,
				CustomerName = order.CustomerName,
				HouseNumber = (int)order.HouseNumber,
				PostCode = order.PostCode,
				Price = order.Price
		    });

			this._context.Orders.AddRange(entitites);

			this._context.SaveChanges();
		}
    }
}

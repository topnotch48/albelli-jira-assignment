using Albelli.Orders.Persistence.Enitities;
using Microsoft.EntityFrameworkCore;

namespace Albelli.Orders.Persistence
{
    public class OrdersContext : DbContext
    {
	    public OrdersContext()
	    {
	    }

		public OrdersContext(DbContextOptions<OrdersContext> options) : base(options)
		{
		}

		public DbSet<Order> Orders { get; set; }
	}
}

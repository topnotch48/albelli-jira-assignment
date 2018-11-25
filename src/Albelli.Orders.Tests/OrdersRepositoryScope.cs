using System;
using Albelli.Orders.Persistence;
using Albelli.Orders.Persistence.Contracts;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Albelli.Orders.Tests
{
    public class OrdersRepositoryScope : IDisposable
    {
		public IOrdersRepository Repository { get; }

	    private readonly OrdersContext _context;

	    public OrdersRepositoryScope(DbContextOptions<OrdersContext> options, IMapper mapper)
	    {
			this._context = new OrdersContext(options);
			this.Repository = new OrdersRepository(this._context, mapper);
	    }

	    public void Dispose()
	    {
			this._context?.Dispose();
	    }
    }
}

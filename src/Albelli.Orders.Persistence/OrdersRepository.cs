using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Albelli.Core.Contracts;
using Albelli.Orders.Contracts;
using Albelli.Orders.Contracts.Models;
using Albelli.Orders.Persistence.Contracts;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Order = Albelli.Orders.Persistence.Enitities.Order;

namespace Albelli.Orders.Persistence
{
	public class OrdersRepository : IOrdersRepository
	{
	    private readonly OrdersContext _dbContext;
		private readonly IMapper _mapper;

	    public OrdersRepository(OrdersContext dbContext, IMapper mapper)
	    {
		    this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		    this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
	    }

	    public async Task<Orders.Contracts.Models.Order> Create(OrderNew newOrder)
	    {
		    if (newOrder == null)
			    throw new ArgumentNullException(nameof(newOrder));

		    var order = this._mapper.Map<Order>(newOrder);

		    var entry = await this._dbContext.Orders.AddAsync(order);

		    try
		    {
			    await this._dbContext.SaveChangesAsync();
		    }
		    catch (ArgumentException ex)
		    {
			    throw new OrderExistsException(order.OrderId, ex);
			}
		    catch (DbUpdateException ex)
		    {
			    throw new OrderExistsException(order.OrderId, ex);
		    }

            return this._mapper.Map<Orders.Contracts.Models.Order>(entry.Entity);
	    }

	    public async Task Delete(OrderId orderId)
	    {
		    if (orderId == null)
			    throw new ArgumentNullException(nameof(orderId));

		    var order = await GetOrder(orderId);

		    if (order == null)
				throw new OrderDoesNotExistException(orderId.Value);

		    this._dbContext.Orders.Remove(order);

		    await this._dbContext.SaveChangesAsync();
	    }

	    public async Task<PagingResult<Orders.Contracts.Models.Order>> Get(Paging paging)
	    {
			if (paging == null)
				throw new ArgumentNullException(nameof(paging));

		    uint? count = null;
		    uint skip = paging.Skip;
		    uint take = paging.Take;

			if (paging.IncludeTotalCnt)
				count = (uint) await this._dbContext.Orders.CountAsync();

		    var items = await this._dbContext.Orders
			    .Skip((int)skip)
			    .Take((int)take)
			    .ToListAsync();

		    var orders = this._mapper.Map<IList<Orders.Contracts.Models.Order>>(items);

			return new PagingResult<Orders.Contracts.Models.Order>(skip, take, orders, count);
		}

	    public async Task<Orders.Contracts.Models.Order> Get(OrderId orderId)
	    {
		    if (orderId == null)
			    throw new ArgumentNullException(nameof(orderId));

		    var order = await this.GetOrder(orderId);

		    return this._mapper.Map<Orders.Contracts.Models.Order>(order);
	    }

	    public async Task<Orders.Contracts.Models.Order> Update(OrderId orderId, OrderUpdate orderUpdate)
	    {
		    if (orderId == null)
			    throw new ArgumentNullException(nameof(orderId));

		    if (orderUpdate == null)
			    throw new ArgumentNullException(nameof(orderUpdate));

		    var entity = await GetOrder(orderId);

		    if (entity == null)
			    throw new OrderDoesNotExistException(orderId.Value);

		    this._dbContext.Entry(entity).Property("RowVersion").OriginalValue = orderUpdate.RowVersion;

		    var order = this._mapper.Map(orderUpdate, entity);

			this._dbContext.Orders.Update(order);

		    try
		    {
			    await this._dbContext.SaveChangesAsync();
			}
		    catch (DbUpdateConcurrencyException ex)
		    {
			    throw new OrderAlreadyChangedException(orderId.Value, ex);
		    }
		    
		    return this._mapper.Map<Orders.Contracts.Models.Order>(order);
	    }

		protected async Task<Order> GetOrder(OrderId orderId)
		{
			if (orderId == null)
				throw new ArgumentNullException(nameof(orderId));

			var order = await this._dbContext.Orders
				.AsNoTracking()
				.SingleOrDefaultAsync(o => o.OrderId == orderId.ToString());

			return order;
		}
	}
}

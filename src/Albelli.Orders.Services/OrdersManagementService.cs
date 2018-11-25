using System;
using System.Threading.Tasks;
using Albelli.Core.Contracts;
using Albelli.Orders.Contracts;
using Albelli.Orders.Contracts.Models;
using Albelli.Orders.Persistence.Contracts;

namespace Albelli.Orders.Services
{
	public class OrdersManagementService : IOrdersManagementService
	{
	    private readonly IOrdersRepository _ordersRepository;

	    public OrdersManagementService(IOrdersRepository ordersRepository)
	    {
		    this._ordersRepository = ordersRepository ?? throw new ArgumentNullException(nameof(ordersRepository));
	    }

	    public async Task<Order> Create(OrderNew order)
	    {
			if (order == null)
				throw new ArgumentNullException(nameof(order));

		    var created = await this._ordersRepository.Create(order);
		    return created;
	    }

	    public async Task Delete(OrderId orderId)
	    {
		    if (orderId == null)
			    throw new ArgumentNullException(nameof(orderId));

			await this._ordersRepository.Delete(orderId);
	    }

	    public async Task<PagingResult<Order>> Get(Paging paging)
	    {
		    if (paging == null)
			    throw new ArgumentNullException(nameof(paging));

		    var pagingResult = await this._ordersRepository.Get(paging);
		    return pagingResult;
	    }

	    public async Task<Order> Get(OrderId orderId)
	    {
		    if (orderId == null)
			    throw new ArgumentNullException(nameof(orderId));

			var order = await this._ordersRepository.Get(orderId);
		    return order;
	    }

	    public async Task<Order> Update(OrderId orderId, OrderUpdate order)
	    {
		    if (orderId == null)
			    throw new ArgumentNullException(nameof(orderId));

		    if (order == null)
			    throw new ArgumentNullException(nameof(order));

			var updated = await this._ordersRepository.Update(orderId, order);
		    return updated;
	    }
	}
}

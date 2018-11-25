using System.Threading.Tasks;
using Albelli.Core.Contracts;
using Albelli.Orders.Contracts.Models;

namespace Albelli.Orders.Persistence.Contracts
{
	public interface IOrdersRepository
	{
		Task<Order> Create(OrderNew newOrder);
		Task Delete(OrderId orderId);
		Task<PagingResult<Order>> Get(Paging paging = null);
		Task<Order> Get(OrderId orderId);
		Task<Order> Update(OrderId orderId, OrderUpdate order);
	}
}
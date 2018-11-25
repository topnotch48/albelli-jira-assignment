using System.Threading.Tasks;
using Albelli.Core.Contracts;
using Albelli.Orders.Contracts;
using Albelli.Orders.Contracts.Models;

namespace Albelli.Orders.Services
{
	public interface IOrdersManagementService
	{
		Task<Order> Create(OrderNew order);
		Task Delete(OrderId orderId);
		Task<PagingResult<Order>> Get(Paging paging);
		Task<Order> Get(OrderId orderId);
		Task<Order> Update(OrderId orderId, OrderUpdate order);
	}
}
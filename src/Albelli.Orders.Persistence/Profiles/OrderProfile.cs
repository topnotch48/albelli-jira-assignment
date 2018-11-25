using Albelli.Orders.Contracts.Models;
using AutoMapper;
using Order = Albelli.Orders.Persistence.Enitities.Order;

namespace Albelli.Orders.Persistence.Profiles
{
	public class OrderProfile : Profile
	{
		public OrderProfile()
		{
			CreateMap<Order, Orders.Contracts.Models.Order>();

			CreateMap<OrderNew, Order>()
				.ForMember(order => order.RowVersion, opt => opt.Ignore());

			CreateMap<OrderUpdate, Order>()
				.ForMember(order => order.OrderId, opt => opt.Ignore());
		}
	}
}

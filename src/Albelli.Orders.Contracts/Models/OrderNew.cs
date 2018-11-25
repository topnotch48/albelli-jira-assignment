using System;

namespace Albelli.Orders.Contracts.Models
{
    public class OrderNew : IOrderNew
    {
	    public string OrderId { get; set; }

	    public decimal Price { get; set; }

	    public string CustomerName { get; set; }

	    public string PostCode { get; set; }

	    public uint HouseNumber { get; set; }


	    public static OrderNew From(IOrderNew source, Action<IOrderNew> modifyFn = null)
	    {
		    var copy = new OrderNew
			{
			    Price = source.Price,
			    CustomerName = source.CustomerName,
			    PostCode = source.PostCode,
			    HouseNumber = source.HouseNumber,
			    OrderId = source.OrderId
		    };

		    modifyFn?.Invoke(copy);

		    return copy;
	    }
	}
}

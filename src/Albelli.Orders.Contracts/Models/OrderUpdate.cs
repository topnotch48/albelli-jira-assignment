using System;

namespace Albelli.Orders.Contracts.Models
{
    public class OrderUpdate : IOrderUpdate
    {
	    public decimal Price { get; set; }

	    public string CustomerName { get; set; }

	    public string PostCode { get; set; }

	    public uint HouseNumber { get; set; }

	    public byte[] RowVersion { get; set; }


	    public static OrderUpdate From(IOrderUpdate source, Action<IOrderUpdate> modifyFn = null)
	    {
		    var copy = new OrderUpdate
		    {
			    Price = source.Price,
			    CustomerName = source.CustomerName,
			    PostCode = source.PostCode,
			    HouseNumber = source.HouseNumber,
			    RowVersion = source.RowVersion
		    };

			modifyFn?.Invoke(copy);

		    return copy;
	    }
	}
}

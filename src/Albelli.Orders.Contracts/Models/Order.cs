namespace Albelli.Orders.Contracts.Models
{
    public class Order : IOrderNew, IOrderUpdate
    {
	    public string OrderId { get; set; }

	    public decimal Price { get; set; }

	    public string CustomerName { get; set; }

	    public string PostCode { get; set; }

	    public uint HouseNumber { get; set; }

	    public byte[] RowVersion { get; set; }
	}
}

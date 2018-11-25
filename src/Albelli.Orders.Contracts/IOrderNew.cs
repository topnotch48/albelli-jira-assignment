namespace Albelli.Orders.Contracts
{
	public interface IOrderNew
	{
		string OrderId { get; set; }

		decimal Price { get; set; }

		string CustomerName { get; set; }

		string PostCode { get; set; }

		uint HouseNumber { get; set; }
	}
}
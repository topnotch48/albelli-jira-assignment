namespace Albelli.Orders.Contracts
{
	public interface IOrderUpdate
	{
		decimal Price { get; set; }

		string CustomerName { get; set; }

		string PostCode { get; set; }

		uint HouseNumber { get; set; }

		byte[] RowVersion { get; set; }
	}
}
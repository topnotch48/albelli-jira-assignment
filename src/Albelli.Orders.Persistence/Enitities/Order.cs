using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Albelli.Orders.Contracts;
using Albelli.Orders.Contracts.Models;

namespace Albelli.Orders.Persistence.Enitities
{
    public class Order
    {
		[Key]
		[RegularExpression(OrderConstants.OrderId.ValueFormat)]
		public string OrderId { get; set; }

	    [DataType(DataType.Currency)]
	    [Column(TypeName = "money")]
	    [Required]
		public decimal Price { get; set; }

	    [Required]
	    [StringLength(OrderConstants.CustomerName.MaxLength, MinimumLength = OrderConstants.CustomerName.MinLength)]
		public string CustomerName { get; set; }

	    [Required]
		[RegularExpression(OrderConstants.PostCode.ValueFormat)]
		public string PostCode { get; set; }

	    [Required]
		[Range(OrderConstants.HouseNumber.Min, OrderConstants.HouseNumber.Max)]
		public int HouseNumber { get; set; }

		[Timestamp]
		public byte[] RowVersion { get; set; }
	}
}

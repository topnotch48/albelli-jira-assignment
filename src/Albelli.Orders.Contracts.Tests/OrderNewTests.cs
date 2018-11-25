using Albelli.Orders.Contracts.Models;
using Xunit;

namespace Albelli.Orders.Contracts.Tests
{
    public class OrderNewTests
    {
		public OrderNew Source { get; }

	    public OrderNewTests()
	    {
		    Source = new OrderNew()
		    {
			    CustomerName = "Test Name",
			    HouseNumber = 1,
			    OrderId = "SAL123456",
			    PostCode = "1541UT",
			    Price = 20m
		    };
		}

	    [Fact]
	    public void OrderNew_Creates_A_Copy_Of_Provided_Source()
	    {
		    var copy = OrderNew.From(Source);

			Assert.True(copy != Source);
			Assert.Equal(copy.OrderId, Source.OrderId);
			Assert.Equal(copy.HouseNumber, Source.HouseNumber);
			Assert.Equal(copy.PostCode, Source.PostCode);
			Assert.Equal(copy.Price, Source.Price);
			Assert.Equal(copy.CustomerName, Source.CustomerName);
	    }

	    [Fact]
	    public void OrderNew_Creates_A_Copy_Of_Provided_Source_And_Can_Supply_A_Callback_To_Perform_Extra_Actions_On_Copy()
	    {
		    var updatedName = "Updated";

			var copy = OrderNew.From(Source, order => order.CustomerName = updatedName);

		    Assert.True(copy != Source);
		    Assert.Equal(copy.OrderId, Source.OrderId);
		    Assert.Equal(copy.HouseNumber, Source.HouseNumber);
		    Assert.Equal(copy.PostCode, Source.PostCode);
		    Assert.Equal(copy.Price, Source.Price);
		    Assert.NotEqual(copy.CustomerName, Source.CustomerName);
		    Assert.True(copy.CustomerName == updatedName);
	    }
	}
}

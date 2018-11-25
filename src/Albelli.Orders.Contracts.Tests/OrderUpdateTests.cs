using Albelli.Orders.Contracts.Models;
using Xunit;

namespace Albelli.Orders.Contracts.Tests
{
    public class OrderUpdateTests
    {
	    public OrderUpdate Source { get; }

	    public OrderUpdateTests()
	    {
		    Source = new OrderUpdate()
		    {
			    CustomerName = "Test Name",
			    HouseNumber = 1,
			    RowVersion = new byte[] {1, 1, 1, 1},
			    PostCode = "1541UT",
			    Price = 20m
		    };
	    }

	    [Fact]
	    public void OrderNew_Creates_A_Copy_Of_Provided_Source()
	    {
		    var copy = OrderUpdate.From(Source);

		    Assert.True(copy != Source);
		    Assert.Equal(copy.RowVersion, Source.RowVersion);
		    Assert.Equal(copy.HouseNumber, Source.HouseNumber);
		    Assert.Equal(copy.PostCode, Source.PostCode);
		    Assert.Equal(copy.Price, Source.Price);
		    Assert.Equal(copy.CustomerName, Source.CustomerName);
	    }

	    [Fact]
	    public void OrderNew_Creates_A_Copy_Of_Provided_Source_And_Can_Supply_A_Callback_To_Perform_Extra_Actions_On_Copy()
	    {
		    var updatedName = "Updated";

		    var copy = OrderUpdate.From(Source, order => order.CustomerName = updatedName);

		    Assert.True(copy != Source);
		    Assert.Equal(copy.RowVersion, Source.RowVersion);
		    Assert.Equal(copy.HouseNumber, Source.HouseNumber);
		    Assert.Equal(copy.PostCode, Source.PostCode);
		    Assert.Equal(copy.Price, Source.Price);
		    Assert.NotEqual(copy.CustomerName, Source.CustomerName);
		    Assert.True(copy.CustomerName == updatedName);
	    }
	}
}

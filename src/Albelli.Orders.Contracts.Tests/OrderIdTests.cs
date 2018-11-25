using System;
using Albelli.Orders.Contracts.Models;
using Xunit;

namespace Albelli.Orders.Contracts.Tests
{
    public class OrderIdTests
    {
		[Fact]
	    public void OrderId_Throws_ArgumentNullException_When_Attempt_To_Create_It_With_Empty_Value()
	    {
		    Assert.Throws<ArgumentNullException>(() => new OrderId(string.Empty));
	    }

	    [Theory]
		[InlineData("SALE231231")]
		[InlineData("SAL_2312321")]
		[InlineData("SAL231242424321")]
		[InlineData("SAL2312424243SA")]
		[InlineData("SAL")]
		[InlineData("213123")]
	    public void OrderId_Throws_ArgumentException_When_Attempt_To_Create_It_With_Invalid_Value(string orderIdValue)
	    {
		    Assert.Throws<ArgumentException>(() => new OrderId(orderIdValue));
	    }

		[Theory]
		[InlineData("SAL123456")]
		[InlineData("SAL1234567")]
		[InlineData("SAL12345678")]
		[InlineData("SAL123456789")]
		public void OrderId_Is_Accessable_Through_Value_Property_Once_Created_Successfully_Created(string orderIdValue)
	    {
		    var orderId = new OrderId(orderIdValue);
			Assert.True(orderId.Value == orderIdValue);
	    }
	}
}

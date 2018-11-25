using System;
using System.Linq;
using System.Threading.Tasks;
using Albelli.Core.Contracts;
using Albelli.Orders.Contracts.Models;
using Albelli.Orders.Persistence.Contracts;
using Albelli.Orders.Services;
using Albelli.Orders.Tests.Integration.Orders;
using Moq;
using Xunit;

namespace Albelli.Orders.Tests
{
    public class OrdersManagementServiceTests
    {
		public IOrdersManagementService Service { get; }
		public Mock<IOrdersRepository> Repository { get; }

	    public OrdersManagementServiceTests()
	    {
		    this.Repository = new Mock<IOrdersRepository>(MockBehavior.Loose);
		    this.Service = new OrdersManagementService(Repository.Object);
	    }

		[Fact]
	    public void Service_Requires_Instance_Of_IOrdersRepository_In_Order_To_Be_Created()
	    {
		    Assert.Throws<ArgumentNullException>(() => new OrdersManagementService(null));
		}

		[Fact]
	    public async Task Create_Method_Throws_If_No_Order_Specified()
		{
			await Assert.ThrowsAsync<ArgumentNullException>(() => Service.Create(null));
		}

		[Fact]
	    public async Task Create_Method_Calls_Create_Method_Of_IOrdersRepository_In_Order_To_Execute_A_Call()
	    {
		    var order = OrdersSeed.Orders.Value.FirstOrDefault();
		    Assert.NotNull(order);

		    var orderNew = OrderNew.From(order);

		    await Service.Create(orderNew);

			this.Repository.Verify(repository => repository.Create(orderNew), Times.Once);
			this.Repository.VerifyNoOtherCalls();
	    }

		[Fact]
	    public async Task Delete_Method_Throws_If_No_OrderId_Specified()
	    {
		    await Assert.ThrowsAsync<ArgumentNullException>(() => Service.Delete(null));
		}

		[Fact]
	    public async Task Delete_Method_Calls_Delete_Method_Of_IOrdersRepository_In_Order_To_Execute_A_Call()
	    {
		    var order = OrdersSeed.Orders.Value.FirstOrDefault();
		    Assert.NotNull(order);

		    var orderId = OrderId.FromString(order.OrderId);

		    await Service.Delete(orderId);

		    this.Repository.Verify(repository => repository.Delete(orderId), Times.Once);
		    this.Repository.VerifyNoOtherCalls();
		}

		[Fact]
	    public async Task PaginatedGet_Method_Throws_If_No_Paging_Were_Provided()
		{
		    await Assert.ThrowsAsync<ArgumentNullException>(() => Service.Get(paging: null));
		}

		[Fact]
	    public async Task PaginatedGet_Method_Calls_Get_Method_Of_IOrdersRepository_With_Paging_Options_In_Order_To_Execute_A_Call()
		{
			var paging = new Paging(20, 5);

		    await Service.Get(paging);

		    this.Repository.Verify(repository => repository.Get(paging), Times.Once);
			this.Repository.VerifyNoOtherCalls();
		}

		[Fact]
	    public async Task GetOrder_Method_Throws_If_No_OrderId_Specified()
		{
			await Assert.ThrowsAsync<ArgumentNullException>(() => Service.Get(orderId: null));
		}

		[Fact]
	    public async Task GetOrder_Method_Calls_Get_Method_Of_IOrdersRepository_In_Order_To_Execute_A_Call()
	    {
		    var order = OrdersSeed.Orders.Value.FirstOrDefault();
		    Assert.NotNull(order);

		    var orderId = OrderId.FromString(order.OrderId);

		    await Service.Get(orderId);

		    this.Repository.Verify(repository => repository.Get(orderId), Times.Once);
		    this.Repository.VerifyNoOtherCalls();
		}

		[Fact]
	    public async Task Update_Method_Throws_If_No_OrderId_Specified()
	    {
		    await Assert.ThrowsAsync<ArgumentNullException>(() => Service.Update(orderId: null, order: new OrderUpdate()));
		}

		[Fact]
	    public async Task Update_Method_Throws_If_No_OrderUpdate_Specified()
	    {
		    await Assert.ThrowsAsync<ArgumentNullException>(() => Service.Update(orderId: OrderId.FromString("SAL2312412"), order: null));
		}

		[Fact]
		public async Task Update_Method_Calls_Update_Method_Of_IOrdersRepository_In_Order_To_Execute_A_Call()
	    {
		    var order = OrdersSeed.Orders.Value.FirstOrDefault();
		    Assert.NotNull(order);

		    var orderId = OrderId.FromString(order.OrderId);
		    var orderUpdate = OrderUpdate.From(order);

		    await Service.Update(orderId, orderUpdate);

		    this.Repository.Verify(repository => repository.Update(orderId, orderUpdate), Times.Once);
		    this.Repository.VerifyNoOtherCalls();
		}
	}
}

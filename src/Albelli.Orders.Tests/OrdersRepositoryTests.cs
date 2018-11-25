using System;
using System.Linq;
using System.Threading.Tasks;
using Albelli.Core.Contracts;
using Albelli.Orders.Contracts.Models;
using Albelli.Orders.Persistence;
using Albelli.Orders.Persistence.Contracts;
using Albelli.Orders.Persistence.Profiles;
using Albelli.Orders.Tests.Integration.Orders;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Albelli.Orders.Tests
{
    public class OrdersRepositoryTests
    {
	    private readonly Func<OrdersRepositoryScope> _repoScopeCreate;

	    public OrdersRepositoryTests()
	    {
		    var config = new MapperConfiguration(cfg => { cfg.AddProfile<OrderProfile>(); });
		    var mapper = config.CreateMapper();
			var options = new DbContextOptionsBuilder<OrdersContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

		    this._repoScopeCreate = () => new OrdersRepositoryScope(options, mapper);
	    }

		[Fact]
		public void Repository_Requires_Instance_Of_OrdersContext_And_Mapper_In_Order_To_Be_Created()
		{
			var mapper = new Mock<IMapper>(MockBehavior.Loose);	
			Assert.Throws<ArgumentNullException>(() => new OrdersRepository(null, mapper.Object));
			var context = new Mock<OrdersContext>(MockBehavior.Loose);
			Assert.Throws<ArgumentNullException>(() => new OrdersRepository(context.Object, null));
		}

		[Fact]
	    public async Task Create_Method_Adds_Provided_Order_To_Context()
	    {
		    var order = OrdersSeed.Orders.Value.FirstOrDefault();
		    Assert.NotNull(order);

			using (var scope = this._repoScopeCreate())
		    {
			    var orderNew = OrderNew.From(order);

				var created = await scope.Repository.Create(orderNew);

			    Assert.NotNull(created);
			    Assert.True(created.OrderId == orderNew.OrderId);
			    Assert.True(created.CustomerName == orderNew.CustomerName);
			    Assert.True(created.PostCode == orderNew.PostCode);
			    Assert.True(created.HouseNumber == orderNew.HouseNumber);
			    Assert.True(created.Price == orderNew.Price);
		    }

		    using (var scope = this._repoScopeCreate())
		    {
			    var saved = await scope.Repository.Get(OrderId.FromString(order.OrderId));
				Assert.NotNull(saved);
		    }
		}

		[Fact]
	    public async Task Create_Method_Throws_If_No_OrderNew_Provided()
	    {
		    using (var scope = this._repoScopeCreate())
		    {
			    await Assert.ThrowsAsync<ArgumentNullException>(() => scope.Repository.Create(null));
		    }
		}

		[Fact]
	    public async Task Create_Method_Throws_If_Order_With_Provided_Id_Exists_In_Context()
	    {
		    var order = OrdersSeed.Orders.Value.FirstOrDefault();
		    Assert.NotNull(order);

		    var orderNew = OrderNew.From(order);

			using (var scope = this._repoScopeCreate())
		    {
			    await scope.Repository.Create(orderNew);
		    }

		    using (var scope = this._repoScopeCreate())
		    {
			    var ex = await Assert.ThrowsAsync<OrderExistsException>(() => scope.Repository.Create(orderNew));
			    Assert.True(ex.OrderId == orderNew.OrderId);
			}   
		}

	    [Fact]
	    public async Task Delete_Method_Throws_If_No_OrderId_Provided()
	    {
		    using (var scope = this._repoScopeCreate())
		    {
			    await Assert.ThrowsAsync<ArgumentNullException>(() => scope.Repository.Delete(null));
		    }
		}

		[Fact]
	    public async Task Delete_Method_Throws_If_Order_With_Specified_OrderId_Does_Not_Exist()
	    {
		    using (var scope = this._repoScopeCreate())
		    {
			    var orderId = OrderId.FromString("SAL246363");
			    var ex = await Assert.ThrowsAsync<OrderDoesNotExistException>(() => scope.Repository.Delete(orderId));
				Assert.True(ex.OrderId == orderId.Value);
		    }
		}

	    [Fact]
	    public async Task Delete_Method_Removes_Specified_Order_From_Context()
	    {
		    var order = OrdersSeed.Orders.Value.FirstOrDefault();
		    Assert.NotNull(order);

			var orderNew = OrderNew.From(order);
			var orderId = OrderId.FromString(orderNew.OrderId);

		    using (var scope = this._repoScopeCreate())
		    {
			    await scope.Repository.Create(orderNew);
		    }

		    using (var scope = this._repoScopeCreate())
		    {
			    await scope.Repository.Delete(orderId);
		    }

		    using (var scope = this._repoScopeCreate())
		    {
			    var deleted = await scope.Repository.Get(orderId);
				Assert.Null(deleted);
		    }
		}

		[Fact]
	    public async Task PaginatedGet_Method_Throws_If_No_Paging_Options_Provided()
	    {
		    using (var scope = this._repoScopeCreate())
		    {
			    await Assert.ThrowsAsync<ArgumentNullException>(() => scope.Repository.Get(paging: null));
		    }
		}

		[Fact]
	    public async Task PaginatedGet_Method_Includes_Total_Count_If_Specified_In_Paginated_Settings()
	    {
		    var paging = Paging.Default;
		    paging.IncludeTotalCnt = true;

		    using (var scope = this._repoScopeCreate())
		    {
				var result = await scope.Repository.Get(paging);
				Assert.NotNull(result);
				Assert.True(result.TotalCount == 0);
				Assert.True(result.Skipped == paging.Skip);
			    Assert.True(result.Take <= paging.Take);
		    }
		}

		[Fact]
	    public async Task PaginatedGet_Method_Skips_Total_Count_If__Not_Specified_In_Paginated_Settings()
	    {
		    var paging = Paging.Default;
		    paging.IncludeTotalCnt = false;

		    using (var scope = this._repoScopeCreate())
		    {
			    var result = await scope.Repository.Get(paging);
			    Assert.NotNull(result);
			    Assert.Null(result.TotalCount);
			    Assert.True(result.Skipped == paging.Skip);
			    Assert.True(result.Take <= paging.Take);
		    }
		}

		[Fact]
		public async Task PaginatedGet_Method_Returns_Paginated_Result_Of_Orders()
	    {
		    var paging = Paging.Default;
		    paging.IncludeTotalCnt = true;

		    using (var scope = this._repoScopeCreate())
		    {
			    foreach (var order in OrdersSeed.Orders.Value)
			    {
				    var orderNew = OrderNew.From(order);
				    await scope.Repository.Create(orderNew);
			    }
		    }

		    using (var scope = this._repoScopeCreate())
		    {
			    var result = await scope.Repository.Get(paging);
			    Assert.NotNull(result);
			    Assert.True(result.TotalCount == OrdersSeed.Orders.Value.Count);
			    Assert.True(result.Skipped == paging.Skip);
			    Assert.True(result.Take <= paging.Take);
		    }
		}

		[Fact]
	    public async Task Update_Method_Throws_If_No_OrderId_Provided()
	    {
		    using (var scope = this._repoScopeCreate())
		    {
			    await Assert.ThrowsAsync<ArgumentNullException>(() => scope.Repository.Update(null, new OrderUpdate()));
		    }
		}

		[Fact]
	    public async Task Update_Method_Throws_If_No_OrderUpdate_Provided()
	    {
		    using (var scope = this._repoScopeCreate())
		    {
			    await Assert.ThrowsAsync<ArgumentNullException>(() => scope.Repository.Update(OrderId.FromString("SAL3414214"), null));
		    }
		}

		[Fact]
		public async Task Update_Method_Throws_If_Order_With_Specified_OrderId_Does_Not_Exist()
	    {
		    var order = OrdersSeed.Orders.Value.FirstOrDefault();
		    Assert.NotNull(order);
		    var orderId = OrderId.FromString(order.OrderId);
		    var orderUpdate = OrderUpdate.From(order);

		    using (var scope = this._repoScopeCreate())
		    {
			    var ex = await Assert.ThrowsAsync<OrderDoesNotExistException>(() => scope.Repository.Update(orderId, orderUpdate));
			    Assert.True(ex.OrderId == orderId.Value);
		    }
		}

		[Fact]
	    public async Task Update_Method_Updates_Specified_Order_And_Returns_Updated_Order()
	    {
		    var order = OrdersSeed.Orders.Value.FirstOrDefault();
		    Assert.NotNull(order);

		    var orderNew = OrderNew.From(order);
		    var orderId = OrderId.FromString(orderNew.OrderId);

		    using (var scope = this._repoScopeCreate())
		    {
			    await scope.Repository.Create(orderNew);
		    }

		    using (var scope = this._repoScopeCreate())
		    {
			    var orderUpdate = OrderUpdate.From(order, update => update.CustomerName = "New Top Customer");
			    var updated = await scope.Repository.Update(orderId, orderUpdate);
				Assert.NotNull(updated);
				Assert.True(updated.OrderId == orderId.Value);
				Assert.True(updated.CustomerName == orderUpdate.CustomerName);
		    }
		}
    }
}

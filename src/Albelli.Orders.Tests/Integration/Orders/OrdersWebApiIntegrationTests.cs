using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Albelli.Common.Tests;
using Albelli.Common.Tests.Data;
using Albelli.Core.Contracts;
using Albelli.Orders.Contracts.Models;
using Albelli.Orders.Tests.Integration.Orders.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;

namespace Albelli.Orders.Tests.Integration.Orders
{
	public class OrdersWebApiIntegrationTests : IDisposable
	{
		public HttpClient Client { get; }

		public OrdersWebApiIntegrationTests()
		{
			var webHostBuilder = new WebHostBuilder()
				.UseStartup<StartupTest>();

			var server = new TestServer(webHostBuilder);
			this.Client = server.CreateClient();
			this.Client.Timeout = TimeSpan.FromSeconds(30);
		}

		public void Dispose()
		{
			Client?.Dispose();
		}

		[Theory]
		[InlineData(0U, 10U)]
		[InlineData(null, 10U)]
		[InlineData(0U, null)]
		[InlineData(null, null)]
		public async Task GetOrders_Returns_Paginated_List_Configured_By_Query(uint? skip, uint? take)
	    {
			var uri = $"{Routes.Orders}?skip={skip}&take={take}";
		    var taken = take ?? Paging.Default.Take;
		    var skipped = skip ?? Paging.Default.Skip;

		    var response = await this.Client.GetAsync(uri);

		    Assert.True(response.IsSuccessStatusCode);
			var result = await response.GetFromBody<PagingResult<Order>>();
			Assert.NotNull(result);
		    Assert.True(result.Skipped == skipped);
		    Assert.True(result.Take == taken);
		    Assert.True(result.Items.Count <= taken);
			Assert.True(result.TotalCount.HasValue);
		}

		[Fact]
		public async Task GetOrder_Returns_Requested_Order_When_Order_Exists()
		{
			var existingOrder = OrdersSeed.Orders.Value.FirstOrDefault();
			Assert.NotNull(existingOrder);

			var response = await this.Client.GetAsync(Routes.OrderUrl(existingOrder.OrderId));

			Assert.True(response.IsSuccessStatusCode);
			var result = await response.GetFromBody<Order>();
			Assert.NotNull(result);
			Assert.True(existingOrder.OrderId == result.OrderId);
		}

		[Fact]
		public async Task GetOrder_Returns_204_StatusCode_When_Requested_Order_Does_Not_Exist()
		{
			var uknownOrderId = "UKN2312312";

			var response = await this.Client.GetAsync(Routes.OrderUrl(uknownOrderId));

			Assert.True(response.StatusCode == HttpStatusCode.NoContent);
			var content = await response.Content.ReadAsStringAsync();
			Assert.True(string.IsNullOrEmpty(content));
		}

		[Theory]
		[InvalidOrderIds]
		public async Task GetOrder_Returns_A_List_Of_Validation_Errors_When_OrderIdQuery_Model_Is_Not_Valid(string orderId)
		{
			var response = await this.Client.GetAsync(Routes.OrderUrl(orderId));

			Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
			var validationErrors = await response.GetFromBody<Dictionary<string, object>>();
			Assert.NotNull(validationErrors);
			Assert.True(validationErrors.ContainsKey(nameof(OrderId)));
		}

		[Fact]
		public async Task PlaceOrder_Returns_Newly_Created_Order_With_Location_Headers_Set()
		{
			var newOrder = new OrderNew
			{
				CustomerName = "Ivan M",
				HouseNumber = 12,
				OrderId = "SAL1005236",
				PostCode = "1673AO",
				Price = 20.0m
			};

			var content = new StringContent(JsonConvert.SerializeObject(newOrder), Encoding.UTF8, "application/json");

			var response = await this.Client.PostAsync(Routes.Orders, content);

			Assert.True(response.StatusCode == HttpStatusCode.Created);
			Assert.True(response.Headers.Location.OriginalString == Routes.OrderUrl(newOrder.OrderId));
			var order = await response.GetFromBody<Order>();
			Assert.NotNull(order);
			Assert.True(order.OrderId == newOrder.OrderId);
		}

		[Fact]
		public async Task PlaceOrder_Fails_When_Attempt_To_Create_Order_With_Existing_OrderId()
		{
			var existingOrder = OrdersSeed.Orders.Value.FirstOrDefault();
			Assert.NotNull(existingOrder);

			var newOrder = new OrderNew
			{
				CustomerName = "Ivan M",
				HouseNumber = 12,
				OrderId = existingOrder.OrderId,
				PostCode = "1673AO",
				Price = 20.0m
			};

			var response = await this.Client.PostAsync(Routes.Orders, newOrder.AsStringContent());

			Assert.True(response.StatusCode == HttpStatusCode.InternalServerError);
			var error = await response.GetFromBody<ExposableApiError>();
			Assert.NotNull(error);
			Assert.True(!string.IsNullOrEmpty(error.Reason));
		}

		[Theory]
		[InvalidOrderNew]
		public async Task PlaceOrder_Returns_A_List_Of_Validation_Errors_When_OrderNew_Model_Is_Not_Valid(OrderNew newOrder)
		{
			var response = await this.Client.PostAsync(Routes.Orders, newOrder.AsStringContent());

			Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
			var validationErrors = await response.GetFromBody<Dictionary<string, object>>();
			Assert.NotNull(validationErrors);
			Assert.True(validationErrors.Keys.Count > 0);
		}

		[Fact]
		public async Task DeleteOrder_Returns_200_StatusCode_When_Deletion_Was_Successful()
		{
			var existingOrder = OrdersSeed.Orders.Value.FirstOrDefault();
			Assert.NotNull(existingOrder);

			var response = await this.Client.DeleteAsync(Routes.OrderUrl(existingOrder.OrderId));

			Assert.True(response.IsSuccessStatusCode);
		}

		[Fact]
		public async Task DeleteOrder_Fails_When_Try_To_Delete_Not_Existing_Order()
		{
			var uknownOrderId = "UKN2312312";

			var response = await this.Client.DeleteAsync(Routes.OrderUrl(uknownOrderId));

			Assert.True(response.StatusCode == HttpStatusCode.InternalServerError);
			var error = await response.GetFromBody<ExposableApiError>();
			Assert.NotNull(error);
			Assert.True(!string.IsNullOrEmpty(error.Reason));
		}

		[Theory]
		[InvalidOrderIds]
		public async Task DeleteOrder_Returns_A_List_Of_Validation_Errors_When_OrderId_Query_Model_Is_Not_Valid(string orderId)
		{
			var response = await this.Client.DeleteAsync(Routes.OrderUrl(orderId));

			Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
			var validationErrors = await response.GetFromBody<Dictionary<string, object>>();
			Assert.NotNull(validationErrors);
			Assert.True(validationErrors.ContainsKey(nameof(OrderId)));
		}

		[Fact]
		public async Task UpdateOrder_Returns_200_StatusCode_And_Updated_Order_When_Successful()
		{
			var existingOrder = OrdersSeed.Orders.Value.FirstOrDefault();
			Assert.NotNull(existingOrder);

			existingOrder.CustomerName = "Change me to this";

			var response = await this.Client.PutAsync(Routes.OrderUrl(existingOrder.OrderId), existingOrder.AsStringContent());

			var order = await response.GetFromBody<Order>();
			Assert.NotNull(order);
			Assert.True(order.OrderId == existingOrder.OrderId);
			Assert.True(order.CustomerName == existingOrder.CustomerName);
		}

		[Theory]
		[InvalidOrderIds]
		public async Task UpdateOrder_Returns_A_List_Of_Validation_Errors_When_OrderId_Is_Not_Valid(string orderId)
		{
			var existingOrder = OrdersSeed.Orders.Value.FirstOrDefault();
			Assert.NotNull(existingOrder);

			var response = await this.Client.PutAsync(Routes.OrderUrl(orderId), existingOrder.AsStringContent());

			Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
			var validationErrors = await response.GetFromBody<Dictionary<string, object>>();
			Assert.NotNull(validationErrors);
			Assert.True(validationErrors.ContainsKey(nameof(OrderId)));
		}

		[Theory]
		[InvalidOrderUpdates]
		public async Task UpdateOrder_Returns_A_List_Of_Validation_Errors_When_OrderUpdate_Model_Is_Not_Valid(OrderUpdate update)
		{
			var existingOrder = OrdersSeed.Orders.Value.FirstOrDefault();
			Assert.NotNull(existingOrder);

			var response = await this.Client.PutAsync(Routes.OrderUrl(existingOrder.OrderId), update.AsStringContent());

			Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
			var validationErrors = await response.GetFromBody<Dictionary<string, object>>();
			Assert.NotNull(validationErrors);
			Assert.True(validationErrors.Keys.Count > 0);
		}
	}
}

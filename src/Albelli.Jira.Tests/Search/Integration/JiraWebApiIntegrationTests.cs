using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Albelli.Common.Tests;
using Albelli.Common.Tests.Data;
using Albelli.Core.Contracts;
using Albelli.Jira.Contracts.Models;
using Albelli.Orders.Contracts.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Albelli.Jira.Tests.Search.Integration
{
    public class JiraWebApiIntegrationTests : IDisposable
    {
	    public HttpClient Client { get; }

	    public JiraWebApiIntegrationTests()
	    {
		    var webHostBuilder = new WebHostBuilder()
			    .UseStartup<StartupTest>()
			    .ConfigureAppConfiguration(builder => builder.AddJsonFile("appsettings.json"));

		    var server = new TestServer(webHostBuilder);
		    this.Client = server.CreateClient();
		    this.Client.Timeout = TimeSpan.FromSeconds(30);
	    }

	    public void Dispose()
	    {
		    Client?.Dispose();
	    }

	    [Theory]
	    [InvalidOrderIds]
	    public async Task GetIssues_Returns_A_List_Of_Validation_Errors_When_OrderIdQuery_Model_Is_Not_Valid(string orderId)
	    {
		    var response = await this.Client.GetAsync(Routes.IssuesByOrderId(orderId));

		    Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
		    var validationErrors = await response.GetFromBody<Dictionary<string, object>>();
		    Assert.NotNull(validationErrors);
		    Assert.True(validationErrors.ContainsKey(nameof(OrderId)));
	    }


	    [Theory]
	    [InlineData(0U, 10U)]
	    [InlineData(null, 10U)]
	    [InlineData(0U, null)]
	    [InlineData(null, null)]
	    public async Task GetIssues_Returns_Paginated_List_Configured_By_Query(uint? skip, uint? take)
	    {
		    var issueRecord = JiraIssuesSeed.Records.Value.FirstOrDefault();
			Assert.NotNull(issueRecord);

		    var uri = $"{Routes.IssuesByOrderId(issueRecord.OrderId)}";
		    if (skip.HasValue) uri = uri + (uri.Contains("?") ? "&" : "?") + $"skip={skip}";
			if (take.HasValue) uri = uri + (uri.Contains("?") ? "&" : "?") + $"take={take}";

			var taken = take ?? Paging.Default.Take;
		    var skipped = skip ?? Paging.Default.Skip;

			var response = await this.Client.GetAsync(uri);

			Assert.True(response.IsSuccessStatusCode);
		    var result = await response.GetFromBody<PagingResult<JiraShortIssue>>();
			Assert.NotNull(result);
			Assert.True(result.Skipped == skipped);
		    Assert.True(result.Take == taken);
		    Assert.True(result.Items.Count <= taken);
		    Assert.True(result.TotalCount.HasValue);
		}

		[Fact]
	    public async Task GetIssues_Returns_Empty_List_If_There_Are_No_Issue_With_Specified_OrderId()
	    {
		    var orderId = OrderId.FromString("TST2312312").Value;

		    var uri = $"{Routes.IssuesByOrderId(orderId)}";

		    var response = await this.Client.GetAsync(uri);

		    Assert.True(response.IsSuccessStatusCode);
		    var result = await response.GetFromBody<PagingResult<JiraShortIssue>>();
		    Assert.NotNull(result);
			Assert.True(result.Items.Count == 0);
		}
	}
}

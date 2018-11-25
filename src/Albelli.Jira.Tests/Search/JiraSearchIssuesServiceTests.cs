using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Albelli.Jira.Contracts;
using Albelli.Jira.Services;
using Albelli.Jira.Tests.Search.Handlers;
using Albelli.Orders.Contracts.Models;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Albelli.Jira.Tests.Search
{
    public class JiraSearchIssuesServiceTests
    {
		public JiraApiSettings Settings { get; }

		public JiraSearchIssuesServiceTests()
	    {
			var builder = new ConfigurationBuilder()
			    .AddJsonFile("appsettings.json");

		    var configuration = builder.Build();

		    Settings = configuration
			    .GetSection(nameof(JiraApiSettings))
			    .Get<JiraApiSettings>();
	    }

		[Fact]
	    public void Service_Requires_Instance_Of_HttpClient_In_Order_To_Be_Created()
	    {
		    Assert.Throws<ArgumentNullException>(() => new JiraSearchIssuesService(null));
	    }

		[Fact]
	    public async Task SearchIssuesByOrderId_Throws_If_OrderId_Was_Not_Specified()
	    {
			var service = new JiraSearchIssuesService(new HttpClient());
		    await Assert.ThrowsAsync<ArgumentNullException>(() => service.SearchIssuesByOrderId(null));
	    }

	    [Fact]
	    public async Task SearchIssuesByOrderId_Returns_Paginated_List_Jira_Issue_When_Executed_Successfully()
	    {
		    var issueRecord = JiraIssuesSeed.Records.Value.FirstOrDefault();
		    Assert.NotNull(issueRecord);

		    var client = new HttpClient(new JiraSearchIssuesHandler()) { BaseAddress = Settings.RestApiUri };
			var service = new JiraSearchIssuesService(client);
		    var orderId = OrderId.FromString(issueRecord.OrderId);

		    var result = await service.SearchIssuesByOrderId(orderId);

			Assert.NotNull(result);
			Assert.True(result.Items.Count == issueRecord.Issues.Count);
	    }

	    [Fact]
	    public async Task SearchIssuesByOrderId_Throws_Jira_Exception_When_HttpResponse_Has_Non_OK_Status_Code()
	    {
		    var issueRecord = JiraIssuesSeed.Records.Value.FirstOrDefault();
		    Assert.NotNull(issueRecord);

		    var client = new HttpClient(new ErrorStatusCodeHandler()) { BaseAddress = Settings.RestApiUri };
		    var service = new JiraSearchIssuesService(client);
		    var orderId = OrderId.FromString(issueRecord.OrderId);

		    await Assert.ThrowsAsync<JiraException>(() => service.SearchIssuesByOrderId(orderId));
	    }

	    [Fact]
	    public async Task SearchIssuesByOrderId_Throws_Jira_Exception_When_HttpContent_Contains_Invalid_Json()
	    {
		    var issueRecord = JiraIssuesSeed.Records.Value.FirstOrDefault();
		    Assert.NotNull(issueRecord);

		    var client = new HttpClient(new InvalidJsonHandler()) { BaseAddress = Settings.RestApiUri };
		    var service = new JiraSearchIssuesService(client);
		    var orderId = OrderId.FromString(issueRecord.OrderId);

		    await Assert.ThrowsAsync<JiraException>(() => service.SearchIssuesByOrderId(orderId));
	    }
	}
}

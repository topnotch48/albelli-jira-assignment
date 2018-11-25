using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Albelli.Common.Tests;
using Albelli.Jira.Contracts.Models;
using Albelli.Jira.Contracts.Responses;

namespace Albelli.Jira.Tests.Search.Handlers
{
    public class JiraSearchIssuesHandler : HttpMessageHandler
	{
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var records = JiraIssuesSeed.Records.Value;

			var queryParams = System.Web.HttpUtility.ParseQueryString(request.RequestUri.Query);
			var jql = queryParams.Get("jql");
			var orderId = jql.Replace("description~", String.Empty);
			var startAt = queryParams.Get("startAt");
			var maxResults = queryParams.Get("maxResults");

			var record = records.FirstOrDefault(r => r.OrderId == orderId);
			var issues = record != null ? record.Issues : new List<JiraShortIssue>();

			var response = new JiraSearchIssuesResponse
			{
				Issues = issues,
				MaxResults = string.IsNullOrEmpty(maxResults) ? 50 : uint.Parse(maxResults),
				StartAt = string.IsNullOrEmpty(startAt) ? 0 : uint.Parse(startAt),
				Expand = "",
				Total = (uint)issues.Count
			};

			var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
			{
				Content = response.AsStringContent()
			};

			return Task.FromResult(responseMessage);
		}
	}
}

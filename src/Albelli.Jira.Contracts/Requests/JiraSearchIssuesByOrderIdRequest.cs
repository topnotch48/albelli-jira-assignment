using System;
using System.Collections.Generic;
using System.Net.Http;
using Albelli.Core.Contracts;
using Albelli.Orders.Contracts.Models;

namespace Albelli.Jira.Contracts.Requests
{
    public class JiraSearchIssuesByOrderIdRequest : JiraRequest
    {
	    public IReadOnlyCollection<string> Fields => new[]
	    {
		    "id",
			"key",
			"summary",
			"priority",
			"status"
	    };

	    public override HttpMethod Verb => HttpMethod.Get;

	    public override string Path => "search";

	    public override string Jql => $"description~{this.OrderId}";

		public override string QueryParams => $"fields={string.Join(",", this.Fields)}&startAt={this.Paging.Skip}&maxResults={this.Paging.Take}";

	    public string OrderId { get; }

		public Paging Paging { get; }

	    public JiraSearchIssuesByOrderIdRequest(OrderId orderId, Paging paging = null)
	    {
		    this.OrderId = orderId.Value ?? throw new ArgumentNullException(nameof(orderId));
		    this.Paging = GetJiraPaging(paging);
	    }
    }
}

using System;

namespace Albelli.Jira.Tests.Search.Integration
{
	public static class Routes
	{
		public static string BaseUri => "/api";

		public static string Issues => $"{BaseUri}/search";

		public static Func<string, string> IssuesByOrderId = orderId => $"{Issues}/{orderId}/issues";
	}
}

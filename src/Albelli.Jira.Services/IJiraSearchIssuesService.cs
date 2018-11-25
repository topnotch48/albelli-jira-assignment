using System;
using System.Threading.Tasks;
using Albelli.Core.Contracts;
using Albelli.Jira.Contracts.Models;
using Albelli.Orders.Contracts.Models;

namespace Albelli.Jira.Services
{
	public interface IJiraSearchIssuesService : IDisposable
	{
		Task<PagingResult<JiraShortIssue>> SearchIssuesByOrderId(OrderId orderId, Paging paging = null);
	}
}
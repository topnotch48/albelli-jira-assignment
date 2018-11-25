using System.Collections.Generic;
using Albelli.Jira.Contracts.Models;

namespace Albelli.Jira.Tests.Search
{
    public class JiraIssueRecord
    {
		public string OrderId { get; set; }
		public IList<JiraShortIssue> Issues { get; set; } = new List<JiraShortIssue>();
    }
}

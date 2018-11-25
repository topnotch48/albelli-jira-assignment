using System.Collections.Generic;

namespace Albelli.Jira.Contracts.Responses
{
    public class JiraSearchResponse<TIssue> where TIssue : class
    {
		public string Expand { get; set; }

	    public uint StartAt { get; set; }

	    public uint MaxResults { get; set; }

	    public uint Total { get; set; }

		public IList<TIssue> Issues { get; set; } = new List<TIssue>();
    }
}

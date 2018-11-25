namespace Albelli.Jira.Contracts.Models
{
	public class JiraShortIssueFields
    {
		public string Summary { get; set; }

		public JiraIssuePriority Priority { get; set; }

		public JiraIssueStatus Status { get; set; }
    }
}

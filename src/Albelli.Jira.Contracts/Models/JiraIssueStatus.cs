namespace Albelli.Jira.Contracts.Models
{
	public class JiraIssueStatus
	{
		public string Id { get; set; }

		public string Self { get; set; }

		public string Description { get; set; }

		public string IconUrl { get; set; }

		public string Name { get; set; }

		public JiraIssueStatusCatergory StatusCategory { get; set; }
	}
}

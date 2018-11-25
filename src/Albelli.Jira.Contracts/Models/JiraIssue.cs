namespace Albelli.Jira.Contracts.Models
{
    public class JiraIssue<TIssueFields> where TIssueFields : class
    {
		public string Expand { get; set; }

		public string Id { get; set; }

		public string Key { get; set; }

		public string Self { get; set; }

		public TIssueFields Fields { get; set; }
    }
}

using System.Net.Http;
using Albelli.Core.Contracts;

namespace Albelli.Jira.Contracts.Requests
{
    public abstract class JiraRequest
    {
	    public static Paging GetJiraPaging(Paging paging)
	    {
		    if (paging == null)
			    return Paging.Default;

		    var take = paging.Take > 0 ? paging.Take : Paging.Default.Take;
		    var skip = paging.Skip > 0 ? paging.Skip : Paging.Default.Skip;

			var merged = new Paging(take, skip);

		    return merged;
	    }

		public abstract HttpMethod Verb { get; }

		public abstract string Path { get; }

		public abstract string Jql { get; }

		public abstract string QueryParams { get; }

		public virtual string Uri => $"{this.Path}?jql={this.Jql}&{this.QueryParams}";
	}
}

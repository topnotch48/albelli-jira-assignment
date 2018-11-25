 using System;

namespace Albelli.Jira.Contracts
{
    public class JiraApiSettings
    {
		public string BaseUrl { get; set; }

	    public string RestApiSuffix { get; set; }

	    public TimeSpan RequestTimeout { get; } = new TimeSpan(0, 0, 10);

	    public Uri RestApiUri
	    {
		    get
			{
				var baseUri = new Uri(this.BaseUrl);
				var apiUri = new Uri(baseUri, RestApiSuffix);
				return apiUri;
			}
	    }
    }
}

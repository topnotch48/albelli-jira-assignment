using System.Net.Http;
using Albelli.Jira.Contracts;
using Albelli.Jira.Tests.Search.Handlers;
using Albelli.Jira.WebApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Albelli.Jira.Tests.Search.Integration
{
    public class StartupTest : Startup
    {
	    public StartupTest(IConfiguration configuration) : base(configuration)
	    {
	    }

	    protected override void ConfigureHttpClient(IServiceCollection services)
	    {
		    var settings = this.Configuration
			    .GetSection(nameof(JiraApiSettings))
			    .Get<JiraApiSettings>();

			services.AddScoped<HttpClient>(provider =>
		    {
			    var handler = new JiraSearchIssuesHandler();
			    return new HttpClient(handler) { BaseAddress = settings.RestApiUri, Timeout = settings.RequestTimeout };
		    });
		}
	}
}

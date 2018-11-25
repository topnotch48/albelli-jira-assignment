using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Albelli.Jira.Tests.Search.Handlers
{
	public class InvalidJsonHandler : HttpMessageHandler
	{
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
			CancellationToken cancellationToken)
		{
			var response = new HttpResponseMessage(HttpStatusCode.OK)
			{
				Content = new StringContent(
					@"
				/%{ sdasdasdas: 'sdsdsdsda' }
			")
			};
			return Task.FromResult(response);
		}

	}
}

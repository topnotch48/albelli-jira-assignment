using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Albelli.Jira.Tests.Search.Handlers
{
    public class ErrorStatusCodeHandler : HttpMessageHandler
    {
	    private readonly HttpStatusCode _code;

		public ErrorStatusCodeHandler(HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
		{
			this._code = statusCode;
		}

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
			CancellationToken cancellationToken)
		{
			var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
			return Task.FromResult(response);
		}

	}
}

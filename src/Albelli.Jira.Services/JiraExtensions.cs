using System;
using System.Net.Http;
using Albelli.Jira.Contracts.Requests;

namespace Albelli.Jira.Services
{
    public static class JiraExtensions
    {
	    public static HttpRequestMessage ToRequestMessage(this JiraRequest request, Action<HttpRequestMessage> configureFn = null)
	    {
			var requestMessage = new HttpRequestMessage(request.Verb, request.Uri);
			configureFn?.Invoke(requestMessage);
		    return requestMessage;
	    }
    }
}

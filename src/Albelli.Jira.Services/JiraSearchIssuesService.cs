using System;
using System.Net.Http;
using System.Threading.Tasks;
using Albelli.Core.Contracts;
using Albelli.Jira.Contracts;
using Albelli.Jira.Contracts.Models;
using Albelli.Jira.Contracts.Requests;
using Albelli.Jira.Contracts.Responses;
using Albelli.Jira.Services.Properties;
using Albelli.Orders.Contracts.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Albelli.Jira.Services
{
	public class JiraSearchIssuesService : IJiraSearchIssuesService
	{
	    private readonly HttpClient _httpClient;

		public JiraSearchIssuesService(HttpClient httpClient)
		{
			this._httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
		}

	    public async Task<PagingResult<JiraShortIssue>> SearchIssuesByOrderId(OrderId orderId, Paging paging = null)
	    {
			if (orderId == null)
				throw new ArgumentNullException(nameof(orderId));

			var jiraRequest = new JiraSearchIssuesByOrderIdRequest(orderId, paging);

		    var request = jiraRequest.ToRequestMessage();

		    var response = await this._httpClient.SendAsync(request);

		    if (!response.IsSuccessStatusCode)
		    {
			    var exMessage = string.Format(Resources.JiraEndpointFailureGeneric, response.ReasonPhrase,
				    response.StatusCode);

			    throw new JiraException(exMessage);
		    }

			try
		    {  
			    var content = await response.Content.ReadAsStringAsync();

			    var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

			    var result = JsonConvert.DeserializeObject<JiraSearchIssuesResponse>(content, settings);

			    return new PagingResult<JiraShortIssue>(result.StartAt, result.MaxResults, result.Issues, result.Total);
		    }
		    catch (Exception ex)
		    {
			    var exMessage = string.Format(Resources.JiraExceptionOnResponse, ex.Message,
				    nameof(JiraSearchIssuesByOrderIdRequest));

				throw new JiraException(exMessage, ex);
		    }
	    }

	    public void Dispose()
	    {
		    _httpClient?.Dispose();
	    }
    }
}

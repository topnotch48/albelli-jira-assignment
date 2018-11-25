using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Albelli.Core.Contracts;
using Albelli.Jira.Contracts.Responses;
using Albelli.Jira.Services;
using Albelli.Orders.Contracts.Models;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable 1570

namespace Albelli.Jira.WebApi.Controllers
{
    [Route("api/search")]
    public class JiraIssuesController : Controller
    {
	    private readonly IJiraSearchIssuesService _jiraSearchService;

	    public JiraIssuesController(IJiraSearchIssuesService jiraSearchService)
	    {
		    this._jiraSearchService = jiraSearchService ?? throw new ArgumentNullException(nameof(jiraSearchService));
	    }

		/// <summary>
		/// Retrieves a paginated list of jira issues which mention specified OrderId
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     GET api/search/SAL231413/issues?skip=0&take=10
		///
		/// </remarks>
		/// <param name="query">OrderId query model</param>
		/// <param name="paging">Pagination options model</param>
		/// <returns>A paginated list of issues</returns>
		/// <response code="200">Paginated list of issues retrieved</response>
		/// <response code="400">Dictionary containing OrderIdQuery model validation errors</response>
		[HttpGet("{OrderId}/issues")]
	    [ProducesResponseType(typeof(JiraSearchIssuesResponse), 200)]
	    [ProducesResponseType(typeof(Dictionary<string, object>), 400)]
		public async Task<IActionResult> GetIssues([FromRoute] OrderIdQuery query, [FromQuery] Paging paging)
	    {
		    if (!ModelState.IsValid)
			    return BadRequest(ModelState);

		    var orderId = OrderId.FromString(query.OrderId);

			var results = await this._jiraSearchService.SearchIssuesByOrderId(orderId, paging);

	        return Ok(results);
        }
    }
}

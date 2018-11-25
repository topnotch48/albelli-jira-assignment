using System;
using Albelli.Core.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Albelli.Common.Web.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
	    private readonly ILogger _logger;

	    public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
	    {
		    this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
	    }

	    public override void OnException(ExceptionContext context)
	    {
		    var ex = context.Exception;

		    var actionName = context.ActionDescriptor.DisplayName;

		    var reason = $"An error '{FetchExceptionMessage(ex)}' occurred in the '{actionName}' action";

			this._logger.LogError(ex, reason);

		    if (!(ex is ExposableApiException))
		    {
			    var error = ExposableApiError.Create(reason, StatusCodes.Status500InternalServerError);
			    context.Result = new ObjectResult(error) { StatusCode = error.StatusCode };
			    return;
		    }

		    var exposableEx = ex as ExposableApiException;

		    context.Result = new ObjectResult(exposableEx.AsExposable())
			{
				StatusCode = exposableEx.StatusCode ?? StatusCodes.Status500InternalServerError
			};
	    }

	    private string FetchExceptionMessage(Exception ex)
	    {
		    return ex.InnerException != null ? ex.InnerException.Message : ex.Message;
	    }
	}
}

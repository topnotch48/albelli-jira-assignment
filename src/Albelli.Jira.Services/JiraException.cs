using System;
using Albelli.Core.Contracts;

namespace Albelli.Jira.Services
{
    public class JiraException : ExposableApiException
	{
	    public JiraException(string message) : base(message)
	    {
	    }

		public JiraException(string message, Exception ex) : base(message, ex)
	    {
	    }
    }
}

using System;

namespace Albelli.Core.Contracts
{
    public abstract class ExposableApiException : Exception
    {
		public string Reason { get; set; }

		public int? StatusCode { get; set; }

	    protected ExposableApiException(string reason) : base(reason)
	    {
		    this.Reason = reason;
	    }

	    protected ExposableApiException(string reason, Exception ex) : base(reason, ex)
	    {
		    this.Reason = reason;
	    }

		public ExposableApiError AsExposable()
		{
			return ExposableApiError.FromException(this);
		}
    }
}

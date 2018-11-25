using System.Runtime.Serialization;

namespace Albelli.Core.Contracts
{
	[DataContract]
    public class ExposableApiError
    {
		[DataMember]
		public string Reason { get; set; }

		[IgnoreDataMember]
		public int? StatusCode { get; set; }


	    public static ExposableApiError Create(string reason, int? statusCode = null)
	    {
		    return new ExposableApiError
		    {
			    Reason = reason,
			    StatusCode = statusCode
		    };
	    }

	    public static ExposableApiError FromException(ExposableApiException ex)
	    {
		    return new ExposableApiError
		    {
			    Reason = ex.Reason,
			    StatusCode = ex.StatusCode
		    };
	    }
    }
}

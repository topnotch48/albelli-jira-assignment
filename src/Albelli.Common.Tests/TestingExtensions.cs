using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Albelli.Common.Tests
{
    public static class TestingExtensions
    {
	    public static async Task<T> GetFromBody<T>(this HttpResponseMessage response)
	    {
		    var message = await response.Content.ReadAsStringAsync();
		    return JsonConvert.DeserializeObject<T>(message);
	    }

	    public static HttpContent AsStringContent<T>(this T source, Encoding encoding = null, string mimeType = "application/json")
	    {
		    var content = new StringContent(JsonConvert.SerializeObject(source), encoding ?? Encoding.UTF8, mimeType);
		    return content;
	    }
	}
}

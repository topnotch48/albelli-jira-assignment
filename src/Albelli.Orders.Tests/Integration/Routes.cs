using System;

namespace Albelli.Orders.Tests.Integration
{
    public static class Routes
    {
	    public static string BaseUri => "/api";
	    public static string Orders => $"{BaseUri}/orders";
	    public static Func<string, string> OrderUrl = orderId => $"{Orders}/{orderId}";
    }
}

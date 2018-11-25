using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Albelli.Orders.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
			try
			{
				BuildWebHost(args).Run();
			}
			finally
			{
				NLog.LogManager.Shutdown();
			}
		}

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
	            .CaptureStartupErrors(false)
				.ConfigureLogging(logging => logging.ClearProviders())
	            .UseNLog(new NLogAspNetCoreOptions { IgnoreEmptyEventId = true })
				.UseStartup<Startup>()
                .Build();
    }
}

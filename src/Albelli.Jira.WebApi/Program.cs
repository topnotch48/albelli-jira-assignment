using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Albelli.Jira.WebApi
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
                .UseStartup<Startup>()
				.ConfigureLogging(logging => logging.ClearProviders())
				.UseNLog(new NLogAspNetCoreOptions { IgnoreEmptyEventId = true })
                .Build();
    }
}

using Albelli.Orders.Persistence;
using Albelli.Orders.Tests.Integration.Orders;
using Albelli.Orders.WebApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Albelli.Orders.Tests.Integration
{
    public class StartupTest : Startup
    {
	    public StartupTest(IConfiguration configuration) : base(configuration)
	    {
	    }

	    protected override void ConfigureDatabase(IServiceCollection services)
	    {
		    services.AddDbContext<OrdersContext>(builder => builder.UseInMemoryDatabase("orders.db"));
		    services.AddScoped<OrdersSeeder>();
		}

	    public override void Configure(IApplicationBuilder app, IHostingEnvironment env)
	    {
			base.Configure(app, env);

		    using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
		    {
			    var seeder = serviceScope.ServiceProvider.GetService<OrdersSeeder>();
			    seeder.Seed();
		    }
		}
	}
}

using System;
using Albelli.Common.Web.Filters;
using Albelli.Orders.Persistence;
using Albelli.Orders.Persistence.Contracts;
using Albelli.Orders.Persistence.Profiles;
using Albelli.Orders.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Albelli.Orders.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
			var config = new AutoMapper.MapperConfiguration(cfg => { cfg.AddProfile<OrderProfile>(); });
	        var mapper = config.CreateMapper();
	        services.AddSingleton(mapper);

			services.AddScoped(typeof(IOrdersManagementService), typeof(OrdersManagementService));
	        services.AddScoped(typeof(IOrdersRepository), typeof(OrdersRepository));

	        this.ConfigureDatabase(services);

			var mvc = services.AddMvc(options => { options.Filters.Add(typeof(ApiExceptionFilter)); });
		        
		    mvc.AddFluentValidation(options =>
		    {
			    options.RegisterValidatorsFromAssemblyContaining<Startup>();
			    options.RegisterValidatorsFromAssemblyContaining(typeof(ApiExceptionFilter));
		    });
		}

	    protected virtual void ConfigureDatabase(IServiceCollection services)
	    {
		    var connectionString = Configuration.GetConnectionString(nameof(OrdersContext));

		    if (string.IsNullOrEmpty(connectionString))
			    throw new Exception($"{ nameof(connectionString) } was not found in appsettings.json. Unable to start the host.");

			services.AddDbContext<OrdersContext>(options =>
			{
				options.UseSqlServer(connectionString);
			});

		    var provider = services.BuildServiceProvider();

		    using (var serviceScope = provider.GetRequiredService<IServiceScopeFactory>().CreateScope())
		    {
			    var context = serviceScope.ServiceProvider.GetService<OrdersContext>();
			    if (!context.AllAvailableMigrationsApplied())
				    context.Database.Migrate();
		    }
		}

	    public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}

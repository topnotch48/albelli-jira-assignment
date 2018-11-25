using System;
using System.Net.Http;
using Albelli.Common.Web.Filters;
using Albelli.Jira.Contracts;
using Albelli.Jira.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Albelli.Jira.WebApi
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
	        this.ConfigureHttpClient(services);

			services.AddScoped(typeof(IJiraSearchIssuesService), typeof(JiraSearchIssuesService));

	        var mvc = services.AddMvc(options =>
			{
				options.Filters.Add(typeof(ApiExceptionFilter));
			});

	        mvc.AddFluentValidation(options =>
	        {
		        options.RegisterValidatorsFromAssemblyContaining<Startup>();
		        options.RegisterValidatorsFromAssemblyContaining(typeof(ApiExceptionFilter));
	        });
		}

	    protected virtual void ConfigureHttpClient(IServiceCollection services)
	    {
			var settings = this.Configuration
			    .GetSection(nameof(JiraApiSettings))
			    .Get<JiraApiSettings>();

		    if (settings == null)
			    throw new Exception($"{ nameof(JiraApiSettings) } configuration was not found in appsettings.json. Unable to start the host.");

		    services.AddSingleton(settings);

			services.AddScoped<HttpClient>(provider =>
			{
				var handler = new HttpClientHandler();
				return new HttpClient(handler) { BaseAddress = settings.RestApiUri, Timeout = settings.RequestTimeout };
			});
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using FourWallsInc.Entity.SSO;
using FourWallsInc.Infrastructure.ConfigMgmt;
using FourWallsInc.Utilities;
using CMS.Master.Api.DataAccess;
using CMS.Master.Api.Business;

namespace CMS.Master.Api
{
	public class Startup
	{
		private readonly IConfiguration configuration;
		private SsoInfo ssoInfo;

		public Startup
			(
				IConfiguration configuration
			)
		{
			this.configuration = configuration;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices (IServiceCollection services)
		{
			// Get the SSO information from the appSettings.json configuration source.
			services.Configure<SsoInfo> (this.configuration.GetSection ("SsoInfo"));

			// Add the abstractions and their concrete types to the dependency injection container.
			services.AddScoped<IConfigManager, DefaultConfigManager> ();
			services.AddScoped<IModuleLinkDataAccess, ModuleLinkDataAccess> ();
			services.AddScoped<ICmsModuleAndLinksBizFacade, CmsModuleAndLinksBizFacade> ();

			// Get the SSO info (about the IDP, master app pair, module app pairs, etc.)
			this.ssoInfo = services.BuildServiceProvider ().GetService<IOptions<SsoInfo>> ().Value;

			services
				.AddAuthentication
					(
						options =>
						{
							// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: Since this is an "API app", the following
							// are the expected default authentication and challenge schemes.
							options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
							options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
						}
					)
				.AddJwtBearer
					(
						o =>
						{
							// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: The URL of the IDP that is going to protect this API app.
							o.Authority = this.ssoInfo.IdpServerUri;
							o.Audience = this.ssoInfo.MasterLayoutApp.Api.ResourceName;
							// The name of this API app that the protecting IDP would recognize.
							o.RequireHttpsMetadata = false;
						}
					);

			services.AddMemoryCache ();
			services.AddMvc ();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure (IApplicationBuilder app, IHostingEnvironment env)
		{
			// Allow the JavaScripts of friendly other protected applications to call the API services
			// of this API app, by enabling C.O.R.S. between the two applications.
			app.UseCors
				(
					builder =>
						// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: Adding "CORS" (Cross-origin Resource Sharing) between
						// API app and its companion web app. This means that a JavaScript from the companion web app
						// can access the API service in this API app.
						builder
							.WithOrigins (this.ssoInfo.MasterLayoutApp.App.RedirectUri.GetCorsUri ())          // URL of the master web app.
							.AllowAnyHeader ()
				);

			if (env.IsDevelopment ())
			{
				app.UseDeveloperExceptionPage ();
			}

			app.UseAuthentication ();
			app.UseMvc ();
		}
	}
}

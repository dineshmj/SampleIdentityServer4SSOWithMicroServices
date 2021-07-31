using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using FourWallsInc.Entity.SSO;
using FourWallsInc.WebUI.Cookies;
using FourWallsInc.Utilities;

namespace CMS.Proposals.Api
{
	public class Startup
	{
		private readonly IConfiguration configuration;
		private AppPair currentModuleAppPair;

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
			services.Configure<AppPair> (this.configuration.GetSection ("ModuleInfo"));

			this.currentModuleAppPair = services.BuildServiceProvider ().GetService<IOptions<AppPair>> ().Value;
			var idpServerUri = this.configuration.GetSection ("IdpServerUri").Value;

			services
				.AddAuthentication
					(
						options =>
						{
							// // ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: Since this is an "API app", the following are
							// the expected default authentication and challenge schemes.
							options.DefaultAuthenticateScheme
								= JwtBearerDefaults.AuthenticationScheme;
							options.DefaultChallengeScheme
								= JwtBearerDefaults.AuthenticationScheme;
						}
					)
				.AddJwtBearer
					(
						o =>
						{
							// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: The URL of the IDP that is going to protect this API app.
							o.Authority = idpServerUri;

							o.Audience = this.currentModuleAppPair.Api.ResourceName;
							// The name of this API app that the protecting IDP would recognize.
							o.RequireHttpsMetadata = false;
						}
					);

			services.AddMemoryCache ();

			services.Configure<CookieAuthenticationOptions>
				(
					x =>
					{
						x.SessionStore = new MemoryCacheTicketStore ();
					}
				);

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
							.WithOrigins (this.currentModuleAppPair.App.RedirectUri.GetCorsUri ())          // URL of the companion web app.
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

using System.IO;
using System.Security.Cryptography.X509Certificates;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using IdentityServer4.Services;

using FourWallsInc.DataAccess;
using FourWallsInc.Entity.LobApp.CMS;
using FourWallsInc.Entity.SSO;
using FourWallsInc.Infrastructure.ConfigMgmt;
using CMS.IDP.App.Business;
using CMS.IDP.App.Business.SSO;
using CMS.IDP.App.DataAccess;

namespace CMS.IDP.App
{
	/// <summary>
	/// Starts up the IDP web app.
	/// </summary>
	public class Startup
	{
		private readonly IHostingEnvironment environment;
		private readonly IConfiguration configuration;
		private bool useStubData = false;

		public Startup
			(
				IHostingEnvironment environment,
				IConfiguration configuration
			)
		{
			this.environment = environment;
			this.configuration = configuration;
			this.useStubData = configuration.GetValue<bool> ("UseStubData");
		}

		/// <summary>
		/// Helps configure the services that are associated with this IDP.
		/// </summary>
		/// <param name="services">The services collection.</param>
		public void ConfigureServices (IServiceCollection services)
		{
			// ↓↓ CATCH: Read the SSO related info from appSettings.json.
			services.Configure<SsoInfo> (this.configuration.GetSection ("SsoInfo"));

			// ↓↓ CATCH: Tell the dependency injection container about the custom abstractions
			// and concrete types that you're planning to use.
			services.AddScoped<IConfigManager, DefaultConfigManager> ();
			services.AddScoped<IDataAccess<CmsLoginInfo>, CmsLoginInfoDataAccess> ();
			services.AddScoped<ICmsAuthenticationDataAccess, CmsAuthenticationDataAccess> ();
			
			if (this.useStubData)
			{
				services.AddScoped<ICmsAuthenticationBizFacade, StubCmsAuthenticationBizFacade> ();
			}
			else
			{
				services.AddScoped<ICmsAuthenticationBizFacade, CmsAuthenticationBizFacade> ();
			}

			services.AddScoped<IProfileService, CmsUserProfileService> ();
			services.AddScoped<IAspNetCoreWebAndApiAppsDataProvider, AspNetCoreWebAndApiAppsDataProvider> ();

			// ↓↓ CATCH: Build the dependency injection container so that you can service-locate dependencies.
			var provider = services.BuildServiceProvider ();

			// ↓↓ CATCH: Service-locate the app and services data provider.
			var appAndServicesDataProvider
				= provider.GetService<IAspNetCoreWebAndApiAppsDataProvider> ();

			services
				// ↓↓ CATCH: Tells to use the identity server.
				.AddIdentityServer ()

				// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: The certificate that this IDP would use for encrypting the bag of JWT's.
				.AddSigningCredential (new X509Certificate2 (Path.Combine (this.environment.ContentRootPath, ".\\ins1teg.pfx"), "India1947"))

				// ↓↓ CATCH: A list of API services resources (the master API service + all module API services resources).
				// Tells this IDP as to which all API app resources that this IDP is going to protect.
				.AddInMemoryApiResources (appAndServicesDataProvider.GetApiResourcesOfProtectedServices ())

				// ↓↓ CATCH: A list of web clients (master web client + all module web clients).
				// Tells this IDP as to which all web clients that this IDP is going to protect.
				.AddInMemoryClients (appAndServicesDataProvider.GetClientsOfProtectedApps ())

				// ↓↓ INCOMPLETE CATCH: Tells this IDP as to what are the "resources associated with the authenticated users"
				// that this IDP is going to mention in the list of JWT's that are sent back after authentication.
				.AddInMemoryIdentityResources (appAndServicesDataProvider.GetIdentityResources ())

				// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: Helps this IDP link to the CMS User Store, from where it can find
				// necessary information related to the signing in user. This is an extension method that you wrote
				// and is available inside IdentityServerBuilderExtensions static class.
				.AddCmsUserStore ();

			services
				.AddMvc ();
				// Tells that this IDP is going to use MVC to show its authentication and consent screens.
		}

		/// <summary>
		/// Helps configure this web app.
		/// </summary>
		/// <param name="app">The application.</param>
		/// <param name="env">The environment.</param>
		public void Configure
			(
				IApplicationBuilder app,
				IHostingEnvironment env
			)
		{
			if (env.IsDevelopment ())
			{
				app.UseDeveloperExceptionPage ();
			}

			app.UseIdentityServer ();
			app.UseStaticFiles ();
			app.UseMvcWithDefaultRoute ();
		}
	}
}

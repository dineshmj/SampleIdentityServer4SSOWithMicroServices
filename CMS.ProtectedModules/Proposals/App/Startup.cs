using System;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Joonasw.AspNetCore.SecurityHeaders;

using FourWallsInc.Entity.SSO;
using FourWallsInc.WebUI.Cookies;

namespace CMS.Proposals.App
{
	public class Startup
	{
		private readonly IConfiguration configuration;
		private AppPair moduleAppPair;
		private string idpServerUri;

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
			// Read the module info from configuration.
			services.Configure<AppPair> (this.configuration.GetSection ("ModuleInfo"));

			this.moduleAppPair = services.BuildServiceProvider ().GetService<IOptions<AppPair>> ().Value;
			this.idpServerUri = this.configuration.GetSection ("IdpServerUri").Value;

			services
				.AddAuthentication
					(
						// Tells this web app (i.e., Master Web App) as to how to get authenticated.
						options =>
						{
							options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
							options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
							// The above statement mandates that this Web App requires OpenID Connect based
							// authentication, and it should be set to the authentication cookie.
						}
					)
				.AddCookie
					(
						options =>
						{
							options.ExpireTimeSpan = TimeSpan.FromMinutes (60);
							options.SessionStore = new MemoryCacheTicketStore ();
							// The name of the cookie that this application has to send back, if the
							// incoming request is already authenticated.
						}
					)
				.AddOpenIdConnect
					(
						options =>
							this.SetOpenIdConnectOptionsHybrid (options)
							// Tells this web application that it should expect a hybrid type tokens bag response,
							// which will carry OpenID Connect "id token", "access token", and the "refresh token".
					);

			services.AddMemoryCache ();

			services.AddMvc ();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure (IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment ())
			{
				app.UseDeveloperExceptionPage ();
				app.UseBrowserLink ();
			}
			else
			{
				app.UseExceptionHandler ("/Home/Error");
			}

			//JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
			app.UseCsp (csp =>
			{
				csp.AllowFrames.FromAnywhere ();
				csp.AllowScripts
						.FromSelf ()
						.From ("ajax.aspnetcdn.com");
				csp.AllowStyles
						.FromSelf ()
						.From ("ajax.aspnetcdn.com");
			});

			app.UseAuthentication ();
			app.UseStaticFiles ();


			//app.UseWebSockets ();
			//app.UseSignalR
			//	(
			//		routes =>
			//		{
			//			routes.MapHub<PaymentInfoOutputQueueMonitorHub> ("hubs");
			//		}
			//	);
			app.UseMvc
				(
					routes =>
					{
						routes.MapRoute
							(
								name: "default",
								template: "{controller=Home}/{action=Index}/{id?}"
							);
						//template: "{controller=PaymentInfo}/{action=Create}/{id?}");
					}
				);
		}

		#region Private methods.

		/// <summary>
		/// Sets the 
		/// </summary>
		/// <param name="options"></param>
		private void SetOpenIdConnectOptionsHybrid (OpenIdConnectOptions options)
		{
			// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: Tells as to which IDP is to be used for authentication.
			options.Authority = this.idpServerUri;

			options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			// Instructs that this web app (i.e., the Master Web App) should use authentication cookies
			// in the subsequent interactions with the web browser.

			options.RequireHttpsMetadata = false;

			// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: Sets up the client ID of this web app (i.e., the Master Web App).
			// NOTE: This should be same as what the hosting IDP recognizes.
			options.ClientId = this.moduleAppPair.App.ClientId;

			// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: This client secret string should be same as what the hosting IDP recognizes for this
			// web app.
			options.ClientSecret = this.moduleAppPair.App.ClientSecret;

			options.ResponseType = "code id_token";
			// The string "code id_token" refers to the "hybrid flow", where the IDP is expected to send
			// the ID token, the Access Token and the Refresh Token.

			// Causes this web app (Master web app) to publish that the Master API app is in scope
			// for this web app.
			options.Scope.Add (this.moduleAppPair.Api.ResourceName);

			options.Scope.Add ("offline_access");
			// This is necessary for this web app to be able to trade the "refresh token" for a fresh "access token"
			// if the access token becomes stale.

			options.Scope.Add ("email");
			options.Scope.Add ("profile");

			options.SaveTokens = true;
			options.GetClaimsFromUserInfoEndpoint = true;
		}

		/// <summary>
		/// These are used only if we are exposing this web app as a web app instead of as an API app.
		/// </summary>
		/// <param name="options"></param>
		private void SetOpenIdConnectOptionsImplicit (OpenIdConnectOptions options)
		{
			options.Authority = this.idpServerUri;
			options.SignInScheme = "Cookies";
			options.RequireHttpsMetadata = false;
			options.ResponseType = "id_token";
			options.ClientId = this.moduleAppPair.App.ClientId;
			options.SaveTokens = true;
		}

		#endregion
	}
}

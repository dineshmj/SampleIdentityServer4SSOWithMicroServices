using System;
using System.IdentityModel.Tokens.Jwt;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using FourWallsInc.Entity.SSO;
using FourWallsInc.WebUI.Cookies;
using FourWallsInc.Infrastructure.ConfigMgmt;

namespace CMS.Master.App
{
	/// <summary>
	/// Helps configure this web app (Master web app) the exact information that it has to
	/// exchange with the protecting IDP.
	/// </summary>
	public class Startup
	{
		private readonly IConfiguration configuration;
		private AppPair masterAppPair;
		private string idpServerUri;

		public Startup
			(
				IConfiguration configuration
			)
		{
			this.configuration = configuration;
		}

		#region Methods.

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure (IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment ())
			{
				app.UseDeveloperExceptionPage ();
				app.UseBrowserLink ();
			}else
			{
				app.UseExceptionHandler ("/Home/Error");
			}

			JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear ();

			// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: Tells this web app that authentication is required.
			// If not found authenticated, the app would send a 302 - Found response to instruct the
			// web browser to go to the IDP login URL to get authenticated.
			app.UseAuthentication ();

			app.UseStaticFiles ();

			app.UseMvc
				(
					routes =>
					{
						routes.MapRoute
						(
								name: "default",
							template: "{controller=BrandInfo}/{action=Index}/{id?}"
						);
					}
				);
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices (IServiceCollection services)
		{
			services.Configure<AppPair> (this.configuration.GetSection ("MasterLayoutInfo"));

			services.AddScoped<IConfigManager, DefaultConfigManager> ();

			this.masterAppPair = services.BuildServiceProvider ().GetService<IOptions<AppPair>> ().Value;
			this.idpServerUri = this.configuration.GetSection ("IdpServerUri").Value;

			services
				.AddAuthentication
					(
						// Tells this web app (i.e., Master Web App) as to how to get authenticated.
						options =>
						{
							// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: The above statement mandates that this Web App requires
							// OpenID Connect based authentication, and it should be set to the authentication cookie.
							options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
							options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
						}
					)
				.AddCookie
					(
						options =>
						{
							// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: Please remember to NEVER EVER GIVE A NAME to the cookie
							// that this application has to send back. It should "append" the cookie info into the
							// existing cookie that came with the HTTP request, if the incoming request is already
							// authenticated. You do not see "options.CookieName = " because of the above reason.
							options.ExpireTimeSpan = TimeSpan.FromMinutes (60);
							options.SessionStore = new MemoryCacheTicketStore ();
						}
					)
				.AddOpenIdConnect
					(
						options =>
							// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: Tells this web application that it should expect a hybrid type
							// tokens bag response, which will carry OpenID Connect "id token", "access token", and the
							// "refresh token".
							this.SetOpenIdConnectOptionsHybrid (options)
					);

			services.AddMemoryCache ();
			services.AddMvc ();
			// Tells this application that it should use MVC based UIs.
		}

		#endregion

		#region Private methods.

		/// <summary>
		/// Sets the 
		/// </summary>
		/// <param name="options"></param>
		private void SetOpenIdConnectOptionsHybrid (OpenIdConnectOptions options)
		{
			// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: Tells as to which IDP is to be used for authentication.
			options.Authority = this.idpServerUri;

			// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: Instructs that this web app (i.e., the Master Web App) should use
			// authentication cookies in the subsequent interactions with the web browser.
			options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

			options.RequireHttpsMetadata = false;

			options.ClientId = this.masterAppPair.App.ClientId;
			// Sets up the client ID of this web app (i.e., the Master Web App).
			// NOTE: This should be same as what the hosting IDP recognizes.

			options.ClientSecret = this.masterAppPair.App.ClientSecret;
			// This client secret string should be same as what the hosting IDP recognizes for this
			// web app.

			options.ResponseType = "code id_token";
			// The string "code id_token" refers to the "hybrid flow", where the IDP is expected to send
			// the ID token, the Access Token and the Refresh Token.

			options.Scope.Add (this.masterAppPair.Api.ResourceName);
			// Causes this web app (Master web app) to publish that the Master API app is in scope
			// for this web app.

			// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: If this web app consumes the APIs exposed by other modules' API apps,
			// this is the place where you mention the resources of those API services.
			foreach (var oneResourceName in this.masterAppPair.AdditionalApiResourcesThatWouldBeUsed)
			{
				options.Scope.Add (oneResourceName);
			}

			// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: This is necessary for this web app to be able to trade
			// the "refresh token" for a fresh "access token" if the access token becomes stale.
			options.Scope.Add ("offline_access");

			options.Scope.Add ("email");
			options.Scope.Add ("profile");

			options.SaveTokens = true;
			options.GetClaimsFromUserInfoEndpoint = true;
		}

		#endregion
	}
}

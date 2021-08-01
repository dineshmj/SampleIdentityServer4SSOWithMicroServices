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

namespace CMS.Quotes.App
{
	public class Startup
	{
		private readonly IConfiguration configuration;
		private AppPair moduleAppPair;
		private string idpServerUri;

		public Startup (IConfiguration configuration)
		{
			this.configuration = configuration;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices (IServiceCollection services)
		{
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
							// options.Cookie.Name = this.moduleAppPair.App.ClientId;
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
			app.UseCors
			  (
				  builder =>
					  builder.WithOrigins (this.idpServerUri)          // URL of the master web app.
						  .AllowAnyHeader ()
			  );

			app.UseSecurityHeadersMiddleware
				(
					new CustomResponseHeadersBuilder ()
					  .AddDefaultSecurePolicy ()
					  .AddCustomHeader ("Access-Control-Allow-Origin", "*")
				);

			if (env.IsDevelopment ())
			{
				app.UseDeveloperExceptionPage ();
				app.UseBrowserLink ();
			}
			else
			{
				app.UseExceptionHandler ("/Home/Error");
			}

			app.UseCsp (csp => {
					 csp.AllowFrames.FromAnywhere ();
					 csp.AllowScripts
							 .FromSelf ()
							 .From ("ajax.aspnetcdn.com");
					 csp.AllowStyles
							 .FromSelf ()
							 .From ("ajax.aspnetcdn.com");
				 });

			//JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

			app.UseAuthentication ();
			app.UseStaticFiles ();

			app.UseMvc (routes => {
					 routes.MapRoute
						 (
							  name: "default",
							  template: "{controller=Home}/{action=Index}/{id?}"
						 );
				 });
		}

		private void SetOpenIdConnectOptionsHybrid (OpenIdConnectOptions options)
		{
			options.Authority = this.idpServerUri;
			// Tells as to which IDP is to be used for authentication.

			options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			// Instructs that this web app (i.e., the Master Web App) should use authentication cookies
			// in the subsequent interactions with the web browser.

			options.RequireHttpsMetadata = false;

			options.ClientId = this.moduleAppPair.App.ClientId;
			// Sets up the client ID of this web app (i.e., the Master Web App).
			// NOTE: This should be same as what the hosting IDP recognizes.

			options.ClientSecret = this.moduleAppPair.App.ClientSecret;
			// This client secret string should be same as what the hosting IDP recognizes for this
			// web app.

			options.ResponseType = "code id_token";
			// The string "code id_token" refers to the "hybrid flow", where the IDP is expected to send
			// the ID token, the Access Token and the Refresh Token.

			options.Scope.Add (this.moduleAppPair.Api.ResourceName);

			options.Scope.Add ("offline_access");
			// This is necessary for this web app to be able to trade the "refresh token" for a fresh "access token"
			// if the access token becomes stale.

			options.Scope.Add ("email");
			options.Scope.Add ("profile");

			options.SaveTokens = true;
			options.GetClaimsFromUserInfoEndpoint = true;
		}
	}
}

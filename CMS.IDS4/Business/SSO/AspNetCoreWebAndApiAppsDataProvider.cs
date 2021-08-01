using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Options;

using IdentityServer4;
using IdentityServer4.Models;

using FourWallsInc.Entity.SSO;

namespace CMS.IDP.App.Business.SSO
{
	// Contains implemnetations for fetching of API resources and clients
	// of the protected applications.
	public sealed class AspNetCoreWebAndApiAppsDataProvider
		: IAspNetCoreWebAndApiAppsDataProvider
	{
		private readonly SsoInfo ssoInfo;

		public AspNetCoreWebAndApiAppsDataProvider (IOptions<SsoInfo> ssoInfoOptions)
		{
			// Get the SSO info from appSettings.json.
			this.ssoInfo = ssoInfoOptions.Value;
		}

		// Gets the API resources of protected services.
		public IEnumerable<ApiResource> GetApiResourcesOfProtectedServices ()
		{
			var apiResourcesList
				= new List<ApiResource>
					{
						// Get the master layout application's API resource.
						new ApiResource
							(
								// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: The Master API App.
								ssoInfo.MasterLayoutApp.Api.ResourceName,
								ssoInfo.MasterLayoutApp.Api.ResourceFriendlyName
							)
					};

			// Get the individual module applications' API resources.
			ssoInfo
				.ModuleApps
				.ToList ()
				.ForEach
				(
					appPair =>
					{
						apiResourcesList.Add
							(
								new ApiResource
								(
									// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: Individual module API app's resource.
									appPair.Api.ResourceName,
									appPair.Api.ResourceFriendlyName
								)
							);
					}
				);

			return apiResourcesList;
		}

		// Gets the clients of protected apps.
		public IEnumerable<Client> GetClientsOfProtectedApps ()
		{
			// Prepare a fresh list of clients.
			var clients
				= new List<Client>
					{
						// Add the master layout application's client.
						this.GetMasterClient ()
					};

			// Add the individual module application clients.
			clients.AddRange (this.GetModuleClients ());

			return clients;
		}

		// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ INCOMPLETE CATCH: Gets the identity resource.
		public IEnumerable<IdentityResource> GetIdentityResources ()
		{
			return new IdentityResource []
				{
					new IdentityResources.OpenId (),	// The OpenID Connect unique ID after authentication.
					new IdentityResources.Profile (),	// The authenticated user's profile.
					new IdentityResources.Email ()		// Email.
				};
		}

		// Gets the master client.
		private Client GetMasterClient ()
		{
			var masterClient
				= new Client
				{
					// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: The web client ID.
					ClientId = this.ssoInfo.MasterLayoutApp.App.ClientId, // Master Web App.

					// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: The above setting causes the Master Web App to behave like an API App.
					// In Hybrid mode, the client (i.e., here the Master Web App) gets three different JWT's, which are "Identity Token",
					// "Access Token", and the "Refresh Token" as part of the bag of JWT's that the protected applications receive.
					AllowedGrantTypes = GrantTypes.Hybrid,

					ClientSecrets =
					{
						// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: The web client secret key.
						new Secret (this.ssoInfo.MasterLayoutApp.App.ClientSecret.Sha256 ())
					},

					// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: The API resources that this web client is allowed to call.
					AllowedScopes = this.GetMasterClientScopes (),

					// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: Indicates if the "consent page" is to be shown or not.
					RequireConsent = false,

					// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ INCOMPLETE CATCH: Need to study the purpose of the assignment below.
					AllowOfflineAccess = true,      // Indicates that the protected client can use the "Refresh Token", if the current
													// Identity Token and Access Token have become stale.

					// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ INCOMPLETE CATCH: Apparently this should be false, if you're using Hybrid approach for the web client.
					// http://docs.identityserver.io/en/latest/reference/client.html
					// AllowAccessTokensViaBrowser = true,			

					// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: This is very important - if you want to restrict the web client
					// MVC controller action methods to be accessible to specific roles, you need to have the claims bundled
					// into the ID token. This is where you tell IDP to do it.
					AlwaysIncludeUserClaimsInIdToken = true,

					RedirectUris = new [] { ssoInfo.MasterLayoutApp.App.RedirectUri },
					// This is the URI tha this IDP has to provide to the web browser, with which the web browser would navigate
					// to the required UI page of the protected web app (i.e., the Master Web App here.)

					PostLogoutRedirectUris = new [] { ssoInfo.MasterLayoutApp.App.SignOutRedirectUri }
				};

			return masterClient;
		}

		// Gets the module clients.
		private IList<Client> GetModuleClients ()
		{
			return
				this.ssoInfo
					.ModuleApps
					.Select
						(
							moduleAppPair =>
							{
								return new Client
								{
									ClientId = moduleAppPair.App.ClientId, // The protected module web app client ID.

									AllowedGrantTypes = GrantTypes.Hybrid,          // See the comment above for master client.
																					// The implicit flow differs from the hybrid flow in only one thing - the "Refresh Token" would not
																					// be sent in the bag of JWT's sent by this IDP.

									ClientSecrets =
									{
										new Secret (moduleAppPair.App.ClientSecret.Sha256 ())
									},

									AllowedScopes = this.GetModuleClientScopes (moduleAppPair),

									RequireConsent = false,             // See the comment above for master client.
									AllowOfflineAccess = true,          // See the comment above for master client.

									// AllowAccessTokensViaBrowser = true,
									AlwaysIncludeUserClaimsInIdToken = true,        // See the comment above for master client.

									RedirectUris = new [] { moduleAppPair.App.RedirectUri },
									// See the commentabove.

									PostLogoutRedirectUris = new [] { moduleAppPair.App.SignOutRedirectUri }
								};
							}
						)
					.ToList ();
		}

		// Gets the master client scopes.
		private string [] GetMasterClientScopes ()
		{
			// Prepare a fresh list of allowed scopes.
			var allowedScopes = new List<string> ();

			// Add the minimum required scopes.
			allowedScopes.AddRange (this.GetMinimumRequiredScopes ());

			// Add master layout application's API service as one of the allowed scopes.
			allowedScopes.Add (this.ssoInfo.MasterLayoutApp.Api.ResourceName);

			// Add other protected module applications' API services as the allowed scopes
			// for the master client (assuming that the master client would require to consume
			// the services of the protected modules).
			this.ssoInfo
				.ModuleApps
				.ToList ()
				.ForEach
					(
						oneModuleApp =>
							allowedScopes.Add (oneModuleApp.Api.ResourceName)
					);

			return (allowedScopes.ToArray ());
		}

		// Gets the module client scopes.
		private string [] GetModuleClientScopes (AppPair moduleAppPair)
		{
			// Prepare a fresh list of allowed scopes.
			var allowedScopes = new List<string> ();

			// Add the minimum required scopes.
			allowedScopes.AddRange (this.GetMinimumRequiredScopes ());

			// Add module application's API service as one of the allowed scopes.
			allowedScopes.Add (moduleAppPair.Api.ResourceName);

			return allowedScopes.ToArray ();
		}

		// Gets the minimum required scopes for a protected client application.
		private string [] GetMinimumRequiredScopes ()
		{
			return
				(
					new []
					{
						IdentityServerConstants.StandardScopes.OpenId,				// Open ID Connect ID.
						IdentityServerConstants.StandardScopes.Profile,				// Authenticated user's profile.
						IdentityServerConstants.StandardScopes.Email				// Email.
					}
				);
		}
	}
}

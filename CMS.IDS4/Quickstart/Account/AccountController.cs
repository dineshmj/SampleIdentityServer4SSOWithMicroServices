// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Test;

using CMS.IDP.App.Business;

namespace IdentityServer4.Quickstart.UI
{
	/// <summary>
	/// This sample controller implements a typical login/logout/provision workflow for local and external accounts.
	/// The login service encapsulates the interactions with the user data store. This data store is in-memory only and cannot be used for production!
	/// The interaction service provides a way for the UI to communicate with identityserver for validation and context retrieval
	/// </summary>
	[SecurityHeaders]
	public class AccountController
		: Controller
	{
		private readonly ICmsAuthenticationBizFacade cmsAuthBizFacade;
		private readonly IIdentityServerInteractionService interaction;
		private readonly IClientStore clientStore;
		private readonly IAuthenticationSchemeProvider schemeProvider;
		private readonly IEventService events;

		public AccountController
			(
				IIdentityServerInteractionService interaction,
				IClientStore clientStore,
				IAuthenticationSchemeProvider schemeProvider,
				IEventService events,
				// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: You're injecting cms authentication biz facade to the Account MVC Controller.
				// This will help the Account controller to get necessary authentication related services.
				ICmsAuthenticationBizFacade cmsAuthBizFacade
			)
		{
			this.interaction = interaction;
			this.clientStore = clientStore;
			this.schemeProvider = schemeProvider;
			this.events = events;

			// ↓↓ CATCH: This is where you would plug in your own custom identity management library (e.g. ASP.NET Identity)
			this.cmsAuthBizFacade = cmsAuthBizFacade;
		}

		/// <summary>
		/// Show login page.
		/// </summary>
		[HttpGet]
		public async Task<IActionResult> Login (string returnUrl)
		{
			// Build a model so we know what to show on the login page.
			var loginViewModel = await this.BuildLoginViewModelAsync (returnUrl);

			if (loginViewModel.IsExternalLoginOnly)
			{
				// we only have one option for logging in and it's an external provider
				return await this.ExternalLogin (loginViewModel.ExternalLoginScheme, returnUrl);
			}

			return base.View (loginViewModel);
		}

		/// <summary>
		/// Handles postback from username / password login page.
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login (LoginInputModel model, string button)
		{
			// Did the user click on "Cancel"?
			if (button != "login")
			{
				// Yes.
				var context = await interaction.GetAuthorizationContextAsync (model.ReturnUrl);

				if (context != null)
				{
					// If the user cancels, send a result back into IdentityServer as if they 
					// denied the consent (even if this client does not require consent).
					// this will send back an access denied OIDC error response to the client.
					await interaction.GrantConsentAsync (context, ConsentResponse.Denied);

					// We can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
					return base.Redirect (model.ReturnUrl);
				}
				else
				{
					// Since we don't have a valid context, then we just go back to the home page
					return base.Redirect ("~/");
				}
			}

			// The user clicked on "Login" button.
			if (base.ModelState.IsValid)
			{
				// // ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: Validate username / password against application's user store.
				if (this.cmsAuthBizFacade.ValidateCredentials (model.Username, model.Password))
				{
					// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: The statement below fetches the signed in user's details, which
					// would be worked upon by this IDP for sending the list of available roles, etc.
					var lobApplicationUser = this.cmsAuthBizFacade.FindUserByLoginId (model.Username);

					await events.RaiseAsync
						(
							new UserLoginSuccessEvent
							(
								lobApplicationUser.Username,
								lobApplicationUser.SubjectId,
								lobApplicationUser.Username
							)
						);

					// Only set explicit expiration here if user chooses "remember me". 
					// Otherwise we rely upon expiration configured in cookie middleware.
					AuthenticationProperties props = null;

					if (AccountOptions.AllowRememberLogin && model.RememberLogin)
					{
						props = new AuthenticationProperties
						{
							IsPersistent = true,
							ExpiresUtc = DateTimeOffset.UtcNow.Add (AccountOptions.RememberMeLoginDuration)
						};
					};

					// // ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: Issue authentication cookie with subject ID and username
					await base.HttpContext.SignInAsync (lobApplicationUser.SubjectId, lobApplicationUser.Username, props);

					// // ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: Make sure the returnUrl is still valid, and if so redirect back to
					// authorize endpoint or a local page the IsLocalUrl check is only necessary if you want to support
					// additional local pages, otherwise IsValidReturnUrl is more strict
					if (interaction.IsValidReturnUrl (model.ReturnUrl) || Url.IsLocalUrl (model.ReturnUrl))
					{
						return base.Redirect (model.ReturnUrl);
					}

					return base.Redirect ("~/");
				}

				await events.RaiseAsync (new UserLoginFailureEvent (model.Username, "invalid credentials"));

				base.ModelState.AddModelError ("", AccountOptions.InvalidCredentialsErrorMessage);
			}

			// something went wrong, show form with error
			var loginViewModel = await this.BuildLoginViewModelAsync (model);
			return base.View (loginViewModel);
		}

		/// <summary>
		/// initiate roundtrip to external authentication provider
		/// </summary>
		[HttpGet]
		public async Task<IActionResult> ExternalLogin (string provider, string returnUrl)
		{
			if (AccountOptions.WindowsAuthenticationSchemeName == provider)
			{
				// Windows authentication needs special handling
				return await this.ProcessWindowsLoginAsync (returnUrl);
			}
			else
			{
				// Start challenge and roundtrip the return URL
				var props = new AuthenticationProperties ()
				{
					RedirectUri = Url.Action ("ExternalLoginCallback"),
					Items =
					{
						{ "returnUrl", returnUrl },
						{ "scheme", provider },
					}
				};

				return base.Challenge (props, provider);
			}
		}

		/// <summary>
		/// Post processing of external authentication
		/// </summary>
		[HttpGet]
		public async Task<IActionResult> ExternalLoginCallback ()
		{
			// Read external identity from the temporary cookie
			var result
				= await base.HttpContext.AuthenticateAsync
					(
						IdentityServerConstants.ExternalCookieAuthenticationScheme
					);

			if (result?.Succeeded != true)
			{
				throw new Exception ("External authentication error");
			}

			// Look up our user and external provider info
			var (user, provider, providerUserId, claims) = FindUserFromExternalProvider (result);

			if (user == null)
			{
				// This might be where you might initiate a custom workflow for user registration
				// in this sample we don't show how that would be done, as our sample implementation
				// simply auto-provisions new external user
				user = this.AutoProvisionUser (provider, providerUserId, claims);
			}

			// This allows us to collect any additonal claims or properties
			// for the specific prtotocols used and store them in the local auth cookie.
			// this is typically used to store data needed for signout from those protocols.
			var additionalLocalClaims = new List<Claim> ();
			var localSignInProps = new AuthenticationProperties ();

			this.ProcessLoginCallbackForOidc (result, additionalLocalClaims, localSignInProps);
			this.ProcessLoginCallbackForWsFed (result, additionalLocalClaims, localSignInProps);
			this.ProcessLoginCallbackForSaml2p (result, additionalLocalClaims, localSignInProps);

			// issue authentication cookie for user
			await events.RaiseAsync (new UserLoginSuccessEvent (provider, providerUserId, user.SubjectId, user.Username));
			await base.HttpContext.SignInAsync (user.SubjectId, user.Username, provider, localSignInProps, additionalLocalClaims.ToArray ());

			// delete temporary cookie used during external authentication
			await base.HttpContext.SignOutAsync (IdentityServerConstants.ExternalCookieAuthenticationScheme);

			// validate return URL and redirect back to authorization endpoint or a local page
			var returnUrl = result.Properties.Items ["returnUrl"];

			if (interaction.IsValidReturnUrl (returnUrl) || Url.IsLocalUrl (returnUrl))
			{
				return base.Redirect (returnUrl);
			}

			return base.Redirect ("~/");
		}

		/// <summary>
		/// Show logout page
		/// </summary>
		[HttpGet]
		public async Task<IActionResult> Logout (string logoutId)
		{
			// Build a model so the logout page knows what to display
			var logoutViewModel = await this.BuildLogoutViewModelAsync (logoutId);

			if (logoutViewModel.ShowLogoutPrompt == false)
			{
				// If the request for logout was properly authenticated from IdentityServer, then
				// we don't need to show the prompt and can just log the user out directly.
				return await this.Logout (logoutViewModel);
			}

			return base.View (logoutViewModel);
		}

		/// <summary>
		/// Handle logout page postback
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout (LogoutInputModel model)
		{
			// Build a model so the logged out page knows what to display
			var loggedOutViewModel = await this.BuildLoggedOutViewModelAsync (model.LogoutId);

			if (base.User?.Identity.IsAuthenticated == true)
			{
				// Delete local authentication cookie
				await base.HttpContext.SignOutAsync ();

				// Raise the logout event
				await events.RaiseAsync (new UserLogoutSuccessEvent (User.GetSubjectId (), User.GetDisplayName ()));
			}

			// Check if we need to trigger sign-out at an upstream identity provider
			if (loggedOutViewModel.TriggerExternalSignout)
			{
				// Build a return URL so the upstream provider will redirect back
				// to us after the user has logged out. this allows us to then
				// complete our single sign-out processing.
				string url = base.Url.Action ("Logout", new { logoutId = loggedOutViewModel.LogoutId });

				// This triggers a redirect to the external provider for sign-out
				return base.SignOut (new AuthenticationProperties { RedirectUri = url }, loggedOutViewModel.ExternalAuthenticationScheme);
			}

			return base.View ("LoggedOut", loggedOutViewModel);
		}

		/*****************************************/
		/* Helper APIs for the AccountController */
		/*****************************************/
		private async Task<LoginViewModel> BuildLoginViewModelAsync (string returnUrl)
		{
			var context = await interaction.GetAuthorizationContextAsync (returnUrl);
			if (context?.IdP != null)
			{
				// This is meant to short circuit the UI and only trigger the one external IdP
				return new LoginViewModel
				{
					EnableLocalLogin = false,
					ReturnUrl = returnUrl,
					Username = context?.LoginHint,
					ExternalProviders = new ExternalProvider [] { new ExternalProvider { AuthenticationScheme = context.IdP } }
				};
			}

			var schemes = await schemeProvider.GetAllSchemesAsync ();

			var providers = schemes
				.Where
				(
					x =>
						x.DisplayName != null
						|| (x.Name.Equals (AccountOptions.WindowsAuthenticationSchemeName, StringComparison.OrdinalIgnoreCase))
				)
				.Select
				(
					x =>
						new ExternalProvider
						{
							DisplayName = x.DisplayName,
							AuthenticationScheme = x.Name
						}
				)
				.ToList ();

			var allowLocal = true;

			if (context?.ClientId != null)
			{
				var client = await clientStore.FindEnabledClientByIdAsync (context.ClientId);

				if (client != null)
				{
					allowLocal = client.EnableLocalLogin;

					if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any ())
					{
						providers = providers.Where (provider => client.IdentityProviderRestrictions.Contains (provider.AuthenticationScheme)).ToList ();
					}
				}
			}

			return new LoginViewModel
			{
				AllowRememberLogin = AccountOptions.AllowRememberLogin,
				EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
				ReturnUrl = returnUrl,
				Username = context?.LoginHint,
				ExternalProviders = providers.ToArray ()
			};
		}

		private async Task<LoginViewModel> BuildLoginViewModelAsync (LoginInputModel model)
		{
			var vm = await BuildLoginViewModelAsync (model.ReturnUrl);
			vm.Username = model.Username;
			vm.RememberLogin = model.RememberLogin;
			return vm;
		}

		private async Task<LogoutViewModel> BuildLogoutViewModelAsync (string logoutId)
		{
			var vm = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

			if (User?.Identity.IsAuthenticated != true)
			{
				// if the user is not authenticated, then just show logged out page
				vm.ShowLogoutPrompt = false;
				return vm;
			}

			var context = await interaction.GetLogoutContextAsync (logoutId);
			if (context?.ShowSignoutPrompt == false)
			{
				// it's safe to automatically sign-out
				vm.ShowLogoutPrompt = false;
				return vm;
			}

			// show the logout prompt. this prevents attacks where the user
			// is automatically signed out by another malicious web page.
			return vm;
		}

		private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync (string logoutId)
		{
			// get context information (client name, post logout redirect URI and iframe for federated signout)
			var logout = await interaction.GetLogoutContextAsync (logoutId);

			var vm = new LoggedOutViewModel
			{
				AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
				PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
				ClientName = string.IsNullOrEmpty (logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
				SignOutIframeUrl = logout?.SignOutIFrameUrl,
				LogoutId = logoutId
			};

			if (User?.Identity.IsAuthenticated == true)
			{
				var idp = User.FindFirst (JwtClaimTypes.IdentityProvider)?.Value;
				if (idp != null && idp != IdentityServerConstants.LocalIdentityProvider)
				{
					var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync (idp);
					if (providerSupportsSignout)
					{
						if (vm.LogoutId == null)
						{
							// if there's no current logout context, we need to create one
							// this captures necessary info from the current logged in user
							// before we signout and redirect away to the external IdP for signout
							vm.LogoutId = await interaction.CreateLogoutContextAsync ();
						}

						vm.ExternalAuthenticationScheme = idp;
					}
				}
			}

			return vm;
		}

		private async Task<IActionResult> ProcessWindowsLoginAsync (string returnUrl)
		{
			// see if windows auth has already been requested and succeeded
			var result = await HttpContext.AuthenticateAsync (AccountOptions.WindowsAuthenticationSchemeName);
			if (result?.Principal is WindowsPrincipal wp)
			{
				// we will issue the external cookie and then redirect the
				// user back to the external callback, in essence, tresting windows
				// auth the same as any other external authentication mechanism
				var props = new AuthenticationProperties ()
				{
					RedirectUri = Url.Action ("ExternalLoginCallback"),
					Items =
					{
						{ "returnUrl", returnUrl },
						{ "scheme", AccountOptions.WindowsAuthenticationSchemeName },
					}
				};

				var id = new ClaimsIdentity (AccountOptions.WindowsAuthenticationSchemeName);
				id.AddClaim (new Claim (JwtClaimTypes.Subject, wp.Identity.Name));
				id.AddClaim (new Claim (JwtClaimTypes.Name, wp.Identity.Name));

				// add the groups as claims -- be careful if the number of groups is too large
				if (AccountOptions.IncludeWindowsGroups)
				{
					var wi = wp.Identity as WindowsIdentity;
					var groups = wi.Groups.Translate (typeof (NTAccount));
					var roles = groups.Select (x => new Claim (JwtClaimTypes.Role, x.Value));
					id.AddClaims (roles);
				}

				await HttpContext.SignInAsync (
					IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme,
					new ClaimsPrincipal (id),
					props);
				return Redirect (props.RedirectUri);
			}
			else
			{
				// trigger windows auth
				// since windows auth don't support the redirect uri,
				// this URL is re-triggered when we call challenge
				return Challenge (AccountOptions.WindowsAuthenticationSchemeName);
			}
		}

		private (TestUser user, string provider, string providerUserId, IEnumerable<Claim> claims) FindUserFromExternalProvider (AuthenticateResult result)
		{
			var externalUser = result.Principal;

			// try to determine the unique id of the external user (issued by the provider)
			// the most common claim type for that are the sub claim and the NameIdentifier
			// depending on the external provider, some other claim type might be used
			var userIdClaim = externalUser.FindFirst (JwtClaimTypes.Subject) ??
							  externalUser.FindFirst (ClaimTypes.NameIdentifier) ??
							  throw new Exception ("Unknown userid");

			// remove the user id claim so we don't include it as an extra claim if/when we provision the user
			var claims = externalUser.Claims.ToList ();
			claims.Remove (userIdClaim);

			var provider = result.Properties.Items ["scheme"];
			var providerUserId = userIdClaim.Value;

			// find external user
			// NOTE: This line is commented out ===>>>    var user = _users.FindByExternalProvider(provider, providerUserId);

			// return (user, provider, providerUserId, claims);
			return (null, provider, providerUserId, claims);            // <==== NOTE: temporary fix.
		}

		private TestUser AutoProvisionUser (string provider, string providerUserId, IEnumerable<Claim> claims)
		{
			// NOTE: This line is commented out ===>>>    var user = _users.AutoProvisionUser(provider, providerUserId, claims.ToList());
			// return user;
			return null;            // <==== NOTE: temporary fix.
		}

		private void ProcessLoginCallbackForOidc (AuthenticateResult externalResult, List<Claim> localClaims, AuthenticationProperties localSignInProps)
		{
			// if the external system sent a session id claim, copy it over
			// so we can use it for single sign-out
			var sid = externalResult.Principal.Claims.FirstOrDefault (x => x.Type == JwtClaimTypes.SessionId);
			if (sid != null)
			{
				localClaims.Add (new Claim (JwtClaimTypes.SessionId, sid.Value));
			}

			// if the external provider issued an id_token, we'll keep it for signout
			var id_token = externalResult.Properties.GetTokenValue ("id_token");
			if (id_token != null)
			{
				localSignInProps.StoreTokens (new [] { new AuthenticationToken { Name = "id_token", Value = id_token } });
			}
		}

		private void ProcessLoginCallbackForWsFed (AuthenticateResult externalResult, List<Claim> localClaims, AuthenticationProperties localSignInProps)
		{
		}

		private void ProcessLoginCallbackForSaml2p (AuthenticateResult externalResult, List<Claim> localClaims, AuthenticationProperties localSignInProps)
		{
		}
	}
}

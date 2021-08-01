using System;
using System.Linq;
using System.Threading.Tasks;

using IdentityServer4.Models;
using IdentityServer4.Services;

namespace CMS.IDP.App.Business
{
	// Contains necessary logic to connect this IDP to the protected applications' DB store
	// where this IDP can get to know about the user's details, roles, etc.
	public class CmsUserProfileService
		: IProfileService
	{
		private readonly ICmsAuthenticationBizFacade cmsAuthBizFacade;

		public CmsUserProfileService (ICmsAuthenticationBizFacade cmsAuthBizFacade)
		{
			this.cmsAuthBizFacade = cmsAuthBizFacade;
		}

		// This method is called whenever claims about the user are requested
		// (e.g., during token creation or via the userinfo endpoint.)
		public Task GetProfileDataAsync (ProfileDataRequestContext context)
		{
			// Get the login ID.
			var loginId = context.Subject.Identity.Name;

			// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ CRISIS: Occassionally, the context.Subject.Identiy.Name will be NULL by the
			// time this code gets executed. All other details (such as, first name, last name,
			// email address, subject ID, roles, etc.) would be remembered in the identity surprisingly.
			// In such an event, take the signed in user login ID from the "subject" claim of the user.
			if (String.IsNullOrEmpty (loginId))
			{
				// https://github.com/IdentityServer/IdentityServer3/issues/1938
				// Get the Subject ID from the claims.
				var claim = context.Subject.FindFirst (item => item.Type == "sub");

				if (claim != null)
				{
					loginId = claim.Value;
				}
			}

			// Get the signed in user's details.
			var signedInLobApplicationUser = this.cmsAuthBizFacade.FindUserByLoginId (loginId);

			// Get all the claims that this authenticated user has.
			context.IssuedClaims = signedInLobApplicationUser?.Claims.ToList ();

			return Task.FromResult (0);
		}

		// This method gets called whenever identity server needs to determine if the user
		// is valid or active (e.g. if the user's account has been deactivated since they
		// logged in). (e.g. during token issuance or validation).
		public Task IsActiveAsync (IsActiveContext context)
		{
			// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: As of now, this is hard-coded to TRUE. You can run your logic
			// here to ascertain if the user is active or not, and accordingly set the value to TRUE or FALSE.
			context.IsActive = true;

			return Task.FromResult (0);
		}
	}
}

using System.Collections.Generic;
using System.Security.Claims;

using FourWallsInc.Entity.SSO;
using CMS.IDP.App.DataAccess;

namespace CMS.IDP.App.Business
{
	// This class contains throw-away business code that goes to the
	// LoB application's DB repository, and gets the signing in user's details.
	public sealed class CmsAuthenticationBizFacade
		: ICmsAuthenticationBizFacade
	{
		private readonly ICmsAuthenticationDataAccess authDataAccess;

		public CmsAuthenticationBizFacade (ICmsAuthenticationDataAccess authDataAccess)
		{
			this.authDataAccess = authDataAccess;
		}

		// Validates the specified credentials of the signing in user.
		public bool ValidateCredentials (string loginId, string password)
		{
			var credentialsValid = this.authDataAccess.AreCmsCredentialsValid (loginId, password);
			return (credentialsValid);
		}

		// Gets a LoB application user matching to the specified login ID.
		public LobApplicationUser FindUserByLoginId (string loginId)
		{
			// Identify user and roles.
			var cmsUser = this.authDataAccess.GetCmsUserOf (loginId);
			var rolesFromDb = this.authDataAccess.GetRolesOf (loginId);

			// Prepare SSO claims based on roles.
			var claims = new List<Claim> ();

			// Add the basic claims.
			claims.AddRange
				(
					new []
					{
						new Claim (ClaimTypes.Email, cmsUser.Email),
						new Claim (ClaimTypes.GivenName, cmsUser.FirstName),
						new Claim (ClaimTypes.Surname, cmsUser.LastName)
					}
				);

			// Add the claims from the DB (i.e., the "roles")
			foreach (var oneRole in rolesFromDb)
			{
				claims.Add (new Claim (ClaimTypes.Role, oneRole.Name));
			}

			// Prepare the LoB application user with claims.
			return
				(
					new LobApplicationUser
					{
						// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ CRISIS: cmsUser.Id.ToString () cannot be specified because login ID is more required often.
						// Check the CRISIS part in the CmsUserProfileService for more details.
						SubjectId = loginId,
						Username = loginId,
						Password = null,			// Deliberately removed the password.
						Claims = claims.ToArray ()
					}
				);
		}
	}
}

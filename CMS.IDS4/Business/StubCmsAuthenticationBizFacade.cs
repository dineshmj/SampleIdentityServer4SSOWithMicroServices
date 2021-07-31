using System.Collections.Generic;
using System.Security.Claims;

using FourWallsInc.Entity.SSO;

namespace CMS.IDP.App.Business
{
	// This class contains throw-away business code that goes to the
	// LoB application's DB repository, and gets the signing in user's details.
	public sealed class StubCmsAuthenticationBizFacade
		: ICmsAuthenticationBizFacade
	{
		// Validates the specified credentials of the signing in user.
		public bool ValidateCredentials (string loginId, string password)
		{
			return
				(loginId == "christopher" && password == "123")
				|| (loginId == "tom" && password == "123")
				|| (loginId == "steven" && password == "123")
				|| (loginId == "harrison" && password == "123")
				|| (loginId == "will" && password == "123")
				|| (loginId == "james" && password == "123");
		}

		// Gets a LoB application user matching to the specified login ID.
		public LobApplicationUser FindUserByLoginId (string loginId)
		{
			// Prepare SSO claims based on roles.
			var claims = new List<Claim> ();

			switch (loginId)
			{
				case "christopher":
					claims.AddRange (new [] {
						new Claim (ClaimTypes.Email, "christopher.nolan@fourwallsinc.com"),
						new Claim (ClaimTypes.GivenName, "Christopher"),
						new Claim (ClaimTypes.Surname, "Nolan")
						// No roles for Christopher Nolan.
					});
					break;

				case "tom":
					claims.AddRange (new [] {
						new Claim (ClaimTypes.Email, "tom.hanks@fourwallsinc.com"),
						new Claim (ClaimTypes.GivenName, "Tom"),
						new Claim (ClaimTypes.Surname, "Hanks"),
						// Roles
						new Claim (ClaimTypes.Role, "Quotes Management"),
						new Claim (ClaimTypes.Role, "Quotes Archive"),
						new Claim (ClaimTypes.Role, "Policy Issuance")
					});
					break;

				case "steven":
					claims.AddRange (new [] {
						new Claim (ClaimTypes.Email, "steven.spielberg@fourwallsinc.com"),
						new Claim (ClaimTypes.GivenName, "Steven"),
						new Claim (ClaimTypes.Surname, "Spielberg"),
						// Roles
						new Claim (ClaimTypes.Role, "Proposal Management"),
						new Claim (ClaimTypes.Role, "Broker Management"),
						new Claim (ClaimTypes.Role, "Endorsements"),
					});
					break;

				case "harrison":
					claims.AddRange (new [] {
						new Claim (ClaimTypes.Email, "harrison.ford@fourwallsinc.com"),
						new Claim (ClaimTypes.GivenName, "Harrison"),
						new Claim (ClaimTypes.Surname, "Ford")
						// No roles for Harrison Ford.
					});
					break;

				case "will":
					claims.AddRange (new [] {
						new Claim (ClaimTypes.Email, "will.smith@fourwallsinc.com"),
						new Claim (ClaimTypes.GivenName, "Will"),
						new Claim (ClaimTypes.Surname, "Smith"),
						// Roles
						new Claim (ClaimTypes.Role, "Quotes Management")
					});
					break;

				case "james":
					claims.AddRange (new [] {
						new Claim (ClaimTypes.Email, "james.cameron@fourwallsinc.com"),
						new Claim (ClaimTypes.GivenName, "James"),
						new Claim (ClaimTypes.Surname, "Cameron"),
						// Roles
						new Claim (ClaimTypes.Role, "Claims Management")
					});
					break;
			}

			// Prepare the LoB application user with claims.
			return new LobApplicationUser {
				// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ CRISIS: cmsUser.Id.ToString () cannot be specified because login ID is more required often.
				// Check the CRISIS part in the CmsUserProfileService for more details.
				SubjectId = loginId,
				Username = loginId,
				Password = null,            // Deliberately removed the password.
				Claims = claims.ToArray ()
			};
		}
	}
}

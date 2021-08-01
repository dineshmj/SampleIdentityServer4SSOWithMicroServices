using System.Collections.Generic;

using FourWallsInc.Entity.LobApp.CMS;

namespace CMS.IDP.App.DataAccess
{
	// Abstracts the authentication related tasks.
	public interface ICmsAuthenticationDataAccess
	{
		// Checks if the specified CMS credentials are valid.
		bool AreCmsCredentialsValid (string loginId, string password);

		// Gets the roles of the specified login ID.
		IList<CmsRole> GetRolesOf (string loginId);

		// Gets the CMS user corresponding to the login ID specified.
		CmsUser GetCmsUserOf (string loginId);
	}
}

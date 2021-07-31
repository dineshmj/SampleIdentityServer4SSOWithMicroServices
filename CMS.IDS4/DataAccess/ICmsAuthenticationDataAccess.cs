using System.Collections.Generic;

using FourWallsInc.Entity.LobApp.CMS;

namespace CMS.IDP.App.DataAccess
{
	/// <summary>
	/// Abstracts the authentication related tasks.
	/// </summary>
	public interface ICmsAuthenticationDataAccess
	{
		/// <summary>
		/// Checks if the specified CMS credentials are valid.
		/// </summary>
		/// <param name="loginId">The login ID.</param>
		/// <param name="password">The password.</param>
		/// <returns></returns>
		bool AreCmsCredentialsValid (string loginId, string password);

		/// <summary>
		/// Gets the roles of the specified login ID.
		/// </summary>
		/// <param name="loginId">The login ID.</param>
		/// <returns></returns>
		IList<CmsRole> GetRolesOf (string loginId);

		/// <summary>
		/// Gets the CMS user corresponding to the login ID specified.
		/// </summary>
		/// <param name="loginId">The login ID.</param>
		/// <returns></returns>
		CmsUser GetCmsUserOf (string loginId);
	}
}

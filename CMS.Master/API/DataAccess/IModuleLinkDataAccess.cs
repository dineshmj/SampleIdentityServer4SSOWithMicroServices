using System.Collections.Generic;

using FourWallsInc.Entity.LobApp.CMS;

namespace CMS.Master.Api.DataAccess
{
	/// <summary>
	/// A data access class for module and links info.
	/// </summary>
	public interface IModuleLinkDataAccess
	{
		/// <summary>
		/// Gets the module and link details for the specified login ID.
		/// </summary>
		/// <param name="roleNames">A list of roles to which the signed in user
		/// has access.</param>
		/// <returns></returns>
		IList<CmsModuleAndLinkInfo> GetModuleAndLinkDetailsFor (IList<string> roleNames);
	}
}

using System.Collections.Generic;

using FourWallsInc.Entity.LobApp.CMS;

namespace CMS.Master.Api.Business
{
	/// <summary>
	/// A business facade for managing module and links on the Master UI screen.
	/// </summary>
	public interface ICmsModuleAndLinksBizFacade
	{
		/// <summary>
		/// Gets the module and links details which the logged in user has
		/// access to.
		/// </summary>
		/// <param name="roleNames">A list of role names that are accessible
		/// to the signed in user.</param>
		/// <returns></returns>
		IList<CmsModuleAndLinkInfo> GetModuleAndLinksOf (IList<string> roleNames);
	}
}

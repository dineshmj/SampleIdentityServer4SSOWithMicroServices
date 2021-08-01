using System.Collections.Generic;

using FourWallsInc.Entity.LobApp.CMS;

namespace CMS.Master.Api.Business
{
	// A business facade for managing module and links on the Master UI screen.
	public interface ICmsModuleAndLinksBizFacade
	{
		// Gets the module and links details which the logged in user has
		// access to.
		IList<CmsModuleAndLinkInfo> GetModuleAndLinksOf (IList<string> roleNames);
	}
}

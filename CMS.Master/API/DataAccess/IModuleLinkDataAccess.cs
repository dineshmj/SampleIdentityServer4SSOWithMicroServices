using System.Collections.Generic;

using FourWallsInc.Entity.LobApp.CMS;

namespace CMS.Master.Api.DataAccess
{
	// A data access class for module and links info.
	public interface IModuleLinkDataAccess
	{
		// Gets the module and link details for the specified login ID.
		IList<CmsModuleAndLinkInfo> GetModuleAndLinkDetailsFor (IList<string> roleNames);
	}
}

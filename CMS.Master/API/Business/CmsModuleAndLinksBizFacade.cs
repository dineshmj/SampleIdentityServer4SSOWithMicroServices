using System.Collections.Generic;

using CMS.Master.Api.DataAccess;
using FourWallsInc.Entity.LobApp.CMS;

namespace CMS.Master.Api.Business
{
	// A business facade for managing module and links on the Master UI screen.
	public sealed class CmsModuleAndLinksBizFacade
		: ICmsModuleAndLinksBizFacade
	{
		private readonly IModuleLinkDataAccess moduleLinkDataAccess;

		public CmsModuleAndLinksBizFacade (IModuleLinkDataAccess moduleLinkDataAccess)
		{
			this.moduleLinkDataAccess = moduleLinkDataAccess;
		}

		// Gets the module and links details which the logged in user has access to.
		public IList<CmsModuleAndLinkInfo> GetModuleAndLinksOf (IList<string> roleNames)
		{
			return (this.moduleLinkDataAccess.GetModuleAndLinkDetailsFor (roleNames));
		}
	}
}

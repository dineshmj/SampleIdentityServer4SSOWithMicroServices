using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using CMS.Master.Api.Business;
using FourWallsInc.Entity.LobApp.CMS;

namespace CMS.Master.Api.Controllers
{
	// This controller sends back a list of links based on the access grants available (roles) with the
	// signed in user.
	[Authorize]
	[Produces ("application/json")]
	[Route ("api/ModuleLinkInfo")]
	public sealed class ModuleLinkInfoController
		: Controller
	{
		private readonly ICmsModuleAndLinksBizFacade cmsModuleAndLinksBizFacade;

		public ModuleLinkInfoController (ICmsModuleAndLinksBizFacade cmsModuleAndLinksBizFacade)
		{
			this.cmsModuleAndLinksBizFacade = cmsModuleAndLinksBizFacade;
		}

		// Gets a list of links available to the signed in user.
		public IEnumerable<CmsModuleAndLinkInfo> Get ()
		{
			// // ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: Extract the "roles" of the signed in user by inspecting the Claims.
			var roleNames
				= base.User
					.Claims
					.Where (c => c.Type == ClaimTypes.Role)
					.Select (c => c.Value)
					.ToList ();

			// Prepare a fresh list of module links.
			var moduleLinks
				= this.cmsModuleAndLinksBizFacade
					.GetModuleAndLinksOf (roleNames);

			// Send back the module links, which were preapred based 
			// on the roles of the signed in user.
			return (moduleLinks);
		}
	}
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using FourWallsInc.Entity.LobApp.CMS;

namespace CMS.Master.Api.Controllers
{
	[Authorize]
	[Produces ("application/json")]
	[Route ("api/BrandInfo")]
	public sealed class BrandInfoController
		: Controller
	{
		#region Methods.

		public object Get ()
		{
			return
				(
					new BrandInfo
					{
						CompanyName = "Four Walls Inc.",
						ApplicationName = "Customer Management System",
						Version = "1.2.0.6"
					}
				);
		}

		#endregion
	}
}

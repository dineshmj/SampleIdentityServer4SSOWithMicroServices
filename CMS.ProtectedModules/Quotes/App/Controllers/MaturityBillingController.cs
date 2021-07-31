using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Quotes.App.Controllers
{
	[Authorize]
	public class MaturityBillingController
		: Controller
	{
		// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: The following "role" based authorization control restricts
		// unauthorized access to this MVC action method.
		[Authorize (Roles = Roles.ROLE_MATURITY_BILLING)]
		public IActionResult Compute ()
		{
			return base.View ();
		}

		// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: The following "role" based authorization control restricts
		// unauthorized access to this MVC action method.
		[Authorize (Roles = Roles.ROLE_MATURITY_BILLING)]
		public IActionResult Search ()
		{
			return base.View ();
		}
	}
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Quotes.App.Controllers
{
	[Authorize]
	public class QuotesController
		: Controller
	{
		// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: The following "role" based authorization control restricts
		// unauthorized access to this MVC action method.
		[Authorize (Roles = Roles.QUOTES_MGMT)]
		public IActionResult IssueQuote ()
		{
			return base.View ();
		}

		// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: The following "role" based authorization control restricts
		// unauthorized access to this MVC action method.
		[Authorize (Roles = Roles.QUOTES_MGMT)]
		public IActionResult ModifyQuote ()
		{
			return base.View ();
		}
	}
}

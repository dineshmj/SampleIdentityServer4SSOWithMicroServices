using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Master.App.Controllers
{
	// A controller that gives a "Not Implemented" message to the end user.
	[Authorize]
	public sealed class NotImplementedController
		: Controller
	{
		// Forms the menu strip on the left pane.
		public IActionResult Index ()
		{
			// Render the view.
			return base.View ();
		}
	}
}

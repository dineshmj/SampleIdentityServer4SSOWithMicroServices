using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Master.App.Controllers
{
	/// <summary>
	/// A controller that gives a "Not Implemented" message to the end user.
	/// </summary>
	[Authorize]
	public sealed class NotImplementedController
		: Controller
	{
		#region Action methods.

		/// <summary>
		/// Forms the menu strip on the left pane.
		/// </summary>
		/// <returns></returns>
		public IActionResult Index ()
		{
			// Render the view.
			return (base.View ());
		}

		#endregion
	}
}

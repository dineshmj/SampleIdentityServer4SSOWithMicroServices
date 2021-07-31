using System.Diagnostics;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using CMS.Master.App.Models;

namespace CMS.Master.App.Controllers
{
	/// <summary>
	/// The Home MVC controller.
	/// </summary>
	[Authorize]             // ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: Causes this MVC controller to respond, only if the user is signed in.
	public class HomeController
		: Controller
	{
		public IActionResult Index ()
		{
			return (base.View ());
		}

		public IActionResult About ()
		{
			base.ViewData ["Message"] = "Your application description page.";

			return (base.View ());
		}

		public async Task Logout ()
		{
			// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: The following two statements would cause both this protected
			// web app, as well as the IDP to log the signed in user out. When that happens, the access to the
			// other module web / API apps also would not be possible, as the cookie is going to be invalidated.
			// Once this happens, the LoB applications would not be available, until the user signs in again using
			// the IDP's sign in UI.
			await base.HttpContext.SignOutAsync (CookieAuthenticationDefaults.AuthenticationScheme);
			await base.HttpContext.SignOutAsync (OpenIdConnectDefaults.AuthenticationScheme);
		}

		public IActionResult Contact ()
		{
			base.ViewData ["Message"] = "Your contact page.";

			return (base.View ());
		}

		public IActionResult Error ()
		{
			return (base.View (new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }));
		}
	}
}

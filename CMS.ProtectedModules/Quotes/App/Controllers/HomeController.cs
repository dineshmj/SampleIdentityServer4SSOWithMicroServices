using System.Diagnostics;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using CMS.Quotes.App.Models;

namespace CMS.Quotes.App.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		public IActionResult Index ()
		{
			return base.View ();
		}

		public IActionResult About ()
		{
			base.ViewData ["Message"] = "Your application description page.";

			return base.View ();
		}
		public async Task Logout ()
		{
			// NOTE: The following statements would force this web app, as well as the IDP
			// NOTE: to log the user off.
			await base.HttpContext.SignOutAsync (CookieAuthenticationDefaults.AuthenticationScheme);
			await base.HttpContext.SignOutAsync (OpenIdConnectDefaults.AuthenticationScheme);
		}

		public IActionResult Contact ()
		{
			base.ViewData ["Message"] = "Your contact page.";

			return base.View ();
		}

		public IActionResult Error ()
		{
			return base.View (new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}

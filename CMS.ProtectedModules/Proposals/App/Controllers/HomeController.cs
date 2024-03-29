﻿using System.Diagnostics;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using CMS.Proposals.App.Models;

namespace CMS.Proposals.App.Controllers
{
	[Authorize]
	public class HomeController
		: Controller
	{
		public IActionResult Index ()
		{
			return View ();
		}

		public IActionResult About ()
		{
			ViewData ["Message"] = "Your application description page.";

			return View ();
		}

		public async Task Logout ()
		{
			await HttpContext.SignOutAsync (CookieAuthenticationDefaults.AuthenticationScheme);
			await HttpContext.SignOutAsync (OpenIdConnectDefaults.AuthenticationScheme);
		}

		public IActionResult Contact ()
		{
			ViewData ["Message"] = "Your contact page.";

			return View ();
		}

		public IActionResult Error ()
		{
			return View (new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}

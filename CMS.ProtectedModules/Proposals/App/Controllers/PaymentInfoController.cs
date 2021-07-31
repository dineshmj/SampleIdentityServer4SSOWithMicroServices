using System;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using CMS.Proposals.App.Models;

namespace CMS.Proposals.App.Controllers
{
	[Authorize]
	public sealed class PaymentInfoController
		: Controller
	{
		// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: The following "role" based authorization control restricts
		// unauthorized access to this MVC action method.
		[Authorize (Roles = Roles.ROLE_MAKE_A_PAYMENT)]
		[HttpGet]
		public IActionResult Create ()
		{
			var paymentModel =  new PaymentModel ()
				{
					PaymentDueDate = DateTime.Now.AddDays (10),
					DatePaidOn = DateTime.Now,
					AmountDue = 100,
					InterestAmount = 10,
					PaymentId = -1
				};

			return (base.View (paymentModel));
		}

		// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: The following "role" based authorization control restricts
		// unauthorized access to this MVC action method.
		[Authorize (Roles = Roles.ROLE_MAKE_A_PAYMENT)]
		[HttpGet]
		public IActionResult Search ()
		{
			var paymentModel = new PaymentModel ()
			{
				PaymentDueDate = DateTime.Now.AddDays (10),
				DatePaidOn = DateTime.Now,
				AmountDue = 100,
				InterestAmount = 10,
				PaymentId = -1
			};

			return (base.View (paymentModel));
		}
	}
}

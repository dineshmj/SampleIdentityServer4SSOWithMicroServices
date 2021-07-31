using System;
using System.ComponentModel;

namespace CMS.Quotes.App.Models
{
	public sealed class MaturityBillingModel
	{
		[DisplayName ("Billing Date")]
		public DateTime BillingDate { get; set; }

		[DisplayName ("Maturity Amount")]
		public double MaturityAmount { get; set; }

		[DisplayName ("Interest Accrued")]
		public double InterestAccrued { get; set; }
	}
}
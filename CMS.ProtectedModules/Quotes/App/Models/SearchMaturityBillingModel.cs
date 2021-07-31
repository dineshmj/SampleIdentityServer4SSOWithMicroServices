using System;
using System.ComponentModel;

namespace CMS.Quotes.App.Models
{
	public sealed class SearchMaturityBillingModel
	{
		[DisplayName ("Billing Date From")]
		public DateTime BillingDateFrom { get; set; }

		[DisplayName ("Billing Date To")]
		public DateTime BillingDateTo { get; set; }
	}
}
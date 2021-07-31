using System;
using System.ComponentModel;

namespace CMS.Proposals.App.Models
{
	/// <summary>
	/// A model class represent payment.
	/// </summary>
	public sealed class PaymentModel
	{
		#region Properties.

		[DisplayName ("Payment ID")]
		public long PaymentId { get; set; }

		[DisplayName ("Payment Due Date")]
		public DateTime PaymentDueDate { get; set; }

		[DisplayName ("Date of Payment")]
		public DateTime DatePaidOn { get; set; }

		[DisplayName ("Amount Due")]
		public double AmountDue { get; set; }

		[DisplayName ("Interest Amount")]
		public double InterestAmount { get; set; }

		#endregion
	}
}
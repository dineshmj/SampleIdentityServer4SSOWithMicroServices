using System;

namespace FourWallsInc.Entity.EntityAccess
{
	/// <summary>
	/// An attribute to decorate a business entity to declare
	/// as to which DB table the entity corresponds to.
	/// </summary>
	/// <seealso cref="System.Attribute" />
	public sealed class TableAttribute
		: Attribute
	{
		/// <summary>
		/// Gets or sets the name of the table which the business
		/// entity corresponds to.
		/// </summary>
		public string Name { get; set; }
	}
}
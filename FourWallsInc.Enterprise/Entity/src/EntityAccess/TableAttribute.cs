using System;

namespace FourWallsInc.Entity.EntityAccess
{
	// An attribute to decorate a business entity to declare
	// as to which DB table the entity corresponds to.
	public sealed class TableAttribute
		: Attribute
	{
		// Gets or sets the name of the table which the business
		// entity corresponds to.
		public string Name { get; set; }
	}
}

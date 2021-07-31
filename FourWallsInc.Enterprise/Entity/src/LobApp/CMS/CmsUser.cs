using System;

using FourWallsInc.Entity.EntityAccess;

namespace FourWallsInc.Entity.LobApp.CMS
{
	// Represents a CMS application user.
	[Table (Name = "[dbo].[CmsUser]")]
	public sealed class CmsUser
		: DTOBase
	{
		[Column (IsNonIdentityPrimaryKey = true)]
		public long Id { get; set; }

		[Column]
		public string FirstName { get; set; }

		[Column]
		public string LastName { get; set; }

		[Column]
		public DateTime BirthDate { get; set; }

		[Column]
		public char Gender { get; set; }

		[Column]
		public string MobilePhone { get; set; }

		[Column]
		public string WorkPhone { get; set; }

		[Column]
		public string HomePhone { get; set; }

		[Column]
		public string Email { get; set; }
	}
}

using System;

using FourWallsInc.Entity.EntityAccess;

namespace FourWallsInc.Entity.LobApp.CMS
{
	/// <summary>
	/// Represents information pertaining to users and their roles.
	/// </summary>
	/// <seealso cref="FourWallsInc.Entity.DTOBase" />
	[Table (Name = "[dbo].[CmsUserAndRoleInfo]")]
	public sealed class CmsUserAndRoleInfo
		: DTOBase
	{
		[Column (IsCompositePrimaryKey = true, ForeignKeyTo = typeof (CmsUser))]
		public long UserId { get; set; }

		[Column (IsCompositePrimaryKey = true, ForeignKeyTo = typeof (CmsRole))]
		public long RoleId { get; set; }

		[Column]
		public DateTime ValidFrom { get; set; }

		[Column]
		public DateTime ValidTill { get; set; }

		[Column]
		public char IsActive { get; set; }
	}
}
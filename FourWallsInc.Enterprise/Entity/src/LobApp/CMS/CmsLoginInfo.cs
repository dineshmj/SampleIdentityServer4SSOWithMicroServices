using FourWallsInc.Entity.EntityAccess;

namespace FourWallsInc.Entity.LobApp.CMS
{
	/// <summary>
	/// Represents the authentication credentials of the CMS application.
	/// </summary>
	[Table (Name = "[dbo].[CmsLoginInfo]")]
	public sealed class CmsLoginInfo
		: DTOBase
	{
		[Column (IsNonIdentityPrimaryKey = true)]
		public string LoginId { get; set; }

		[Column]
		public string Password { get; set; }

		[Column (ForeignKeyTo = typeof (CmsUser))]
		public long UserId { get; set; }
	}
}
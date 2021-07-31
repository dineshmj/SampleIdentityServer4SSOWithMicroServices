using FourWallsInc.Entity.EntityAccess;

namespace FourWallsInc.Entity.LobApp.CMS
{
	// Represents the CMS user roles.
	[Table (Name = "[dbo].[CmsRole]")]
	public sealed class CmsRole
		: DTOBase
	{
		[Column (IsNonIdentityPrimaryKey = true)]
		public int Id { get; set; }

		[Column]
		public string Name { get; set; }
	}
}

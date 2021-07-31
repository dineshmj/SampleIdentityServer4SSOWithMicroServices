using FourWallsInc.Entity.EntityAccess;

namespace FourWallsInc.Entity.LobApp.CMS
{
	/// <summary>
	/// Represents the CMS user roles.
	/// </summary>
	/// <seealso cref="FourWallsInc.Entity.DTOBase" />
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
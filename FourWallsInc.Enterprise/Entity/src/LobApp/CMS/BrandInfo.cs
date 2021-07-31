using FourWallsInc.Entity.EntityAccess;

namespace FourWallsInc.Entity.LobApp.CMS
{
	// Represents the brand information the LoB application
	// must display.
	[Table (Name = "[dbo].[BrandInfo]")]
	public sealed class BrandInfo
	{
		[Column]
		public string CompanyName { get; set; }

		[Column]
		public string ApplicationName { get; set; }

		[Column]
		public string Version { get; set; }
	}
}

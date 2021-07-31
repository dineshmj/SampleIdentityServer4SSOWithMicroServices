using System;

using FourWallsInc.Entity.EntityAccess;

namespace FourWallsInc.Entity.LobApp.CMS
{
	[Serializable]
	[Table (Name = "[dbo].[CmsModuleAndLinkInfo]")]
	public sealed class CmsModuleAndLinkInfo
	{
		[Column]
		public string ModuleName { get; set; }

		[Column]
		public string SubModuleName { get; set; }

		[Column]
		public string ClickableLinkDisplayLabel { get; set; }

		[Column]
		public string RelativeUri { get; set; }

		[Column]
		public int CmsRoleId { get; set; }
	}
}
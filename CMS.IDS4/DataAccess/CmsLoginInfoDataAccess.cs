using FourWallsInc.DataAccess;
using FourWallsInc.Entity.LobApp.CMS;
using FourWallsInc.Infrastructure.ConfigMgmt;

namespace CMS.IDP.App.DataAccess
{
	/// <summary>
	/// A data access class for CmsLoginInfo.
	/// </summary>
	/// <seealso cref="FourWallsInc.DataAccess.DataAccessBase{FourWallsInc.Entity.LobApp.CMS.CmsLoginInfo}" />
	public sealed class CmsLoginInfoDataAccess
		: DataAccessBase<CmsLoginInfo>
	{
		public CmsLoginInfoDataAccess (IConfigManager configManager)
			: base (configManager)
		{
			// Intentionally left blank.
		}
	}
}

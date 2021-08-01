using FourWallsInc.DataAccess;
using FourWallsInc.Entity.LobApp.CMS;
using FourWallsInc.Infrastructure.ConfigMgmt;

namespace CMS.IDP.App.DataAccess
{
	// A data access class for CmsLoginInfo.
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

using FourWallsInc.Infrastructure.ConfigMgmt;

namespace FourWallsInc.DataAccess.Tests.DataAccessBase.Helpers
{
	// A test data access class on "ExtremelyUnlikelyTableWithIdentityPk" entity.
	internal sealed class IdentityPkDataAccess
		: DataAccessBase<ExtremelyUnlikelyTableWithIdentityPk>
	{
		public IdentityPkDataAccess (IConfigManager configuration)
			: base (configuration)
		{
			// Intentionally left blank.
		}
	}
}

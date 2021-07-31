using FourWallsInc.Infrastructure.ConfigMgmt;

namespace FourWallsInc.DataAccess.Tests.DataAccessBase.Helpers
{
	/// <summary>
	/// A test data access class on "ExtremelyUnlikelyTableWithIdentityPk" entity.
	/// </summary>
	/// <seealso cref="FourWallsInc.DataAccess.DataAccessBase{FourWallsInc.DataAccess.Tests.DataAccessBase.ExtremelyUnlikelyTableWithIdentityPk}" />
	internal sealed class RegularPkDataAccess
		: DataAccessBase<ExtremelyUnlikelyTableWithRegularPk>
	{
		public RegularPkDataAccess (IConfigManager configuration)
			: base (configuration)
		{
			// Intentionally left blank.
		}
	}
}
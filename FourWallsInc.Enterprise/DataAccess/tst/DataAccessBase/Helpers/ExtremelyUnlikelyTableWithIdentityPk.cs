using FourWallsInc.Entity;
using FourWallsInc.Entity.EntityAccess;

namespace FourWallsInc.DataAccess.Tests.DataAccessBase.Helpers
{
	/// <summary>
	/// A test table tagged to be having an identity column based primary key.
	/// </summary>
	/// <seealso cref="FourWallsInc.Entity.DTOBase" />
	[Table]
	public sealed class ExtremelyUnlikelyTableWithIdentityPk
		: DTOBase
	{
		[Column (IsIdentityPrimaryKey = true)]
		public long Id { get; set; }

		[Column]
		public string TestColumn { get; set; }

		public int DisregardThisColumn { get; set; }
	}
}
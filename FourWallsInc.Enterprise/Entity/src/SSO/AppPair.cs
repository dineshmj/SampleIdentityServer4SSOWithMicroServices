using System.Collections.Generic;

namespace FourWallsInc.Entity.SSO
{
	/// <summary>
	/// Represents a pair of web and API applications
	/// that constitute a module of an LoB application.
	/// </summary>
	public sealed class AppPair
		: DTOBase
	{
		public string Name { get; set; }

		public AppInfo App { get; set; }

		public ApiInfo Api { get; set; }

		/// <summary>
		/// Gets or sets the additional API resources of the modules that would be used
		/// by this master web application.
		/// </summary>
		public IEnumerable<string> AdditionalApiResourcesThatWouldBeUsed { get; set; }
	}
}

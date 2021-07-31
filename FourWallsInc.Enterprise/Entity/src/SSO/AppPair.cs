using System.Collections.Generic;

namespace FourWallsInc.Entity.SSO
{
	// Represents a pair of web and API applications
	// that constitute a module of an LoB application.
	public sealed class AppPair
		: DTOBase
	{
		public string Name { get; set; }

		public AppInfo App { get; set; }

		public ApiInfo Api { get; set; }

		// Gets or sets the additional API resources of the modules that would be used
		// by this master web application.
		public IEnumerable<string> AdditionalApiResourcesThatWouldBeUsed { get; set; }
	}
}

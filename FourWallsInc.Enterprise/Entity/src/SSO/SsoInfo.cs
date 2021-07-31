namespace FourWallsInc.Entity.SSO
{
	// Represents an LoB application that has several module applications, web and API apps of which
	// are protected by an Identity Server 4.
	public sealed class SsoInfo
		: DTOBase
	{
		public string IdpServerUri { get; set; }

		public AppPair MasterLayoutApp { get; set; }

		public AppPair [] ModuleApps { get; set; }
	}
}

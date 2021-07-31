namespace FourWallsInc.Entity.SSO
{
	// Represents an API application that is part of a pair of web and API applications
	// that constitute a module of an LoB application.
	public sealed class ApiInfo
		: DTOBase
	{
		public string ClientId { get; set; }

		public string ResourceName { get; set; }

		public string ResourceFriendlyName { get; set; }
	}
}

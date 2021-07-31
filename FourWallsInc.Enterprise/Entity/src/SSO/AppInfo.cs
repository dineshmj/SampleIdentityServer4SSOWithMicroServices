namespace FourWallsInc.Entity.SSO
{
	/// <summary>
	/// Represents a web application that is part of a pair of web and API applications
	/// that constitute a module of an LoB application.
	/// </summary>
	public sealed class AppInfo
		: DTOBase
	{
		public string ClientId { get; set; }

		public string ClientSecret { get; set; }

		public string RedirectUri { get; set; }

		public string SignOutRedirectUri { get; set; }
	}
}
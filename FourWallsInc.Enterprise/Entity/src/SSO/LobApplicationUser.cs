using System.Collections.Generic;
using System.Security.Claims;

namespace FourWallsInc.Entity.SSO
{
	// Represents an LoB application user identified by an Identity Provider
	// such as the Identity Server 4.
	public sealed class LobApplicationUser
	{
		public string SubjectId { get; set; }

		public string Username { get; set; }
		
		public string Password { get; set; }

		public string ProviderName { get; set; }
		
		public string ProviderSubjectId { get; set; }

		public bool IsActive { get; set; }

		public ICollection<Claim> Claims { get; set; }
	}
}

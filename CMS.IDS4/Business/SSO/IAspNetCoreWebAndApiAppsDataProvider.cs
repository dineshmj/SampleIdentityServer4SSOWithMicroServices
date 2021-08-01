using System.Collections.Generic;

using IdentityServer4.Models;

namespace CMS.IDP.App.Business.SSO
{
	// Abstracts the fetching of API resources and clients of the protected applications.
	public interface IAspNetCoreWebAndApiAppsDataProvider
	{
		// Gets the API resources of protected services.
		IEnumerable<ApiResource> GetApiResourcesOfProtectedServices ();

		// Gets the clients of protected apps.
		IEnumerable<Client> GetClientsOfProtectedApps ();

		// Gets the identity resource.
		IEnumerable<IdentityResource> GetIdentityResources ();
	}
}

using System.Collections.Generic;

using IdentityServer4.Models;

namespace CMS.IDP.App.Business.SSO
{
	/// <summary>
	/// Abstracts the fetching of API resources and clients of the protected applications.
	/// </summary>
	public interface IAspNetCoreWebAndApiAppsDataProvider
	{
		/// <summary>
		/// Gets the API resources of protected services.
		/// </summary>
		/// <returns></returns>
		IEnumerable<ApiResource> GetApiResourcesOfProtectedServices ();

		/// <summary>
		/// Gets the clients of protected apps.
		/// </summary>
		/// <returns></returns>
		IEnumerable<Client> GetClientsOfProtectedApps ();

		/// <summary>
		/// Gets the identity resource.
		/// </summary>
		/// <returns></returns>
		IEnumerable<IdentityResource> GetIdentityResources ();
	}
}

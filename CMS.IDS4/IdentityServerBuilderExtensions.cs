using Microsoft.Extensions.DependencyInjection;

using CMS.IDP.App.Business;

namespace CMS.IDP.App
{
	// Helps connect this IDP with the custom DB-based user store of the list of
	// applications protected by this IDP.
	public static class IdentityServerBuilderExtensions
	{
		// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: This is your own class that provides extension to the
		// IIdentityServiceBuilder abstraction (used by the StartUp class).
		// 
		// Connects this IDP with the custom user store from where profiles of the authenticated
		// users would be fetched.
		public static IIdentityServerBuilder AddCmsUserStore (this IIdentityServerBuilder builder)
		{
			// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: This is an IProfileService concrete class that you wrote.
			// The IDP would use your concrete class when it requires to get user profile related services.
			builder.AddProfileService<CmsUserProfileService> ();

			return builder;
		}
	}
}

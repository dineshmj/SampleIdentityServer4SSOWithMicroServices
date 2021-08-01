using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using CMS.Master.App.Models;
using FourWallsInc.Entity.LobApp.CMS;
using FourWallsInc.Infrastructure.ConfigMgmt;

namespace CMS.Master.App.Controllers
{
	// This controller's name is apparently wrong - it should have been "MenuStripController" instead.
	[Authorize]
	public sealed class MenuStripController
		: Controller
	{
		private readonly IConfigManager configManager;

		public MenuStripController (IConfigManager configManager)
		{
			this.configManager = configManager;
		}

		// Forms the menu strip on the left pane.
		public async Task<IActionResult> Index ()
		{
			const string CFG_KEY_BRAND_INFO_API_SERVICE_URI = "BrandInfoApiServiceUri";
			const string CFG_KEY_MODULE_LINKS_API_SERVICE_URI = "ModuleLinkApiServiceUri";

			// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: In order for the menu strip to be displayed, it is necessary
			// that this web app (i.e., the Master Web App) interacts with its API app counterpart.
			//
			// The API app here (the master API App) is also a protected application. Hence, unless the outgoing
			// HTTP request from this Web App has the "access token", the corresponding API app would not be
			// able to honor your web app's HTTP request.
			//
			// The following code causes this web app to consume its corresponding API app.

			// Get the access token.
			var token = await HttpContext.GetTokenAsync ("access_token");

			// Form an HTTP client.
			using (var client = new HttpClient ())
			{
				// Set the bearer token using the access token above.
				client.DefaultRequestHeaders.Authorization
					= new System.Net.Http.Headers.AuthenticationHeaderValue ("Bearer", token);

				// Call the API RESTful service.
				var moduleAndLinksApiServiceUri
					= this.configManager
						.GetValueForKey (CFG_KEY_MODULE_LINKS_API_SERVICE_URI);     // Config key for module and links API.

				// Call the API RESTful service.
				var brandInfoApiServiceUri
					= this.configManager
						.GetValueForKey (CFG_KEY_BRAND_INFO_API_SERVICE_URI);       // Config key for brand info API.

				// // ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: This is how you make a C# HTTP request call from the web app to the API app.
				// Get brand info. Call the API RESTful service.
				var responseBytes = await (await client.GetAsync (brandInfoApiServiceUri)).Content.ReadAsStringAsync ();
				var brandInfo = JsonConvert.DeserializeObject<BrandInfo> (responseBytes);

				// Get the module and links.
				// Call the API RESTful service.
				responseBytes = await (await client.GetAsync (moduleAndLinksApiServiceUri)).Content.ReadAsStringAsync ();

				// The above RESTful service call fetches the list of valid Module links.
				// TODO: Inject the "ISerializer" to do this job.
				var moduleLinkInfoList
					= JsonConvert.DeserializeObject<IEnumerable<CmsModuleAndLinkInfo>> (responseBytes);

				// Get a unique list of groups (representing each module).
				var moduleGroups
					= moduleLinkInfoList
						.Select (l => l.ModuleName)
						.ToList ()
						.Distinct ();

				// Prepare the module links, for each module Group.
				var moduleLinks
					= moduleGroups
						.Select
							(
								mg =>
								{
									var subModuleGroups
										= moduleLinkInfoList
											.Where (ml => ml.ModuleName == mg)
											.Select (l => l.SubModuleName)
											.ToList ()
											.Distinct ();

									var subModules
										= subModuleGroups
											.Select
												(
													smg =>
													{
														var thisSubModuleLinks
															= moduleLinkInfoList
																.Where
																	(
																		ml =>
																			ml.ModuleName == mg
																			&& ml.SubModuleName == smg
																	);

														var urisOfThisModule
															= thisSubModuleLinks
																.Select
																	(
																		ml =>
																		new LinkUriModel
																		{
																			LinkFriendlyName = ml.ClickableLinkDisplayLabel,
																			LinkUri = ml.RelativeUri
																		}
																	)
																	.ToList ();

														return new SubModuleModel { SubModuleName = smg, Links = urisOfThisModule};
													}
												)
											.ToList ();

									return new ModuleModel { ModuleName = mg, SubModules = subModules };
								}
							)
						.ToList ();

				// Now you have a list of groups, and their corresponding links
				// to module pages.
				var asEnumerable = (IEnumerable<ModuleModel>) moduleLinks;

				var masterLayoutModel
					= new MasterLayoutModel
						{
							CompanyName = brandInfo.CompanyName,
							ApplicationName = brandInfo.ApplicationName,
							Version = brandInfo.Version,
							ModuleModels = asEnumerable
						};

				// Render the menu strip view.
				return base.View (masterLayoutModel);
			}
		}
	}
}

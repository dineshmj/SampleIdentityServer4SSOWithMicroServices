using System;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CMS.IDP.App
{
	/// <summary>
	/// A class that acts as the starting point
	/// for the Identity Server 4.
	/// </summary>
	public class Program
	{
		/// <summary>
		/// The main program.
		/// </summary>
		/// <param name="args">The arguments.</param>
		public static void Main (string [] args)
		{
			Console.Title = "Four Walls Inc. - Identity Provider (IdP) Host";
			BuildWebHost (args).Run ();
		}

		/// <summary>
		/// Builds the web host.
		/// </summary>
		/// <param name="args">The arguments.</param>
		/// <returns></returns>
		public static IWebHost BuildWebHost (string [] args) =>
			WebHost.CreateDefaultBuilder (args)
				// ↓↓ CATCH: Tells the web host builder to use the "StartUp" class.
				.UseStartup<Startup> ()
				.ConfigureAppConfiguration
					(
						(hostingContext, config) =>
						{
							var env = hostingContext.HostingEnvironment;
							config
								// ↓↓ CATCH: Tells to include the appSettings.json configuration source.
								.AddJsonFile ("appsettings.json", optional: true, reloadOnChange: true)
								.AddJsonFile ($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

							config.AddEnvironmentVariables ();
						}
					)
				 .ConfigureLogging
					(
						(hostingContext, logging) =>
						{
							logging.AddConfiguration (hostingContext.Configuration.GetSection ("Logging"));
							logging.AddConsole ();
							logging.AddDebug ();
						}
					)
				.Build ();
	}
}

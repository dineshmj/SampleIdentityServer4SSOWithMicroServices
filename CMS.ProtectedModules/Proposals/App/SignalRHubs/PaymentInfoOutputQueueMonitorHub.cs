using System.IO;

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;

using FourWallsInc.Infrastructure.ConfigMgmt;
using FourWallsInc.Infrastructure.Serialization;

namespace CMS.Proposals.App.SignalRHubs
{
	public class PaymentInfoOutputQueueMonitorHub : Hub
	{
		private readonly ISerializer serializer;

		public PaymentInfoOutputQueueMonitorHub ()
		{
			var builder
				= new ConfigurationBuilder ()
					.SetBasePath (Directory.GetCurrentDirectory ())
					.AddJsonFile ("appsettings.json", optional: true, reloadOnChange: true);

			var configuration = builder.Build ();
			// Prepare the message manager.
			var configManager = new DefaultConfigManager (configuration);

			// Prepare the message manager.
			this.serializer = new EntitySerializer ();
		}

		public string GetPaymentInfoQueueMonitorHubClientId ()
		{
			return (base.Context.ConnectionId);
		}
	}
}
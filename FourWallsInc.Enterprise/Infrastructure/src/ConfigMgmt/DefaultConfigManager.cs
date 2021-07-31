using Microsoft.Extensions.Configuration;

namespace FourWallsInc.Infrastructure.ConfigMgmt
{
	// Implements configuration management.
	public sealed class DefaultConfigManager
		: IConfigManager
	{
		private readonly IConfiguration configuration;

		public DefaultConfigManager (IConfiguration configuration)
		{
			this.configuration = configuration;
		}

		// Gets the value for the specified configuration key.
		public string GetValueForKey (string configKey)
		{
			return this.configuration [configKey];
		}

		// Gets the connection string corresponding to the name specified.
		public string GetConnectionString (string connectionStringName)
		{
			return this.configuration.GetConnectionString (connectionStringName);
		}
	}
}

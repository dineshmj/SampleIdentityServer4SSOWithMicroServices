using Microsoft.Extensions.Configuration;

namespace FourWallsInc.Infrastructure.ConfigMgmt
{
	/// <summary>
	/// Implements configuration management.
	/// </summary>
	/// <seealso cref="FourWallsInc.Infrastructure.ConfigMgmt.IConfigManager" />
	public sealed class DefaultConfigManager
		: IConfigManager
	{
		private readonly IConfiguration configuration;

		public DefaultConfigManager (IConfiguration configuration)
		{
			this.configuration = configuration;
		}

		/// <summary>
		/// Gets the value for the specified configuration key.
		/// </summary>
		/// <param name="configKey">The configuration key.</param>
		/// <returns></returns>
		public string GetValueForKey (string configKey)
		{
			return (this.configuration [configKey]);
		}

		/// <summary>
		/// Gets the connection string corresponding to the name specified.
		/// </summary>
		/// <param name="connectionStringName">Name of the connection string.</param>
		/// <returns></returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public string GetConnectionString (string connectionStringName)
		{
			return (this.configuration.GetConnectionString (connectionStringName));
		}
	}
}
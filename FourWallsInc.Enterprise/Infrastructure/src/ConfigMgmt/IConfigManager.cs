namespace FourWallsInc.Infrastructure.ConfigMgmt
{
	/// <summary>
	/// Abstracts configuration management.
	/// </summary>
	public interface IConfigManager
	{
		/// <summary>
		/// Gets the value for the specified configuration key.
		/// </summary>
		/// <param name="configKey">The configuration key.</param>
		/// <returns></returns>
		string GetValueForKey (string configKey);

		/// <summary>
		/// Gets the connection string corresponding to the name specified.
		/// </summary>
		/// <param name="connectionStringName">Name of the connection string.</param>
		/// <returns></returns>
		string GetConnectionString (string connectionStringName);
	}
}
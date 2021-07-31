namespace FourWallsInc.Infrastructure.ConfigMgmt
{
	// Abstracts configuration management.
	public interface IConfigManager
	{
		// Gets the value for the specified configuration key.
		string GetValueForKey (string configKey);

		// Gets the connection string corresponding to the name specified.
		string GetConnectionString (string connectionStringName);
	}
}

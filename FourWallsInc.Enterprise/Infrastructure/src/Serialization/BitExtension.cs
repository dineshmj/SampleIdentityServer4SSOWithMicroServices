using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FourWallsInc.Infrastructure.Serialization
{
	public static class ByteExtension
	{
		// Gets an instance of the specified type from byte array specified.
		public static TEntity GetTypeFromBytes<TEntity> (this byte [] byteArray)
		{
			if (byteArray == null)
			{
				return default (TEntity);
			}

			var binFormatter = new BinaryFormatter ();

			using (var memoryStream = new MemoryStream (byteArray))
			{
				object instance = binFormatter.Deserialize (memoryStream);
				return (TEntity) instance;
			}
		}
	}
}

using System.Collections.Generic;

namespace FourWallsInc.Infrastructure.Serialization
{
	public interface ISerializer
	{
		IEnumerable<SerializationFormat> SupportedFormats { get; }

		string Serialize<TEntity>
			(
				SerializationFormat serializationFormat,
				TEntity entity
			);

		TEntity DeSerialize <TEntity>
			(
				SerializationFormat serializationFormat,
				string serializedContent
			);
	}
}
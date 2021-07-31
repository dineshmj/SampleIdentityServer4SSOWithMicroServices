using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Newtonsoft.Json;

namespace FourWallsInc.Infrastructure.Serialization
{
    public sealed class EntitySerializer
		: ISerializer
	{
		public IEnumerable<SerializationFormat> SupportedFormats =>
			(
				new ReadOnlyCollection<SerializationFormat>
					(
						new [] { SerializationFormat.Json }
					)
			);

		public string Serialize<TEntity>
			(
				SerializationFormat serializationFormat,
				TEntity entity
			)
		{
			switch (serializationFormat)
			{
				case SerializationFormat.Json:
					return (JsonConvert.SerializeObject (entity));

				default:
					throw (new NotImplementedException ("The specified format '" + serializationFormat + "' is not supported for serialization."));
			}
		}

		public TEntity DeSerialize<TEntity>
			(
				SerializationFormat serializationFormat,
				string serializedContent
			)
		{
			switch (serializationFormat)
			{
				case SerializationFormat.Json:
					return (JsonConvert.DeserializeObject<TEntity> (serializedContent));

				default:
					throw (new NotImplementedException ("The specified format '" + serializationFormat + "' is not supported for de-serialization."));
			}
		}
	}
}
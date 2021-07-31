using System;
using System.Linq;
using System.Text;

using FourWallsInc.Entity;
using FourWallsInc.Utilities;

namespace FourWallsInc.DataAccess.Extensions
{
	/// <summary>
	/// Contains extension methods for data access for DTOs.
	/// </summary>
	public static partial class DtoExtensions
	{
		/// <summary>
		/// Gets a T-SQL parameterized UPDATE query for the specified entity.
		/// </summary>
		/// <typeparam name="TEntity">The type of the entity.</typeparam>
		/// <param name="sourceEntity">The source entity.</param>
		/// <returns></returns>
		public static string GetUpdateQuery<TEntity>
			(
				this TEntity sourceEntity,
				bool considerDefaultValues = false
			)
			where TEntity : DTOBase
		{
			// Get the table attribute.
			var tableAttribute = sourceEntity.GetTable<TEntity> ();

			// Does it have one?
			if (tableAttribute == null)
			{
				// No.
				return (String.Empty);
			}

			// Get the table name.
			var tableNameToUse
				= String.IsNullOrEmpty (tableAttribute.Name)
					? (typeof (TEntity)).Name
					: tableAttribute.Name;

			// Split the owner and table name from the specified name (such as owner = dbo, table name = Customer, etc.)
			string tableOwner, tableName;
			var tableNameParts = tableNameToUse.Replace ("[", String.Empty).Replace ("]", String.Empty).Split ('.');

			// Is owner specified?
			if (tableNameParts.Length == 1)
			{
				// No.
				tableOwner = "dbo";
				tableName = tableNameParts [0];
			}
			else if (tableNameParts.Length == 2)
			{
				tableOwner = tableNameParts [0];
				tableName = tableNameParts [1];
			}
			else
			{
				// The table name is not in [owner].[TableName] format.
				throw (new InvalidOperationException ("Table name in the TableName attribute must be in [owner].[TableName] format."));
			}

			// Get only those properties that are decorated with [Column] attribute.
			var entityColumnProperties
				= (typeof (TEntity))
					.GetProperties ()
					.Where (p => p.GetTableColumn () != null);

			var updateBuilder = new StringBuilder ();
			var whereBuilder = new StringBuilder ();

			// Begin the SELECT clause.
			updateBuilder.AppendLine
				(
$@"DECLARE @numberOfRowsAffected int;

UPDATE
	{ tableNameToUse }
SET"
				);

			// Begin VALUES clause.
			whereBuilder.AppendLine ("WHERE");

			bool atLeastOneColumnAddedInUpdateClause = false;
			bool atLeastOneColumnAddedInWhereClause = false;
			var commentTextAfterComma = String.Empty;

			// Walk through the column properties.
			foreach (var oneProperty in entityColumnProperties)
			{
				var columnAttribute = oneProperty.GetTableColumn ();

				// Now, prepare for building the VALUES clause.
				var propertyValue = oneProperty.GetValue (sourceEntity);
				var propertyType = oneProperty.PropertyType;

				if (propertyValue != null)
				{
					// Determine the DB column name.
					var columnNameToUse
						= String.IsNullOrEmpty (columnAttribute.Name)
							? oneProperty.Name
							: columnAttribute.Name;

					var isAPrimaryKeyColumn
						= columnAttribute.IsIdentityPrimaryKey
							|| columnAttribute.IsNonIdentityPrimaryKey
							|| columnAttribute.IsCompositePrimaryKey;

					// If "default values" of properties are to be ignored (such as, 0 for an int property),
					// then do not add WHERE conditions based on those properties with default values.
					if
						(
							considerDefaultValues           // Consider default values.
							||
							(
								!considerDefaultValues      // Do not consider default values.
								&& propertyType.IsAPrimitiveType ()					// is short, int, float, DateTime, etc.
								&& propertyValue.IsPrimitiveTypeDefaultValue () == false
							)
						)
					{
						// If it is the primary column that is identity,
						// do not add that primary column in the INSERT statement.
						if (isAPrimaryKeyColumn)
						{
							// UPDATE clause.
							updateBuilder.AppendLine ($"\t--Primary key column \"{ columnNameToUse }\" omitted for UPDATE query.");

							// WHERE clause.
							whereBuilder.Append ("\t");

							if (atLeastOneColumnAddedInWhereClause)
							{
								// Add comma after the previously added column.
								whereBuilder.Append ("AND ");
							}

							whereBuilder.Append ($"{ columnNameToUse } = @{ columnNameToUse.ToCamelCase () }");
							atLeastOneColumnAddedInWhereClause = true;
						}
						else
						{
							// Column is not a primary key column.
							// Include it in the UPDATE-SET clause.

							if (atLeastOneColumnAddedInUpdateClause)
							{
								// Add comma after the previously added column.
								updateBuilder.Append (",");

								if (!String.IsNullOrEmpty (commentTextAfterComma))
								{
									updateBuilder.Append (commentTextAfterComma);
									commentTextAfterComma = String.Empty;
								}

								updateBuilder.AppendLine ();
							}

							updateBuilder.Append ($"\t{ columnNameToUse } = @{ columnNameToUse.ToCamelCase () }");
							atLeastOneColumnAddedInUpdateClause = true;
						}
					}
					else
					{
						if (isAPrimaryKeyColumn && !considerDefaultValues)
						{
							throw new InvalidOperationException ($"Primary key column \"{ columnNameToUse  }\"must have a value other than default value of the type for UPDATE query.");
						}
					}
				}
			}

			// Add WHERE clause.
			updateBuilder.Append
				(
$@"
{ whereBuilder.ToString () };

SET @numberOfRowsAffected = @@ROWCOUNT;

SELECT
	@numberOfRowsAffected
FROM
	INFORMATION_SCHEMA.TABLES
WHERE
	TABLE_SCHEMA = '{ tableOwner }'
	AND TABLE_NAME = '{ tableName }';"
				);

			// Get the UPDATE query.
			var updateQuery = updateBuilder.ToString ();

			return updateQuery;
		}
	}
}
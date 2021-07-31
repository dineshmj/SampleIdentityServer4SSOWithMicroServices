using System;
using System.Linq;
using System.Text;

using FourWallsInc.Entity;
using FourWallsInc.Utilities;

namespace FourWallsInc.DataAccess.Extensions
{
	// Contains extension methods for data access for DTOs.
	public static partial class DtoExtensions
	{
		// Gets a T-SQL parameterized INSERT query for the specified entity.
		public static string GetInsertQuery<TEntity>
			(
				this TEntity sourceEntity,
				bool considerDefaultValues = false
			)
			where TEntity : DTOBase
		{
			// Get the table attribute.
			var tableAttribute = sourceEntity.GetTable<TEntity> ();
			if (tableAttribute == null)
				return String.Empty;

			// Get only those properties that are decorated with [Column] attribute.
			var entityColumnProperties
				= (typeof (TEntity))
					.GetProperties ()
					.Where (p => p.GetTableColumn () != null);

			var insertBuilder = new StringBuilder ();
			var valuesBuilder = new StringBuilder ();

			// Begin the SELECT clause.
			insertBuilder.AppendLine ("INSERT INTO");
			insertBuilder.Append ("\t");
			insertBuilder.AppendLine
				(
					String.IsNullOrEmpty (tableAttribute.Name)
						? (typeof (TEntity)).Name
						: tableAttribute.Name
				);
			insertBuilder.AppendLine ("(");

			// Begin VALUES clause.
			valuesBuilder.AppendLine ("VALUES");
			valuesBuilder.AppendLine ("(");

			var atLeastOneColumnAdded = false;
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
					// If "default values" of properties are to be ignored (such as, 0 for an int property),
					// then do not add WHERE conditions based on those properties with default values.
					if
						(
							columnAttribute.IsIdentityPrimaryKey
							|| considerDefaultValues           // Consider default values.
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
						if (columnAttribute.IsIdentityPrimaryKey)
						{
							insertBuilder.AppendLine ("\t--Primary key column omitted as it is an identity column.");
							continue;
						}

						if (atLeastOneColumnAdded)
						{
							// Add comma after the previously added column.
							insertBuilder.Append (",");

							if (!String.IsNullOrEmpty (commentTextAfterComma))
							{
								insertBuilder.Append (commentTextAfterComma);
								commentTextAfterComma = String.Empty;
							}

							insertBuilder.AppendLine ();
							valuesBuilder.AppendLine (",");
						}

						// Determine the DB column name.
						var columnNameToUse
							= String.IsNullOrEmpty (columnAttribute.Name)
									? oneProperty.Name
									: columnAttribute.Name;

						insertBuilder.Append ("\t");
						insertBuilder.Append (columnNameToUse);

						if (columnAttribute.IsNonIdentityPrimaryKey)
						{
							commentTextAfterComma = "\t\t\t-- Regular (non-Identity) primary key.";
						}

						if (columnAttribute.IsCompositePrimaryKey)
						{
							commentTextAfterComma = "\t\t\t-- Composite primary key.";
						}

						valuesBuilder.Append ("\t@");
						valuesBuilder.Append (oneProperty.Name.ToCamelCase ());

						atLeastOneColumnAdded = true;
					}
				}
			}

			// Close the parentheses.
			insertBuilder.AppendLine ();
			insertBuilder.AppendLine (")");
			valuesBuilder.AppendLine ();
			valuesBuilder.AppendLine (")");
			valuesBuilder.AppendLine ();

			valuesBuilder.Append ("SELECT CAST (SCOPE_IDENTITY () AS int)");

			// Add the VALUES clause.
			insertBuilder.Append (valuesBuilder.ToString ());

			// Get the INSERT query.
			var insertQuery = insertBuilder.ToString ();

			return insertQuery;
		}
	}
}

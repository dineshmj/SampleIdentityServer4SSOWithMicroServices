using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using FourWallsInc.Entity;
using FourWallsInc.Entity.EntityAccess;
using FourWallsInc.Utilities;

namespace FourWallsInc.DataAccess.Extensions
{
	/// <summary>
	/// Contains extension methods for data access for DTOs.
	/// </summary>
	public static partial class DtoExtensions
	{
		/// <summary>
		/// Gets a T-SQL parameterized SELECT query for the specified entity with a WHERE
		/// clause having conditions based on entity's property values.
		/// </summary>
		/// <typeparam name="TEntity">The type of the entity.</typeparam>
		/// <param name="sourceEntity">The source entity.</param>
		/// <returns></returns>
		public static string GetSelectQuery<TEntity>
			(
				this TEntity sourceEntity,
				bool considerDefaultValues = false,
				int topNRecords = -1,
				params Expression<Func<TEntity, object>> [] orderByProperties
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

			// Get only those properties that are decorated with [Column] attribute.
			var entityColumnProperties
				= (typeof (TEntity))
					.GetProperties ()
					.Where (p => p.GetTableColumn () != null);

			var selectBuilder = new StringBuilder ();
			var whereBuilder = new StringBuilder ();
			bool whereClauseStarted = false;

			var selectClauseCounter = 0;
			var whereClauseCounter = 0;

			// Begin the SELECT clause.
			selectBuilder.Append ("SELECT");

			if (topNRecords != -1)
			{
				selectBuilder.Append (" TOP ");
				selectBuilder.Append (topNRecords);
			}

			selectBuilder.AppendLine ();

			// Walk through the column properties.
			foreach (var oneProperty in entityColumnProperties)
			{
				var columnAttribute = oneProperty.GetTableColumn ();

				// Determine the DB column name.
				var columnNameToUse
					= String.IsNullOrEmpty (columnAttribute.Name)
							? oneProperty.Name
							: columnAttribute.Name;

				selectBuilder.Append ("\t");
				selectBuilder.Append (columnNameToUse);

				// Add "as" alias only if the column name is different from property name.
				selectBuilder.Append
					(
						(
							String.IsNullOrEmpty (columnAttribute.Name)
							||  columnAttribute.Name == oneProperty.Name
						)
							? String.Empty
							: $" AS { oneProperty.Name }"
					);

				if (selectClauseCounter < entityColumnProperties.Count () - 1)
				{
					selectBuilder.AppendLine (",");
					selectClauseCounter++;
				}

				// Now, prepare for building the WHERE clause.
				var propertyValue = oneProperty.GetValue (sourceEntity);
				var propertyType = oneProperty.PropertyType;

				if (propertyValue != null)
				{
					// If "default values" of properties are to be ignored (such as, 0 for an int property),
					// then do not add WHERE conditions based on those properties with default values.
					if
						(
							considerDefaultValues			// Consider default values.
							||
							(
								!considerDefaultValues      // Do not consider default values.
								&& propertyType.IsAPrimitiveType ()
								&& propertyValue.IsPrimitiveTypeDefaultValue () == false
							)
						)
					{
						// Add WHERE keyword if not added yet.
						if (! whereClauseStarted)
						{
							whereBuilder.AppendLine ("WHERE");
							whereClauseStarted = true;
						}

						whereBuilder.Append ("\t");

						if (whereClauseCounter != 0)
						{
							whereBuilder.Append ("AND ");
						}

						// If the column is varchar, use LIKE operator.
						if (propertyType == typeof (string))
						{
							whereBuilder.Append (columnNameToUse);
							whereBuilder.Append (" LIKE '%' + @");
							whereBuilder.Append (oneProperty.Name.ToCamelCase ());
							whereBuilder.Append (" + '%'");
						}
						else
						{
							// Column is not a varchar; use = operator.
							whereBuilder.Append (columnNameToUse);
							whereBuilder.Append (" = @");
							whereBuilder.Append (oneProperty.Name.ToCamelCase ());
						}

						if (whereClauseCounter < entityColumnProperties.Count () - 1)
						{
							whereBuilder.AppendLine ();
						}

						whereClauseCounter++;
					}
				}
			}

			// Add FROM clause.
			selectBuilder.AppendLine ();
			selectBuilder.AppendLine ("FROM");
			selectBuilder.Append ("\t");
			selectBuilder.AppendLine
				(
					String.IsNullOrEmpty (tableAttribute.Name)
						? (typeof (TEntity)).Name
						: tableAttribute.Name
				);

			// Add the WHERE clause.
			selectBuilder.Append (whereBuilder.ToString ());

			if (orderByProperties != null && orderByProperties.Length > 0)
			{
				var orderByBuilder = new StringBuilder ();
				orderByBuilder.AppendLine ("ORDER BY");
				var atLeastOneColumnWasAdded = false;

				foreach (var oneFunc in orderByProperties)
				{
					var propertyName
						= oneFunc.Body
							.ToString ()
							.Replace ("Convert(e.", String.Empty)
							.Replace (", Object)", String.Empty)
							.Replace ("e.", String.Empty);

					var dbColumnName
						= entityColumnProperties
							.FirstOrDefault (p => p.Name == propertyName)
							.GetCustomAttribute<ColumnAttribute> ()
							.Name;

					if (String.IsNullOrEmpty (dbColumnName))
					{
						dbColumnName = propertyName;
					}

					if (atLeastOneColumnWasAdded)
					{
						orderByBuilder.AppendLine (",");
					}

					orderByBuilder.Append ("\t");
					orderByBuilder.Append (dbColumnName);
					atLeastOneColumnWasAdded = true;
				}

				selectBuilder.AppendLine ();
				selectBuilder.Append (orderByBuilder.ToString ());
			}

			// Get the SELECT query.
			var selectQuery = selectBuilder.ToString ();

			return selectQuery;
		}

		/// <summary>
		/// Gets the table attribute of the specified business entity.
		/// </summary>
		/// <typeparam name="TEntity">The type of the entity.</typeparam>
		/// <returns></returns>
		public static TableAttribute GetTable<TEntity> (this TEntity sourceEntity)
			where TEntity : DTOBase
		{
			return typeof (TEntity).GetCustomAttribute<TableAttribute> ();
		}

		/// <summary>
		/// Gets the table column attribute of the specified property.
		/// </summary>
		/// <param name="property">The property.</param>
		/// <returns></returns>
		public static ColumnAttribute GetTableColumn (this PropertyInfo property)
		{
			return property.GetCustomAttribute<ColumnAttribute> ();
		}

		/// <summary>
		/// Sets the specified value to the identity primary key property of the entity..
		/// </summary>
		/// <typeparam name="TEntity">The type of the entity.</typeparam>
		/// <param name="entity">The entity.</param>
		/// <param name="value">The value.</param>
		public static void SetIdentityPrimaryKeyTo<TEntity> (this TEntity entity, object value)
			where TEntity : DTOBase
		{
			// Get the identity property value.
			var identityPrimaryKeyProperty
				= typeof (TEntity)
					.GetProperties ()
					.FirstOrDefault
						(
							p =>
							{
								var columnAttribute = p.GetCustomAttribute<ColumnAttribute> ();
								return (columnAttribute != null && columnAttribute.IsIdentityPrimaryKey);
							}
						);

			// Can it be written?
			if (identityPrimaryKeyProperty != null && identityPrimaryKeyProperty.CanWrite)
			{
				identityPrimaryKeyProperty.SetValue (entity, value);
			}
		}
	}
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

using Dapper;

using FourWallsInc.DataAccess.Extensions;
using FourWallsInc.Entity;
using FourWallsInc.Infrastructure.ConfigMgmt;

namespace FourWallsInc.DataAccess
{
	/// <summary>
	/// Abstracts the data access activity for business entities.
	/// </summary>
	/// <typeparam name="TEntity">The type of the entity.</typeparam>
	public abstract partial class DataAccessBase<TEntity>
		: IDataAccess<TEntity>
			where TEntity : DTOBase
	{
		private readonly string connectionString;

		protected DataAccessBase (IConfigManager configManager)
		{
			this.connectionString
				= configManager.GetConnectionString (DAConstants.DEFAULT_CONNECTION);
		}

		/// <summary>
		/// Adds a record into the data source.
		/// </summary>
		/// <param name="newInstanceToAdd">The new instance to add.</param>
		/// <returns>
		/// The primary key value.
		/// </returns>
		public object AddNew (TEntity newInstanceToAdd)
		{
			// Prepare SELECT query, with SCOPE_IDENTITY () being read.
			var insertQuery = newInstanceToAdd.GetInsertQuery ();

			using (var dbConnection = new SqlConnection (this.connectionString))
			{
				// Get what Dapper returns.
				var dapperSet
					= (IDictionary<string, object>)
						dbConnection.Query (insertQuery, newInstanceToAdd).Single ();

				// Is the dictionary valid?
				var dictionaryKeys = dapperSet.Keys.ToArray ();
				if (dictionaryKeys == null || dictionaryKeys.Length == 0)
				{
					return (null);
				}

				// Return the number of rows affected.
				return (dapperSet [dictionaryKeys [0]]);
			}
		}

		/// <summary>
		/// Adds a list of records into the data source.
		/// </summary>
		/// <param name="newInstancesToAdd">The new instances to add.</param>
		/// <returns>
		/// The primary key values.
		/// </returns>
		public object AddNew (IList<TEntity> newInstancesToAdd)
		{
			var listOfPrimaryKeys = new List<object> ();

			// Insert each record.
			foreach (var oneEntity in newInstancesToAdd)
			{
				listOfPrimaryKeys.Add (this.AddNew (oneEntity));
			}

			return (listOfPrimaryKeys);
		}

		/// <summary>
		/// Updates the existing record in the data source.
		/// </summary>
		/// <param name="instanceToUpdate">The instance to update.</param>
		/// <returns></returns>
		public int UpdateExisting (TEntity instanceToUpdate)
		{
			// Prepare the UPDATE query.
			var updateQuery = instanceToUpdate.GetUpdateQuery ();

			using (var dbConnection = new SqlConnection (this.connectionString))
			{
				// Get what Dapper returns.
				var rowsAffected = dbConnection.ExecuteScalar<int> (updateQuery, instanceToUpdate);

				// Return the number of rows affected.
				return (rowsAffected);
			}
		}

		/// <summary>
		/// Updates the existing records in the data source.
		/// </summary>
		/// <param name="instancesToUpdate">The instances to update.</param>
		/// <returns></returns>
		public int UpdateExisting (IList<TEntity> instancesToUpdate)
		{
			var rowsUpdatedCounter = 0;

			// Update each record.
			foreach (var oneEntity in instancesToUpdate)
			{
				rowsUpdatedCounter += this.UpdateExisting (oneEntity);
			}

			return (rowsUpdatedCounter);
		}

		/// <summary>
		/// Deletes the existing record from the data source.
		/// </summary>
		/// <param name="instanceToDelete">The instance to delete.</param>
		/// <returns></returns>
		public int DeleteExisting (TEntity instanceToDelete)
		{
			return 1;
		}

		/// <summary>
		/// Deletes the existing records from the data source.
		/// </summary>
		/// <param name="instancesToDelete"></param>
		/// <returns></returns>
		public int DeleteExisting (IList<TEntity> instancesToDelete)
		{
			return 1;
		}

		public TEntity GetFirstMatchingInstance (TEntity searchCriteria)
		{
			return (this.GetMatchingInstances (searchCriteria, 1).FirstOrDefault ());
		}

		/// <summary>
		/// Gets a list of matching instances corresponding
		/// to the search criteria specified.
		/// </summary>
		/// <param name="searchCriteria">The search criteria.</param>
		/// <param name="topNRecords">The top 'n' records number.</param>
		/// <returns></returns>
		public IList<TEntity> GetMatchingInstances
			(
				TEntity searchCriteria,
				int topNRecords = -1,
				params Expression<Func<TEntity, object>> [] orderByProperties
			)
		{
			IList<TEntity> entities;

			// Prepare SELECT query.
			var selectQuery
				= searchCriteria
					.GetSelectQuery
						(
							topNRecords: topNRecords,
							orderByProperties: orderByProperties
						);

			using (var dbConnection = new SqlConnection (this.connectionString))
			{
				// Fetch records.
				entities = dbConnection.Query<TEntity> (selectQuery, searchCriteria).AsList<TEntity> ();
			}

			// Return a readonly list.
			return (new ReadOnlyCollection<TEntity> (entities));
		}
	}
}
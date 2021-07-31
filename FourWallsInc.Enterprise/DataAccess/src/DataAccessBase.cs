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
	// Abstracts the data access activity for business entities.
	public abstract partial class DataAccessBase<TEntity>
		: IDataAccess<TEntity>
			where TEntity : DTOBase
	{
		private readonly string connectionString;

		protected DataAccessBase (IConfigManager configManager)
		{
			this.connectionString = configManager.GetConnectionString (DAConstants.DEFAULT_CONNECTION);
		}

		// Adds a record into the data source.
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
					return null;

				// Return the number of rows affected.
				return dapperSet [dictionaryKeys [0]];
			}
		}

		// Adds a list of records into the data source.
		public object AddNew (IList<TEntity> newInstancesToAdd)
		{
			var listOfPrimaryKeys = new List<object> ();

			// Insert each record.
			foreach (var oneEntity in newInstancesToAdd)
			{
				listOfPrimaryKeys.Add (this.AddNew (oneEntity));
			}

			return listOfPrimaryKeys;
		}

		// Updates the existing record in the data source.
		public int UpdateExisting (TEntity instanceToUpdate)
		{
			// Prepare the UPDATE query.
			var updateQuery = instanceToUpdate.GetUpdateQuery ();

			using (var dbConnection = new SqlConnection (this.connectionString))
			{
				// Get what Dapper returns.
				var rowsAffected = dbConnection.ExecuteScalar<int> (updateQuery, instanceToUpdate);

				// Return the number of rows affected.
				return rowsAffected;
			}
		}

		// Updates the existing records in the data source.
		public int UpdateExisting (IList<TEntity> instancesToUpdate)
		{
			var rowsUpdatedCounter = 0;

			// Update each record.
			foreach (var oneEntity in instancesToUpdate)
			{
				rowsUpdatedCounter += this.UpdateExisting (oneEntity);
			}

			return rowsUpdatedCounter;
		}

		// Deletes the existing record from the data source.
		public int DeleteExisting (TEntity instanceToDelete)
		{
			// return 1;
			throw new NotImplementedException ();
		}

		// Deletes the existing records from the data source.
		public int DeleteExisting (IList<TEntity> instancesToDelete)
		{
			// return 1;
			throw new NotImplementedException ();
		}

		public TEntity GetFirstMatchingInstance (TEntity searchCriteria)
		{
			return this.GetMatchingInstances (searchCriteria, 1).FirstOrDefault ();
		}

		// Gets a list of matching instances corresponding
		// to the search criteria specified.
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
			return new ReadOnlyCollection<TEntity> (entities);
		}
	}
}

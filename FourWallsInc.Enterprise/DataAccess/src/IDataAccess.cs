using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using FourWallsInc.Entity;

namespace FourWallsInc.DataAccess
{
	// Abstracts the data access activity for business entities.
	public interface IDataAccess<TEntity>
		where TEntity : DTOBase
	{
		// Adds a record into the data source.
		object AddNew (TEntity newInstanceToAdd);

		// Adds a list of records into the data source.
		object AddNew (IList<TEntity> newInstancesToAdd);

		// Updates the existing record in the data source.
		int UpdateExisting (TEntity instanceToUpdate);

		// Updates the existing records in the data source.
		int UpdateExisting (IList<TEntity> instancesToUpdate);

		// Deletes the existing record from the data source.
		int DeleteExisting (TEntity instanceToDelete);

		// Deletes the existing records from the data source.
		int DeleteExisting (IList<TEntity> instancesToDelete);

		// Gets the first matching instancecorresponding
		// to the search criteria specified.
		TEntity GetFirstMatchingInstance (TEntity searchCriteria);

		// Gets a list of matching instances corresponding
		// to the search criteria specified.
		IList<TEntity> GetMatchingInstances
			(
				TEntity searchCriteria,
				int topNRecords = -1,
				params Expression<Func<TEntity, object>> [] orderByProperties
			);
	}
}

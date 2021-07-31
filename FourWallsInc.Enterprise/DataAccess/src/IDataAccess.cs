using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FourWallsInc.Entity;

namespace FourWallsInc.DataAccess
{
	/// <summary>
	/// Abstracts the data access activity for business entities.
	/// </summary>
	/// <typeparam name="TEntity">The type of the entity.</typeparam>
	public interface IDataAccess<TEntity>
		where TEntity : DTOBase
	{
		/// <summary>
		/// Adds a record into the data source.
		/// </summary>
		/// <param name="newInstanceToAdd">The new instance to add.</param>
		/// <returns>The primary key value.</returns>
		object AddNew (TEntity newInstanceToAdd);

		/// <summary>
		/// Adds a list of records into the data source.
		/// </summary>
		/// <param name="newInstancesToAdd">The new instances to add.</param>
		/// <returns>The primary key values.</returns>
		object AddNew (IList<TEntity> newInstancesToAdd);

		/// <summary>
		/// Updates the existing record in the data source.
		/// </summary>
		/// <param name="instanceToUpdate">The instance to update.</param>
		/// <returns></returns>
		int UpdateExisting (TEntity instanceToUpdate);

		/// <summary>
		/// Updates the existing records in the data source.
		/// </summary>
		/// <param name="instancesToUpdate">The instances to update.</param>
		/// <returns></returns>
		int UpdateExisting (IList<TEntity> instancesToUpdate);

		/// <summary>
		/// Deletes the existing record from the data source.
		/// </summary>
		/// <param name="instanceToDelete">The instance to delete.</param>
		/// <returns></returns>
		int DeleteExisting (TEntity instanceToDelete);

		/// <summary>
		/// Deletes the existing records from the data source.
		/// </summary>
		/// <param name="instanceToDelete">The instances to delete.</param>
		/// <returns></returns>
		int DeleteExisting (IList<TEntity> instancesToDelete);

		/// <summary>
		/// Gets the first matching instancecorresponding
		/// to the search criteria specified.
		/// </summary>
		/// <param name="searchCriteria">The search criteria.</param>
		/// <returns></returns>
		TEntity GetFirstMatchingInstance (TEntity searchCriteria);

		/// <summary>
		/// Gets a list of matching instances corresponding
		/// to the search criteria specified.
		/// </summary>
		/// <param name="searchCriteria">The search criteria.</param>
		/// <param name="topNRecords">The top 'n' records number.</param>
		/// <param name="orderByProperties">The order by properties.</param>
		/// <returns></returns>
		IList<TEntity> GetMatchingInstances
			(
				TEntity searchCriteria,
				int topNRecords = -1,
				params Expression<Func<TEntity, object>> [] orderByProperties
			);
	}
}
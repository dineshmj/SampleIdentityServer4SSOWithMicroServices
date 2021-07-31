using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using FluentAssertions;
using Moq;

using FourWallsInc.DataAccess.Tests.DataAccessBase.Helpers;
using FourWallsInc.Infrastructure.ConfigMgmt;

namespace Given_that_a_DB_table_with_identity_based_PK_and_corresponding_attribute_decorated_entity_are_available
{
	[TestClass]
	public sealed class When_I_call_the_AddNew_method_to_insert_a_new_record
	{
		/// <summary>
		/// Helps prepare dummy test tables in the DB with test records.
		/// </summary>
		[TestInitialize]
		public void Initialize ()
		{
			// Create a fresh table with no records in it.
			TestTableAndDataCreator.CreateTestTableWithIdentityPrimaryKey ();
		}

		[TestMethod]
		public void It_should_insert_the_new_record_and_return_scope_identity_value_as_primary_key ()
		{
			// Mock the dependencies.
			var configManager = new Mock<IConfigManager> ();
			configManager
				.Setup (c => c.GetConnectionString (It.IsAny<string> ()))
				.Returns (DdlAndDmlConstants.CONNECTION_STRING);

			// Prepare the target object, and parameters for target method.
			var identityPkDataAccess = new IdentityPkDataAccess (configManager.Object);

			var newRecordToInsert
				= new ExtremelyUnlikelyTableWithIdentityPk
				{
					TestColumn = "Some value."
				};

			// Call the target method.
			var primaryKeyOfNewRecord = (int) identityPkDataAccess.AddNew (newRecordToInsert);

			// Assert the expectations. The primary key of the new record must be 1.
			primaryKeyOfNewRecord.Should ().Be (1);
		}

		[TestMethod]
		public void It_should_insert_the_new_records_and_return_an_array_of_scope_identity_values_as_primary_keys ()
		{
			// Mock the dependencies.
			var configManager = new Mock<IConfigManager> ();
			configManager
				.Setup (c => c.GetConnectionString (It.IsAny<string> ()))
				.Returns (DdlAndDmlConstants.CONNECTION_STRING);

			// Prepare the target object, and parameters for target method.
			var identityPkDataAccess = new IdentityPkDataAccess (configManager.Object);

			var newRecordsToInsert
				= new []
				{
					new ExtremelyUnlikelyTableWithIdentityPk
					{
						TestColumn = "Some value."
					},
					new ExtremelyUnlikelyTableWithIdentityPk
					{
						TestColumn = "Some other value."
					}
				};

			// Call the target method.
			var primaryKeysOfNewRecords = (IList<object>) identityPkDataAccess.AddNew (newRecordsToInsert);

			// Assert the expectations. The primary key of the new record must be 1.
			primaryKeysOfNewRecords.Count.Should ().Be (2);
		}

		/// <summary>
		/// Cleans up the debri in the DB that were created for testing purpose.
		/// </summary>
		[TestCleanup]
		public void CleanUp ()
		{
			// Delete records and drop table.
			TestTableAndDataCreator.DeleteRecordsAndDropTestTableWithIdentityPrimaryKey ();
		}
	}
}

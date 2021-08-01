using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using FluentAssertions;
using Moq;

using FourWallsInc.DataAccess.Tests.DataAccessBase.Helpers;
using FourWallsInc.Infrastructure.ConfigMgmt;

namespace Given_that_a_DB_table_with_regular_PK_and_corresponding_attribute_decorated_entity_are_available
{
	[TestClass]
	public sealed class When_I_call_the_AddNew_method_to_insert_a_new_record
	{
		// Helps prepare dummy test tables in the DB with test records.
		[TestInitialize]
		public void Initialize ()
		{
			// Create a fresh table with no records in it.
			TestTableAndDataCreator.CreateTestTableWithRegularPrimaryKey ();
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
			var regularPkDataAccess = new RegularPkDataAccess (configManager.Object);

			var newRecordToInsert
				= new ExtremelyUnlikelyTableWithRegularPk {
						Id = 2000,          // Primary key value is given in the INSERT statement.
						TestColumn = "Some value."
					};

			// Call the target method.
			var primaryKeyOfNewRecord = regularPkDataAccess.AddNew (newRecordToInsert);

			// Assert the expectations. Dapper will not be able to give beck Scope_Identity () because
			// the Primary Key was given as a value in the INSERT statement.
			primaryKeyOfNewRecord.Should ().BeNull ();
		}

		[TestMethod]
		public void It_should_insert_the_new_records_and_return_an_array_of_null_values_as_primary_keys ()
		{
			// Mock the dependencies.
			var configManager = new Mock<IConfigManager> ();
			configManager
				.Setup (c => c.GetConnectionString (It.IsAny<string> ()))
				.Returns (DdlAndDmlConstants.CONNECTION_STRING);

			// Prepare the target object, and parameters for target method.
			var regularPkDataAccess = new RegularPkDataAccess (configManager.Object);

			var newRecordsToInsert
				= new [] {
						new ExtremelyUnlikelyTableWithRegularPk {
								Id = 2000,          // Primary key value is given in the INSERT statement.
								TestColumn = "Some value."
							},
						new ExtremelyUnlikelyTableWithRegularPk {
								Id = 4000,          // Primary key value is given in the INSERT statement.
								TestColumn = "Some other value."
							},
					};

			// Call the target method.
			var primaryKeysOfNewRecords = (IList<object>) regularPkDataAccess.AddNew (newRecordsToInsert);

			// Assert the expectations. Dapper will not be able to give beck Scope_Identity () because
			// the Primary Key was given as a value in the INSERT statement.
			primaryKeysOfNewRecords.Count.Should ().Be (2);
		}

		// Cleans up the debri in the DB that were created for testing purpose.
		[TestCleanup]
		public void CleanUp ()
		{
			// Delete records and drop table.
			TestTableAndDataCreator.DeleteRecordsAndDropTestTableWithRegularPrimaryKey ();
		}
	}
}

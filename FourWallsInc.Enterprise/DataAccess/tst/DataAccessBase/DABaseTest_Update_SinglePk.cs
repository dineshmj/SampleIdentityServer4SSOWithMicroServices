using System.Data.SqlClient;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Dapper;
using FluentAssertions;
using Moq;

using FourWallsInc.DataAccess.Tests.DataAccessBase.Helpers;
using FourWallsInc.Infrastructure.ConfigMgmt;

namespace Given_that_a_DB_table_with_single_column_PK_and_corresponding_attribute_decorated_entity_are_available
{
	[TestClass]
	public sealed class When_I_call_the_UpdateExisting_method_to_update_an_existing_record
	{
		/// <summary>
		/// Helps prepare dummy test tables in the DB with test records.
		/// </summary>
		[TestInitialize]
		public void Initialize ()
		{
			// Create a fresh table with no records in it.
			TestTableAndDataCreator.CreateTestTableWithRegularPrimaryKey ();
			TestTableAndDataCreator.InsertTestDataForRegularPkTable ();
		}

		[TestMethod]
		public void It_should_update_the_existing_record ()
		{
			// Mock the dependencies.
			var configManager = new Mock<IConfigManager> ();
			configManager
				.Setup (c => c.GetConnectionString (It.IsAny<string> ()))
				.Returns (DdlAndDmlConstants.CONNECTION_STRING);

			// Prepare the target object, and parameters for target method.
			var regularPkDataAccess = new RegularPkDataAccess (configManager.Object);

			var existingRecordToUpdate
				= new ExtremelyUnlikelyTableWithRegularPk
				{
					Id = 2,          // Primary key value is given in the INSERT statement.
					TestColumn = "ghi changed to xyz"
				};

			// Call the target method.
			var numberOfRowsAffected = regularPkDataAccess.UpdateExisting (existingRecordToUpdate);

			// Check the DB to see the changes and assert the expectations.
			using (var dbConnection = new SqlConnection (DdlAndDmlConstants.CONNECTION_STRING))
			{
				// SELECT query.
				var selectQuery = "SELECT t.TestColumn from [dbo].[ExtremelyUnlikelyTableWithRegularPk] AS t WHERE t.Id = 2";

				// Get what Dapper returns.
				var testColumnValue = dbConnection.QueryFirst<string> (selectQuery);

				testColumnValue.Should ().Be ("ghi changed to xyz");
				numberOfRowsAffected.Should ().Be (1);
			}
		}

		[TestMethod]
		public void It_should_update_the_existing_records_and_return_total_number_of_rows_affected ()
		{
			// Mock the dependencies.
			var configManager = new Mock<IConfigManager> ();
			configManager
				.Setup (c => c.GetConnectionString (It.IsAny<string> ()))
				.Returns (DdlAndDmlConstants.CONNECTION_STRING);

			// Prepare the target object, and parameters for target method.
			var regularPkDataAccess = new RegularPkDataAccess (configManager.Object);

			var existingRecordsToUpdate
				= new []
				{
					new ExtremelyUnlikelyTableWithRegularPk
					{
						Id = 2,          // Primary key value is given in the INSERT statement.
						TestColumn = "ghi changed to pqr"
					},
					new ExtremelyUnlikelyTableWithRegularPk
					{
						Id = 3,          // Primary key value is given in the INSERT statement.
						TestColumn = "jkl changed to xyz"
					}
				};

			// Call the target method.
			var numberOfRowsAffected = regularPkDataAccess.UpdateExisting (existingRecordsToUpdate);

			// Check the DB to see the changes and assert the expectations.
			using (var dbConnection = new SqlConnection (DdlAndDmlConstants.CONNECTION_STRING))
			{
				string testColumnValueOne, testColumnValueTwo;

				// SELECT query.
				var selectQueryWithoutPkValue = "SELECT t.TestColumn from [dbo].[ExtremelyUnlikelyTableWithRegularPk] AS t WHERE t.Id";

				// Get what Dapper returns.
				testColumnValueOne = dbConnection.QueryFirst<string> (selectQueryWithoutPkValue + " = 2");
				testColumnValueTwo = dbConnection.QueryFirst<string> (selectQueryWithoutPkValue + " = 3");

				testColumnValueOne.Should ().Be ("ghi changed to pqr");
				testColumnValueTwo.Should ().Be ("jkl changed to xyz");

				numberOfRowsAffected.Should ().Be (2);			// Two records updated.
			}
		}

		/// <summary>
		/// Cleans up the debri in the DB that were created for testing purpose.
		/// </summary>
		[TestCleanup]
		public void CleanUp ()
		{
			// Delete records and drop table.
			TestTableAndDataCreator.DeleteRecordsAndDropTestTableWithRegularPrimaryKey ();
		}
	}
}
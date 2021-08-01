using Microsoft.VisualStudio.TestTools.UnitTesting;

using FluentAssertions;
using Moq;

using FourWallsInc.DataAccess.Tests.DataAccessBase.Helpers;
using FourWallsInc.Infrastructure.ConfigMgmt;

namespace Given_that_a_DB_table_with_identity_based_PK_and_corresponding_attribute_decorated_entity_are_available
{
	[TestClass]
	public sealed class When_I_call_the_GetInstances_method_on_DataAccess_class_with_a_search_criteria
	{
		// Helps prepare dummy test tables in the DB with test records.
		[TestInitialize]
		public void Initialize ()
		{
			// Create table and insert records.
			TestTableAndDataCreator.CreateTestTableWithIdentityPrimaryKey ();
			TestTableAndDataCreator.InsertTestDataForIdentityPkTable ();
		}

		[TestMethod]
		public void It_should_return_a_non_null_list_of_entity_instances ()
		{
			// Mock the dependencies.
			var configManager = new Mock<IConfigManager> ();
			configManager
				.Setup (c => c.GetConnectionString (It.IsAny<string> ()))
				.Returns (DdlAndDmlConstants.CONNECTION_STRING);

			// Prepare the target object, and parameters for target method.
			var identityPkDataAccess = new IdentityPkDataAccess (configManager.Object);
			var emptySearchCriteria
				= new ExtremelyUnlikelyTableWithIdentityPk {
						TestColumn = "def"				// There is only one record with this value for this column.
					};

			// Call the target method.
			var listOfUsers
				= identityPkDataAccess
					.GetMatchingInstances (emptySearchCriteria);

			// Assert the expectations.
			listOfUsers.Count.Should ().Be (1);
		}

		// Cleans up the debri in the DB that were created for testing purpose.
		[TestCleanup]
		public void CleanUp ()
		{
			// Delete records and drop table.
			TestTableAndDataCreator.DeleteRecordsAndDropTestTableWithIdentityPrimaryKey ();
		}
	}
}

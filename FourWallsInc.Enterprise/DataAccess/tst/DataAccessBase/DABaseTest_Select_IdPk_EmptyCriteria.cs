using Microsoft.VisualStudio.TestTools.UnitTesting;

using FluentAssertions;
using Moq;

using FourWallsInc.DataAccess.Tests.DataAccessBase.Helpers;
using FourWallsInc.Infrastructure.ConfigMgmt;

namespace Given_that_a_DB_table_with_identity_based_PK_and_corresponding_attribute_decorated_entity_are_available
{
	[TestClass]
	public sealed class When_I_call_the_methods_for_fetching_records_on_DataAccess_class_with_an_empty_search_criteria
	{
		/// <summary>
		/// Helps prepare dummy test tables in the DB with test records.
		/// </summary>
		[TestInitialize]
		public void Initialize ()
		{
			// Create table and insert records.
			TestTableAndDataCreator.CreateTestTableWithIdentityPrimaryKey ();
			TestTableAndDataCreator.InsertTestDataForIdentityPkTable ();
		}

		[TestMethod]
		public void It_should_return_first_matching_instance_of_entity_upon_calling_GetFirstMatching_method ()
		{
			// Mock the dependencies.
			var configManager = new Mock<IConfigManager> ();
			configManager
				.Setup (c => c.GetConnectionString (It.IsAny<string> ()))
				.Returns (DdlAndDmlConstants.CONNECTION_STRING);

			// Prepare the target object, and parameters for target method.
			var identityPkDataAccess = new IdentityPkDataAccess (configManager.Object);
			var emptySearchCriteria = new ExtremelyUnlikelyTableWithIdentityPk ();

			// Call the target method.
			var matchingInstance
				= identityPkDataAccess
					.GetFirstMatchingInstance (emptySearchCriteria);

			// Assert the expectations. The first matching record has "abc" as the 
			matchingInstance.Should ().NotBeNull ();
			matchingInstance.TestColumn.Should ().Be ("abc");
		}

		[TestMethod]
		public void It_should_return_a_list_of_entity_instances_upon_calling_GetMatchingInstances_method ()
		{
			// Mock the dependencies.
			var configManager = new Mock<IConfigManager> ();
			configManager
				.Setup (c => c.GetConnectionString (It.IsAny<string> ()))
				.Returns (DdlAndDmlConstants.CONNECTION_STRING);

			// Prepare the target object, and parameters for target method.
			var identityPkDataAccess = new IdentityPkDataAccess (configManager.Object);
			var emptySearchCriteria = new ExtremelyUnlikelyTableWithIdentityPk ();

			// Call the target method.
			var listOfUsers
				= identityPkDataAccess
					.GetMatchingInstances (emptySearchCriteria, -1, e => e.Id, e => e.TestColumn );

			// Assert the expectations. There were 4 test records inserted.
			listOfUsers.Count.Should ().Be (4);
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
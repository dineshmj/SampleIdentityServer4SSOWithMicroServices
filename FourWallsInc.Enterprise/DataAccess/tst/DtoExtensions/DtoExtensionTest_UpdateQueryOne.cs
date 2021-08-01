using Microsoft.VisualStudio.TestTools.UnitTesting;

using FluentAssertions;

using FourWallsInc.DataAccess.Extensions;
using FourWallsInc.DataAccess.Tests;

namespace Given_that_an_entity_class_without_Table_attribute_is_available
{
	[TestClass]
	public sealed class When_attempted_to_generate_UPDATE_query
	{
		private readonly EntityWithNoAttributeTable incompatibleTestEntity;

		// WHEN part.
		public When_attempted_to_generate_UPDATE_query ()
		{
			// Prepare the test entities.
			this.incompatibleTestEntity
				= new EntityWithNoAttributeTable {
						FirstName = "John",
						LastName = "Smith"
					};
		}

		[TestMethod]
		public void Should_generate_empty_UPDATE_query_text_for_incompatible_entity ()
		{
			var updateQuery = this.incompatibleTestEntity.GetUpdateQuery ();
			updateQuery.Should ().BeEmpty ();
		}
	}
}

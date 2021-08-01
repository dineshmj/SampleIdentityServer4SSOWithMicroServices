using Microsoft.VisualStudio.TestTools.UnitTesting;

using FluentAssertions;

using FourWallsInc.DataAccess.Extensions;
using FourWallsInc.DataAccess.Tests;

namespace Given_that_an_entity_class_without_Table_attribute_is_available
{
	[TestClass]
	public sealed class When_attempted_to_generate_SELECT_query
	{
		private readonly EntityWithNoAttributeTable incompatibleTestEntity;

		// WHEN part.
		public When_attempted_to_generate_SELECT_query ()
		{
			// Prepare the test entities.
			this.incompatibleTestEntity
				= new EntityWithNoAttributeTable {
						FirstName = "John",
						LastName = "Smith"
					};
		}

		[TestMethod]
		public void Should_generate_empty_SELECT_query_text_for_incompatible_entity ()
		{
			var selectQuery = this.incompatibleTestEntity.GetSelectQuery ();
			selectQuery.Should ().BeEmpty ();
		}
	}
}

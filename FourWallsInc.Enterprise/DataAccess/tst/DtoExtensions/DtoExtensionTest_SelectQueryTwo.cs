using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using FluentAssertions;

using FourWallsInc.DataAccess.Extensions;
using FourWallsInc.DataAccess.Tests;

namespace Given_that_an_entity_class_with_Table_attribute_but_no_Name_value_is_available
{
	[TestClass]
	public sealed class When_attempted_to_generate_SELECT_query
	{
		private readonly EntityWithAttributesTableAndColumnOnly entityWithTableAndColumnButNoName;

		// WHEN part.
		public When_attempted_to_generate_SELECT_query ()
		{
			// Prepare the test entities.
			this.entityWithTableAndColumnButNoName
				= new EntityWithAttributesTableAndColumnOnly
					{
						FirstName = "John",
						LastName = "Smith",
						BirthDate = new DateTime (1990, 1, 1),
						InstancesCount = 20
					};
		}

		[TestMethod]
		public void Should_generate_SELECT_query_without_column_aliases ()
		{
			var selectQuery = this.entityWithTableAndColumnButNoName.GetSelectQuery ();
			selectQuery.Should ().Be
				(
@"SELECT
	Id,
	FirstName,
	LastName,
	BirthDate
FROM
	EntityWithAttributesTableAndColumnOnly
WHERE
	FirstName LIKE '%' + @firstName + '%'
	AND LastName LIKE '%' + @lastName + '%'
	AND BirthDate = @birthDate
"
				);
		}
	}
}
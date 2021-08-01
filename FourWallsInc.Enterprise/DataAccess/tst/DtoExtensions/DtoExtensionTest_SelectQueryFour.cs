using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using FluentAssertions;

using FourWallsInc.DataAccess.Extensions;
using FourWallsInc.DataAccess.Tests;

namespace Given_that_an_entity_class_with_Table_and_Column_attributes_and_has_value_for_Name_is_available
{
	[TestClass]
	public sealed class When_one_property_is_null_and_then_attempted_to_generate_SELECT_query
	{
		private readonly EntityWithNameForAttributesBothTableAndColumn entityWithTableAndColumnAndHasNameValues;

		// WHEN part.
		public When_one_property_is_null_and_then_attempted_to_generate_SELECT_query ()
		{
			// Prepare the test entities.
			this.entityWithTableAndColumnAndHasNameValues
				= new EntityWithNameForAttributesBothTableAndColumn {
						FirstName = "John",		// NOTE: The "LastName" is left as NULL.
						BirthDate = new DateTime (1990, 1, 1),
						InstancesCount = 20
					};
		}

		[TestMethod]
		public void Should_generate_SELECT_query_without_WHERE_clause_condition_based_on_property_that_was_set_to_null ()
		{
			var selectQuery = this.entityWithTableAndColumnAndHasNameValues.GetSelectQuery ();

			// NOTE: The WHERE clause does not have a condition based on "LastName" property.
			selectQuery.Should ().Be
				(
@"SELECT
	Id,
	first_name AS FirstName,
	last_name AS LastName,
	birth_date AS BirthDate
FROM
	[dbo].[customer]
WHERE
	first_name LIKE '%' + @firstName + '%'
	AND birth_date = @birthDate
"
				);
		}
	}
}

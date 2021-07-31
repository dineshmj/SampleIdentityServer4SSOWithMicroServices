using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using FluentAssertions;

using FourWallsInc.DataAccess.Extensions;
using FourWallsInc.DataAccess.Tests;

namespace Given_that_an_entity_class_with_Table_and_Column_attributes_and_has_value_for_Name_is_available
{
	[TestClass]
	public sealed class When_attempted_to_generate_SELECT_query
	{
		private readonly EntityWithNameForAttributesBothTableAndColumn entityWithTableAndColumnAndHasNameValues;

		// WHEN part.
		public When_attempted_to_generate_SELECT_query ()
		{
			// Prepare the test entities.
			this.entityWithTableAndColumnAndHasNameValues
				= new EntityWithNameForAttributesBothTableAndColumn
					{
						FirstName = "John",
						LastName = "Smith",
						BirthDate = new DateTime (1990, 1, 1),
						InstancesCount = 20
					};
		}

		[TestMethod]
		public void Should_generate_SELECT_query_with_column_aliases ()
		{
			var selectQuery = this.entityWithTableAndColumnAndHasNameValues.GetSelectQuery ();
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
	AND last_name LIKE '%' + @lastName + '%'
	AND birth_date = @birthDate
"
				);
		}
	}
}
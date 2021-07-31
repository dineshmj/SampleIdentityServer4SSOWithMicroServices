using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using FluentAssertions;

using FourWallsInc.DataAccess.Extensions;
using FourWallsInc.DataAccess.Tests;

namespace Given_that_an_entity_class_with_Table_and_Column_attributes_and_has_value_for_Name_is_available
{
	[TestClass]
	public sealed class When_attempted_to_generate_INSERT_query
	{
		private readonly EntityWithNameForAttributesBothTableAndColumn entityWithTableAndColumnButNoName;

		// WHEN part.
		public When_attempted_to_generate_INSERT_query ()
		{
			// Prepare the test entities.
			this.entityWithTableAndColumnButNoName
				= new EntityWithNameForAttributesBothTableAndColumn
				{
						Id = 102,				// Regular primary key.
						FirstName = "John",
						LastName = "Smith",
						BirthDate = new DateTime (1990, 1, 1),
						InstancesCount = 20
					};
		}

		[TestMethod]
		public void Should_generate_INSERT_query_with_comment_on_regular_primary_key ()
		{
			var insertQuery = this.entityWithTableAndColumnButNoName.GetInsertQuery ();
			insertQuery.Should ().Be
				(
@"INSERT INTO
	[dbo].[customer]
(
	Id,			-- Regular (non-Identity) primary key.
	first_name,
	last_name,
	birth_date
)
VALUES
(
	@id,
	@firstName,
	@lastName,
	@birthDate
)

SELECT CAST (SCOPE_IDENTITY () AS int)"
				);
		}
	}
}
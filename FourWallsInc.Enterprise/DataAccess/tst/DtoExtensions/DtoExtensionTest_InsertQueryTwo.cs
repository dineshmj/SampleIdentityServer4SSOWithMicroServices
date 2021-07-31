using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using FluentAssertions;

using FourWallsInc.DataAccess.Extensions;
using FourWallsInc.DataAccess.Tests;

namespace Given_that_an_entity_class_with_Table_attribute_but_no_Name_value_is_available
{
	[TestClass]
	public sealed class When_attempted_to_generate_INSERT_query
	{
		private readonly EntityWithAttributesTableAndColumnOnly entityWithTableAndColumnButNoName;

		// WHEN part.
		public When_attempted_to_generate_INSERT_query ()
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
		public void Should_generate_INSERT_query_with_identity_primary_key_omitted_comment ()
		{
			var insertQuery = this.entityWithTableAndColumnButNoName.GetInsertQuery ();
			insertQuery.Should ().Be
				(
@"INSERT INTO
	EntityWithAttributesTableAndColumnOnly
(
	--Primary key column omitted as it is an identity column.
	FirstName,
	LastName,
	BirthDate
)
VALUES
(
	@firstName,
	@lastName,
	@birthDate
)

SELECT CAST (SCOPE_IDENTITY () AS int)"
				);
		}
	}
}
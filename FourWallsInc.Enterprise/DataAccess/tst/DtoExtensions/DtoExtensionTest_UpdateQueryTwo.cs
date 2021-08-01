using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using FluentAssertions;

using FourWallsInc.DataAccess.Extensions;
using FourWallsInc.DataAccess.Tests;

namespace Given_that_an_entity_class_with_Table_attribute_but_no_Name_value_is_available
{
	[TestClass]
	public sealed class When_attempted_to_generate_UPDATE_query
	{
		private readonly EntityWithAttributesTableAndColumnOnly firstEntityWithTableAndColumnButNoName;
		private readonly EntityWithAttributesTableAndColumnOnly secondEntityWithTableAndColumnButNoName;

		// WHEN part.
		public When_attempted_to_generate_UPDATE_query ()
		{
			// Prepare the first test entity.
			this.firstEntityWithTableAndColumnButNoName
				= new EntityWithAttributesTableAndColumnOnly {
						// REMEMBER: For this test, the primary key column ("Id") value is not specified.
						FirstName = "John",
						LastName = "Smith",
						BirthDate = new DateTime (1990, 1, 1),
						InstancesCount = 20
					};

			// Prepare the second test entity.
			this.secondEntityWithTableAndColumnButNoName
				= new EntityWithAttributesTableAndColumnOnly {
						Id = 20, // REMEMBER: For this test, we are giving value for the primary key.
						FirstName = "John",
						LastName = "Smith",
						BirthDate = new DateTime (1990, 1, 1),
						InstancesCount = 20
					};
		}

		[TestMethod]
		public void Should_throw_an_invalid_operation_exception_when_primary_key_value_is_not_specified ()
		{
			Exception caughtException = null;

			try
			{
				// Call the target method.
				var updateQuery = this.firstEntityWithTableAndColumnButNoName.GetUpdateQuery ();
			}
			catch (Exception ex)
			{
				caughtException = ex;
			}

			// Assert the expectations.
			caughtException.Should ().BeOfType<InvalidOperationException> ();
			caughtException.Message.Should ().Be ("Primary key column \"Id\"must have a value other than default value of the type for UPDATE query.");
		}

		[TestMethod]
		public void Should_generate_UPDATE_query_with_additional_script_for_number_of_rows_affected_if_primary_key_value_is_specified ()
		{
			// Call the target method.
			var updateQuery = this.secondEntityWithTableAndColumnButNoName.GetUpdateQuery ();

			// Assert the expectations.
			updateQuery.Should ().Be
				(
@"DECLARE @numberOfRowsAffected int;

UPDATE
	EntityWithAttributesTableAndColumnOnly
SET
	--Primary key column ""Id"" omitted for UPDATE query.
	FirstName = @firstName,
	LastName = @lastName,
	BirthDate = @birthDate
WHERE
	Id = @id;

SET @numberOfRowsAffected = @@ROWCOUNT;

SELECT
	@numberOfRowsAffected
FROM
	INFORMATION_SCHEMA.TABLES
WHERE
	TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'EntityWithAttributesTableAndColumnOnly';"
				);
		}
	}
}

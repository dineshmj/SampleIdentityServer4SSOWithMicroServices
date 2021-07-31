using System;
using FourWallsInc.Entity;
using FourWallsInc.Entity.EntityAccess;

namespace FourWallsInc.DataAccess.Tests
{
	/// <summary>
	/// Test entity with no [Table] attribute. This type would be ignored by
	/// the SELECT query generating logic.
	/// </summary>
	/// <seealso cref="FourWallsInc.Entity.DTOBase" />
	public sealed class EntityWithNoAttributeTable
		: DTOBase
	{
		/// <summary>
		/// Having the [Column] attribute below is not going to help because
		/// the entity does not have a [Table] attribute at class's level.
		/// </summary>
		[Column]
		public string FirstName { get; set; }

		[Column]
		public string LastName { get; set; }
	}

	/// <summary>
	/// Test entity with table and column attributes to be used for xUnit BDD tests.
	/// </summary>
	/// <seealso cref="DTOBase" />
	[Table]
	public sealed class EntityWithAttributesTableAndColumnOnly
		: DTOBase
	{
		[Column (IsIdentityPrimaryKey = true)]
		public long Id { get; set; }

		[Column]
		public string FirstName { get; set; }

		[Column]
		public string LastName { get; set; }

		[Column]
		public DateTime BirthDate { get; set; }

		/// <summary>
		/// This column must be ignored by the SELECT query generating
		/// logic because it does not have a [Column] attribute.
		/// </summary>
		public int InstancesCount { get; set; }
	}

	/// <summary>
	/// Test entity with table and column attributes having to be used for xUnit BDD tests.
	/// </summary>
	/// <seealso cref="DTOBase" />
	[Table (Name = "[dbo].[customer]")]
	public sealed class EntityWithNameForAttributesBothTableAndColumn
		: DTOBase
	{
		[Column (IsNonIdentityPrimaryKey = true)]
		public long Id { get; set; }

		[Column (Name = "first_name")]
		public string FirstName { get; set; }

		[Column (Name = "last_name")]
		public string LastName { get; set; }

		[Column (Name = "birth_date")]
		public DateTime BirthDate { get; set; }

		/// <summary>
		/// This column must be ignored by the SELECT query generating
		/// logic because it does not have a [Column] attribute.
		/// </summary>
		public int InstancesCount { get; set; }
	}

	/// <summary>
	/// Test entity with table and column attributes having to be used for xUnit BDD tests.
	/// </summary>
	/// <seealso cref="DTOBase" />
	[Table (Name = "[dbo].[customer]")]
	public sealed class EntityWithCompositePrimaryKeyAndNameForAttributesBothTableAndColumn
		: DTOBase
	{
		[Column (IsCompositePrimaryKey = true)]
		public long IdOne { get; set; }

		[Column (IsCompositePrimaryKey = true)]
		public long IdTwo { get; set; }

		[Column (Name = "first_name")]
		public string FirstName { get; set; }

		[Column (Name = "last_name")]
		public string LastName { get; set; }

		[Column (Name = "birth_date")]
		public DateTime BirthDate { get; set; }

		/// <summary>
		/// This column must be ignored by the SELECT query generating
		/// logic because it does not have a [Column] attribute.
		/// </summary>
		public int InstancesCount { get; set; }
	}
}
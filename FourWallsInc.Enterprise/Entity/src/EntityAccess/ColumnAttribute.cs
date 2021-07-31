using System;
using System.Data;

namespace FourWallsInc.Entity.EntityAccess
{
	// An attribute to decorate a business entity property to declare
	// as to which DB table column the entity property corresponds to.
	public class ColumnAttribute
		: Attribute
	{
		private bool isCompositePrimaryKey;
		private bool isNonIdentityPrimaryKey;
		private bool isIdentityPrimaryKey;

		// Gets or sets the name of the table column which the business
		// entity property corresponds to.
		public string Name { get; set; }

		// Gets or sets the DB type of the DB table column which
		// the business entity property corresponds to.
		public DbType DbType { get; set; }

		// Gets or sets the size of the underlying DB table column.
		public short Size { get; set; }

		// Gets or sets the precision of the underlying DB table column.
		public short Precision { get; set; }

		// Gets or sets a value indicating whether this column is an
		// identity based primary key or not.
		public bool IsIdentityPrimaryKey
		{
			get
			{
				return this.isIdentityPrimaryKey;
			}

			set
			{
				this.isIdentityPrimaryKey = value;

				if (value)
				{
					if (this.isNonIdentityPrimaryKey || this.isCompositePrimaryKey)
					{
						throw new InvalidOperationException ("Please specify only one of the three Primary Key indicating properties.");
					}
				}
			}
		}

		// Gets or sets a value indicating whether this column is a regular
		// primary key or not.
		public bool IsNonIdentityPrimaryKey
		{
			get
			{
				return isNonIdentityPrimaryKey;
			}

			set
			{
				isNonIdentityPrimaryKey = value;

				if (value)
				{
					if (this.isIdentityPrimaryKey || this.isCompositePrimaryKey)
					{
						throw new InvalidOperationException ("Please specify only one of the three Primary Key indicating properties.");
					}
				}
			}
		}

		// Gets or sets a value indicating whether this column is part of a composite primary key or not.
		public bool IsCompositePrimaryKey
		{
			get
			{
				return isCompositePrimaryKey;
			}

			set
			{
				isCompositePrimaryKey = value;

				if (value)
				{
					if (this.isIdentityPrimaryKey || this.IsNonIdentityPrimaryKey)
					{
						throw new InvalidOperationException ("Please specify only one of the three Primary Key indicating properties.");
					}
				}
			}
		}

		// Gets or sets the type to which this column acts as a foreign key.
		public Type ForeignKeyTo { get; set; }
	}
}

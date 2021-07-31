using System;
using System.Data;

namespace FourWallsInc.Entity.EntityAccess
{
	/// <summary>
	/// An attribute to decorate a business entity property to declare
	/// as to which DB table column the entity property corresponds to.
	/// </summary>
	/// <seealso cref="System.Attribute" />
	public class ColumnAttribute
		: Attribute
	{
		private bool isCompositePrimaryKey;
		private bool isNonIdentityPrimaryKey;
		private bool isIdentityPrimaryKey;

		/// <summary>
		/// Gets or sets the name of the table column which the business
		/// entity property corresponds to.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the DB type of the DB table column which
		/// the business entity property corresponds to.
		/// </summary>
		public DbType DbType { get; set; }

		/// <summary>
		/// Gets or sets the size of the underlying DB table column.
		/// </summary>
		public short Size { get; set; }

		/// <summary>
		/// Gets or sets the precision of the underlying DB table column.
		/// </summary>
		public short Precision { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this column is an
		/// identity based primary key or not.
		/// </summary>
		/// <value>
		///   <c>true</c> if this column is a identity based primary key; otherwise, <c>false</c>.
		/// </value>
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
						throw (new InvalidOperationException ("Please specify only one of the three Primary Key indicating properties."));
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this column is a regular
		/// primary key or not.
		/// </summary>
		/// <value>
		///   <c>true</c> if this column is a regular primary key; otherwise, <c>false</c>.
		/// </value>
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
						throw (new InvalidOperationException ("Please specify only one of the three Primary Key indicating properties."));
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this column is part of a composite primary key or not.
		/// </summary>
		/// <value>
		///   <c>true</c> if this column is part of a composite primary key; otherwise, <c>false</c>.
		/// </value>
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
						throw (new InvalidOperationException ("Please specify only one of the three Primary Key indicating properties."));
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the type to which this column acts as a foreign key.
		/// </summary>
		public Type ForeignKeyTo { get; set; }
	}
}
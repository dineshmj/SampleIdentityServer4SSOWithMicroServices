namespace FourWallsInc.DataAccess.Tests.DataAccessBase.Helpers
{
	/// <summary>
	/// Contains DML and DDL SQL queries to create temporary DB tables
	/// and temporary records.
	/// </summary>
	public static class DdlAndDmlConstants
	{
		/// <summary>
		/// The connection string.
		/// </summary>
		public const string CONNECTION_STRING = @"Server = VANAMALA\SQLEXPRESS_DINSH; Database = MicroSvcLobDB; Trusted_Connection = True; MultipleActiveResultSets = true";

		/// <summary>
		/// The T-SQL statements for creating temporary table with
		/// identity column based primary key.
		/// </summary>
		public const string DDL_CREATE_IDENTITY_PK_TABLE_QUERY =
@"IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'ExtremelyUnlikelyTableWithIdentityPk')) BEGIN
    DROP TABLE [dbo].[ExtremelyUnlikelyTableWithIdentityPk]
END

/****** Object:  Table [dbo].[ExtremelyUnlikelyTableWithIdentityPk]    Script Date: 05-Jul-19 3:57:43 PM ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[ExtremelyUnlikelyTableWithIdentityPk](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TestColumn] [varchar](50) NOT NULL,
 CONSTRAINT [PK_ExtremelyUnlikelyTableWithIdentityPk] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]";

		/// <summary>
		/// The T-SQL statements for dropping temporary table with
		/// identity column based primary key.
		/// </summary>
		public const string DDL_DROP_IDENTITY_PK_TABLE_QUERY =
@"DELETE FROM [dbo].[ExtremelyUnlikelyTableWithIdentityPk]

IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'ExtremelyUnlikelyTableWithIdentityPk')) BEGIN
    DROP TABLE [dbo].[ExtremelyUnlikelyTableWithIdentityPk]
END";
		/// <summary>
		/// The T-SQL statements for creating temporary table with
		/// regular primary key (i.e., not based on Identity column).
		/// </summary>
		public const string DDL_CREATE_REGULAR_PK_TABLE_QUERY =
@"
IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'ExtremelyUnlikelyTableWithRegularPk')) BEGIN
    DROP TABLE [dbo].[ExtremelyUnlikelyTableWithRegularPk]
END

/****** Object:  Table [dbo].[ExtremelyUnlikelyTableWithRegularPk]    Script Date: 05-Jul-19 4:01:54 PM ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[ExtremelyUnlikelyTableWithRegularPk](
	[Id] [bigint] NOT NULL,
	[TestColumn] [varchar](50) NOT NULL
) ON [PRIMARY]";

		/// <summary>
		/// The T-SQL statements for dropping temporary table with
		/// regular primary key.
		/// </summary>
		public const string DDL_DROP_REGULAR_PK_TABLE_QUERY =
@"DELETE FROM [dbo].[ExtremelyUnlikelyTableWithRegularPk]

IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'ExtremelyUnlikelyTableWithRegularPk')) BEGIN
    DROP TABLE [dbo].[ExtremelyUnlikelyTableWithRegularPk]
END";
		/// <summary>
		/// The DML T-SQL statements for inserting data into test table with
		/// identity column based primary key.
		/// </summary>
		public const string DML_INSERT_INTO_IDENTITY_PK_TABLE_QUERY =
@"DELETE FROM [dbo].[ExtremelyUnlikelyTableWithIdentityPk]

INSERT INTO [dbo].[ExtremelyUnlikelyTableWithIdentityPk] (TestColumn) VALUES ('abc')
INSERT INTO [dbo].[ExtremelyUnlikelyTableWithIdentityPk] (TestColumn) VALUES ('def')
INSERT INTO [dbo].[ExtremelyUnlikelyTableWithIdentityPk] (TestColumn) VALUES ('ghi')
INSERT INTO [dbo].[ExtremelyUnlikelyTableWithIdentityPk] (TestColumn) VALUES ('jkl')";

		/// <summary>
		/// The DML T-SQL statements for inserting data into test table with
		/// regular primary key.
		/// </summary>
		public const string DML_INSERT_INTO_REGULAR_PK_TABLE_QUERY =
@"DELETE FROM [dbo].[ExtremelyUnlikelyTableWithRegularPk]

INSERT INTO [dbo].[ExtremelyUnlikelyTableWithRegularPk] (Id, TestColumn) VALUES (0, 'abc')
INSERT INTO [dbo].[ExtremelyUnlikelyTableWithRegularPk] (Id, TestColumn) VALUES (1, 'def')
INSERT INTO [dbo].[ExtremelyUnlikelyTableWithRegularPk] (Id, TestColumn) VALUES (2, 'ghi')
INSERT INTO [dbo].[ExtremelyUnlikelyTableWithRegularPk] (Id, TestColumn) VALUES (3, 'jkl')";
	}
}
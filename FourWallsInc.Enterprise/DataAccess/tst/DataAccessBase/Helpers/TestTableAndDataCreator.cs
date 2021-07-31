using System.Data.SqlClient;

namespace FourWallsInc.DataAccess.Tests.DataAccessBase.Helpers
{
	public static class TestTableAndDataCreator
	{
		/// <summary>
		/// Creates the test table with identity primary key.
		/// </summary>
		public static void CreateTestTableWithIdentityPrimaryKey ()
		{
			var connection = new SqlConnection (DdlAndDmlConstants.CONNECTION_STRING);
			var tableCreateQuery = DdlAndDmlConstants.DDL_CREATE_IDENTITY_PK_TABLE_QUERY;
			ExecuteDdlQuery (connection, tableCreateQuery);
		}

		/// <summary>
		/// Drops the test table with identity primary key.
		/// </summary>
		public static void DeleteRecordsAndDropTestTableWithIdentityPrimaryKey ()
		{
			var connection = new SqlConnection (DdlAndDmlConstants.CONNECTION_STRING);
			var tableDropQuery = DdlAndDmlConstants.DDL_DROP_IDENTITY_PK_TABLE_QUERY;
			ExecuteDdlQuery (connection, tableDropQuery);
		}

		/// <summary>
		/// Creates the test table with regular primary key.
		/// </summary>
		public static void CreateTestTableWithRegularPrimaryKey ()
		{
			var connection = new SqlConnection (DdlAndDmlConstants.CONNECTION_STRING);
			var tableCreateQuery = DdlAndDmlConstants.DDL_CREATE_REGULAR_PK_TABLE_QUERY;
			ExecuteDdlQuery (connection, tableCreateQuery);
		}

		/// <summary>
		/// Drops the test table with regular primary key.
		/// </summary>
		public static void DeleteRecordsAndDropTestTableWithRegularPrimaryKey ()
		{
			var connection = new SqlConnection (DdlAndDmlConstants.CONNECTION_STRING);
			var tableDropQuery = DdlAndDmlConstants.DDL_DROP_REGULAR_PK_TABLE_QUERY;
			ExecuteDdlQuery (connection, tableDropQuery);
		}

		/// <summary>
		/// Inserts test data for test table with identity column based primary key.
		/// </summary>
		public static void InsertTestDataForIdentityPkTable ()
		{
			var connection = new SqlConnection (DdlAndDmlConstants.CONNECTION_STRING);
			var insertRecordsQuery = DdlAndDmlConstants.DML_INSERT_INTO_IDENTITY_PK_TABLE_QUERY;
			ExecuteDdlQuery (connection, insertRecordsQuery);
		}

		/// <summary>
		/// Inserts test data for test table with regular primary key.
		/// </summary>
		public static void InsertTestDataForRegularPkTable ()
		{
			var connection = new SqlConnection (DdlAndDmlConstants.CONNECTION_STRING);
			var insertRecordsQuery = DdlAndDmlConstants.DML_INSERT_INTO_REGULAR_PK_TABLE_QUERY;
			ExecuteDdlQuery (connection, insertRecordsQuery);
		}

		#region Private static methods.

		/// <summary>
		/// Executes the DDL query.
		/// </summary>
		/// <param name="connection">The SQL connection.</param>
		/// <param name="ddlQuery">The DDL query.</param>
		private static void ExecuteDdlQuery (SqlConnection connection, string ddlQuery)
		{
			var sqlCommand = new SqlCommand (ddlQuery, connection);
			connection.Open ();
			sqlCommand.ExecuteNonQuery ();
			connection.Close ();
		}

		#endregion
	}
}
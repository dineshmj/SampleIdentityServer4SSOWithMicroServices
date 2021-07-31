using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using Dapper;

using FourWallsInc.Entity.LobApp.CMS;
using FourWallsInc.Infrastructure.ConfigMgmt;

namespace CMS.IDP.App.DataAccess
{
	/// <summary>
	/// Abstracts the authentication related tasks.
	/// </summary>
	public sealed class CmsAuthenticationDataAccess
		: ICmsAuthenticationDataAccess
	{
		private readonly string connectionString;

		public CmsAuthenticationDataAccess
			(
				IConfigManager configManager
			)
		{
			this.connectionString = configManager.GetConnectionString ("DefaultConnection");
		}

		/// <summary>
		/// Checks if the specified CMS credentials are valid.
		/// </summary>
		/// <param name="loginId">The login ID.</param>
		/// <param name="password">The password.</param>
		/// <returns></returns>
		public bool AreCmsCredentialsValid (string loginId, string password)
		{
			// Prepare the SP parameters.
			var dynamicParams = new DynamicParameters (new { cmsLoginId = loginId, cmsPassword = password });

			// ↓↓ ⮦⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺ SUPER CATCH: Dapper apparently considers "bit" type as a number, and hence Int32 is
			// used to accept the output parameter value that holds if the credentials are valid or not.
			dynamicParams.Add ("@areCredentialsValid", DbType.Int32, direction: ParameterDirection.Output);

			using (var dbConnection = new SqlConnection (this.connectionString))
			{
				// Execute the SP.
				dbConnection.Execute
					(
						"[dbo].[usp_authenticateCmsUser]",
						dynamicParams,
						commandType: CommandType.StoredProcedure
					);

				// Get the output parameter value.
				var credentialsValidityBit = dynamicParams.Get<int> ("@areCredentialsValid");

				return (credentialsValidityBit == 1);
			}
		}

		/// <summary>
		/// Gets the roles of the specified login ID.
		/// </summary>
		/// <param name="loginId">The login ID.</param>
		/// <returns></returns>
		public IList<CmsRole> GetRolesOf (string loginId)
		{
			using (var dbConnection = new SqlConnection (this.connectionString))
			{
				// Execute the SP to fetch roles of the specified login ID.
				var roles
					= dbConnection.Query<CmsRole>
						(
							"[dbo].[usp_getRolesOfCmsUser]",
							new { cmsLoginId = loginId },
							commandType: CommandType.StoredProcedure
						);

				return roles.AsList ();
			}
		}

		/// <summary>
		/// Gets the CMS user corresponding to the login ID specified.
		/// </summary>
		/// <param name="loginId">The login ID.</param>
		/// <param name="password">The password.</param>
		/// <returns></returns>
		public CmsUser GetCmsUserOf (string loginId)
		{
			using (var dbConnection = new SqlConnection (this.connectionString))
			{
				// Execute the SP to fetch roles of the specified login ID.
				var cmsUser
					= dbConnection
						.Query<CmsUser>
							(
								"[dbo].[usp_getCmsUser]",
								new { cmsLoginId = loginId },
								commandType: CommandType.StoredProcedure
							)
						.AsList ()
						.FirstOrDefault ();

				return cmsUser;
			}
		}
	}
}

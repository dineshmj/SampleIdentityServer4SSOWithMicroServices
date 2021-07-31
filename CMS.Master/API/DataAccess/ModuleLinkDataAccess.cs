using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

using Dapper;

using FourWallsInc.DataAccess;
using FourWallsInc.Entity.LobApp.CMS;
using FourWallsInc.Infrastructure.ConfigMgmt;

namespace CMS.Master.Api.DataAccess
{
	/// <summary>
	/// A data access class for module and links info.
	/// </summary>
	public sealed class ModuleLinkDataAccess
		: IModuleLinkDataAccess
	{
		private readonly string connectionString;

		public ModuleLinkDataAccess (IConfigManager configManager)
		{
			this.connectionString = configManager.GetConnectionString (DAConstants.DEFAULT_CONNECTION);
		}

		/// <summary>
		/// Gets the module and link details for the specified login ID.
		/// </summary>
		/// <param name="roleNames">A list of roles to which the signed in user
		/// has access.</param>
		/// <returns></returns>
		public IList<CmsModuleAndLinkInfo> GetModuleAndLinkDetailsFor (IList<string> roleNames)
		{
			const string TVP_ROLE_NAME_PARAM_TYPE = "[dbo].[tvpRoleName]";
			const string TVP_COLUMN_ROLE_NAME = "RoleName";
			const string USP_GET_MODULE_AND_LINKS_FOR_ROLES = "[dbo].[usp_getModuleAndLinksOfCmsUser]";

			// Prepare the Table-valued parameter for the SP.
			var tvpRoleNames = new DataTable ();
			tvpRoleNames.Columns.Add (TVP_COLUMN_ROLE_NAME, typeof (string));

			// Add each role to the TVP.
			foreach (var oneRoleName in roleNames)
			{
				tvpRoleNames.Rows.Add (oneRoleName);
			}

			// Prepare the SP parameters.
			using (var dbConnection = new SqlConnection (this.connectionString))
			{
				// Execute the SP.
				var moduleAndLinks
					= dbConnection
						.Query<CmsModuleAndLinkInfo>
							(
								USP_GET_MODULE_AND_LINKS_FOR_ROLES,
								new
								{
									roleNames
										= tvpRoleNames.AsTableValuedParameter (TVP_ROLE_NAME_PARAM_TYPE)
								},
								commandType: CommandType.StoredProcedure
							)
						.AsList ();

				return (new ReadOnlyCollection<CmsModuleAndLinkInfo> (moduleAndLinks));
			}
		}
	}
}

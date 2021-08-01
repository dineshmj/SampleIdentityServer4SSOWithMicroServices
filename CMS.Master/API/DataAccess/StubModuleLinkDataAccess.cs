using System.Collections.Generic;
using System.Collections.ObjectModel;

using FourWallsInc.Entity.LobApp.CMS;

namespace CMS.Master.Api.DataAccess
{
	// A data access class for module and links info.
	public sealed class StubModuleLinkDataAccess
		: IModuleLinkDataAccess
	{
		// Gets the module and link details for the specified login ID.
		public IList<CmsModuleAndLinkInfo> GetModuleAndLinkDetailsFor (IList<string> roleNames)
		{
			var moduleAndLinks = new List<CmsModuleAndLinkInfo> ();

			foreach (var oneRole in roleNames)
			{
				var moduleAndLinksForThisRole = this.GetModuleAndLinkForRole (oneRole);

				if (moduleAndLinksForThisRole.Count > 0)
					moduleAndLinks.AddRange (moduleAndLinksForThisRole);
			}

			return new ReadOnlyCollection<CmsModuleAndLinkInfo> (moduleAndLinks);
		}

		private List<CmsModuleAndLinkInfo> GetModuleAndLinkForRole (string roleName)
		{
			switch (roleName)
			{
				case "Quotes Management":
					return new List<CmsModuleAndLinkInfo> {
						new CmsModuleAndLinkInfo {
							ModuleName = "Quotes",
							SubModuleName = "Quotes Management",
							CmsRoleId = 10101,
							RelativeUri = "http://localhost:57364/Quotes/IssueQuote",
							ClickableLinkDisplayLabel = "Issue Quote"
						},
						new CmsModuleAndLinkInfo {
							ModuleName = "Quotes",
							SubModuleName = "Quotes Management",
							CmsRoleId = 10101,
							RelativeUri = "http://localhost:57364/Quotes/ModifyQuote",
							ClickableLinkDisplayLabel = "Modify Quote"
						},
					};

				case "Quotes Archive":
					return new List<CmsModuleAndLinkInfo> {
						new CmsModuleAndLinkInfo {
							ModuleName = "Quotes",
							SubModuleName = "Quotes Archive",
							CmsRoleId = 10102,
							RelativeUri = "http://localhost:51289/NotImplemented/Index",
							ClickableLinkDisplayLabel = "Search Quote"
						},
						new CmsModuleAndLinkInfo {
							ModuleName = "Quotes",
							SubModuleName = "Quotes Archive",
							CmsRoleId = 10102,
							RelativeUri = "http://localhost:51289/NotImplemented/Index",
							ClickableLinkDisplayLabel = "Archive Quote"
						},
					};

				case "Proposals Management":
					return new List<CmsModuleAndLinkInfo> {
						new CmsModuleAndLinkInfo {
							ModuleName = "Proposals",
							SubModuleName = "Proposals Management",
							CmsRoleId = 10201,
							RelativeUri = "http://localhost:53458/Proposals/ConvertQuoteToProposal",
							ClickableLinkDisplayLabel = "Convert Quote to Proposal"
						},
						new CmsModuleAndLinkInfo {
							ModuleName = "Proposals",
							SubModuleName = "Proposals Management",
							CmsRoleId = 10201,
							RelativeUri = "http://localhost:53458/Proposals/Search",
							ClickableLinkDisplayLabel = "Search Proposals"
						},
					};

				case "Broker Management":
					return new List<CmsModuleAndLinkInfo> {
						new CmsModuleAndLinkInfo {
							ModuleName = "Proposals",
							SubModuleName = "Broker Management",
							CmsRoleId = 10202,
							RelativeUri = "http://localhost:51289/NotImplemented/Index",
							ClickableLinkDisplayLabel = "Manage Brokers"
						},
						new CmsModuleAndLinkInfo {
							ModuleName = "Proposals",
							SubModuleName = "Broker Management",
							CmsRoleId = 10202,
							RelativeUri = "http://localhost:51289/NotImplemented/Index",
							ClickableLinkDisplayLabel = "Search Brokers"
						},
					};

				case "Policy Issuance":
					return new List<CmsModuleAndLinkInfo> {
						new CmsModuleAndLinkInfo {
							ModuleName = "Policy Administration",
							SubModuleName = "Policy Issuance",
							CmsRoleId = 10301,
							RelativeUri = "http://localhost:51289/NotImplemented/Index",
							ClickableLinkDisplayLabel = "Issue Policy"
						},
						new CmsModuleAndLinkInfo {
							ModuleName = "Policy Administration",
							SubModuleName = "Policy Issuance",
							CmsRoleId = 10301,
							RelativeUri = "http://localhost:51289/NotImplemented/Index",
							ClickableLinkDisplayLabel = "Search Policies"
						},
					};

				case "Endorsements":
					return new List<CmsModuleAndLinkInfo> {
						new CmsModuleAndLinkInfo {
							ModuleName = "Policy Administration",
							SubModuleName = "Endorsements",
							CmsRoleId = 10302,
							RelativeUri = "http://localhost:51289/NotImplemented/Index",
							ClickableLinkDisplayLabel = "Add Endorsements"
						},
						new CmsModuleAndLinkInfo {
							ModuleName = "Policy Administration",
							SubModuleName = "Endorsements",
							CmsRoleId = 10302,
							RelativeUri = "http://localhost:51289/NotImplemented/Index",
							ClickableLinkDisplayLabel = "Search Endorsements"
						},
					};

				case "Claims Management":
					return new List<CmsModuleAndLinkInfo> {
						new CmsModuleAndLinkInfo {
							ModuleName = "Claims",
							SubModuleName = "Claims Management",
							CmsRoleId = 10401,
							RelativeUri = "http://localhost:51289/NotImplemented/Index",
							ClickableLinkDisplayLabel = "Raise New Claim"
						}
					};
			}

			// The role doesn't match with any known roles.
			// Send an empty list.
			return new List<CmsModuleAndLinkInfo> ();
		}
	}
}

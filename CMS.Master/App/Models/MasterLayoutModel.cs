using System.Collections.Generic;

namespace CMS.Master.App.Models
{
	public sealed class MasterLayoutModel
	{
		public string CompanyName { get; set; }

		public string ApplicationName { get; set; }

		public string Version { get; set; }

		public IEnumerable<ModuleModel> ModuleModels { get; set; }
	}
}

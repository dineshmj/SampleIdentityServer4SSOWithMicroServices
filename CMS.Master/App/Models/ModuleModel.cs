using System.Collections.Generic;

namespace CMS.Master.App.Models
{
	public sealed class ModuleModel
	{
		public string ModuleName { get; set; }

		public IList<SubModuleModel> SubModules { get; set; }
	}
}

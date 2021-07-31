using System.Collections.Generic;

namespace CMS.Master.App.Models
{
	public sealed class SubModuleModel
	{
		public string SubModuleName { get; set; }

		public IEnumerable<LinkUriModel> Links { get; set; }

	}
}

using System.Collections.Generic;
using System.Linq;
using GdprService;

namespace GdprClientConsole
{
	public class IsMarkedForDeletion : IFileFilter
	{
		private readonly IEnumerable<string> informationTypeNamesThatMarkDeletion;

		public IsMarkedForDeletion()
		{
			this.informationTypeNamesThatMarkDeletion = new List<string>
			{
				"CPR-nummer",
			};
		}

		public IEnumerable<ScannedFile> Apply(IEnumerable<ScannedFile> files)
		{
			return files.Where(f => this.informationTypeNamesThatMarkDeletion.Contains(f.InformationTypeName));
		}

		public FilterProcessResult ProcessThisFile(ScannedFile file)
		{
			return new FilterProcessResult(true, "Marked for deletion", file);
		}
	}
}
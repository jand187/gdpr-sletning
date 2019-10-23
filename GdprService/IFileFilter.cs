using System.Collections.Generic;

namespace GdprService
{
	public interface IFileFilter
	{
		IEnumerable<ScannedFile> Apply(IEnumerable<ScannedFile> files);
		FilterProcessResult ProcessThisFile(ScannedFile file);
	}
}
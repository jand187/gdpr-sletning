using System.Threading.Tasks;

namespace GdprService
{
	public interface IScannedFileMapper
	{
		Task<ScannedFile> Map(string line);
	}

	public class ScannedFileMapper : IScannedFileMapper
	{
		public async Task<ScannedFile> Map(string line)
		{
			return await Task.Run(() => new ScannedFile());
		}
	}
}

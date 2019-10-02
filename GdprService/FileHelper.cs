using System.IO;
using System.Threading.Tasks;

namespace GdprService
{
	public interface IFileHelper
	{
		Task Delete(ScannedFile file);
		string ReadAllText(string path);
	}

	public class FileHelper : IFileHelper
	{
		public async Task Delete(ScannedFile file)
		{
			throw new System.NotImplementedException();
		}

		public string ReadAllText(string path)
		{
			throw new System.NotImplementedException();
		}
	}
}
using System.IO;
using System.Threading.Tasks;

namespace GdprService
{
	public interface IFileHelper
	{
		Task Delete(ScannedFile file);
	}

	public class FileHelper : IFileHelper
	{
		private readonly ILogger logger;

		public FileHelper(ILogger logger)
		{
			this.logger = logger;
		}

		public async Task Delete(ScannedFile file)
		{
			this.logger.Log($"Deleting file '{file.Filename}'.");
			if (!File.Exists(file.Filename))
			{
				throw new FileNotFoundException("File not found", file.Filename);
			}
			File.Delete(file.Filename);
		}
	}
}

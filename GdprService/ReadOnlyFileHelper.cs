using System.Threading.Tasks;

namespace GdprService
{
	public class ReadOnlyFileHelper : IFileHelper
	{
		private readonly ILogger logger;

		public ReadOnlyFileHelper(ILogger logger)
		{
			this.logger = logger;
		}

		public async Task Delete(ScannedFile file)
		{
			this.logger.Log($"Deleting (dry-run) file '{file.Filename}'.");
		}
	}
}

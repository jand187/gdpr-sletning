using System.Collections.Generic;
using System.Linq;

namespace GdprService
{
	public class GdprService
	{
		private readonly IFileHelper fileHelper;
		private readonly ILogger logger;

		public GdprService(IFileHelper fileHelper, ILogger logger)
		{
			this.fileHelper = fileHelper;
			this.logger = logger;
		}
		
		public void DeleteFiles(IEnumerable<ScannedFile> files)
		{
			files.ToList().ForEach(f => this.fileHelper.Delete(f));
		}
	}

	public interface IFileHelper
	{
		void Delete(ScannedFile file);
	}

	public interface ILogger
	{
		void Log(string message);
	}

	public class ScannedFile
	{
	}
}

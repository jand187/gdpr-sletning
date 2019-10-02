using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GdprService
{
	public interface IGdprService
	{
		void DeleteFiles(IEnumerable<ScannedFile> files);
	}

	public class GdprService : IGdprService
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
			files.ToList()
				.ForEach(
					f =>
					{
						try
						{
							this.fileHelper.Delete(f);
						}
						catch (FileNotFoundException e)
						{
							this.logger.LogError(e.Message, e);
						}
						catch (UnauthorizedAccessException e)
						{
							this.logger.LogError(e.Message, e);
						}
					});
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GdprService
{
	public interface IGdprService
	{
		Task DeleteFiles(IEnumerable<ScannedFile> files);
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

		public async Task DeleteFiles(IEnumerable<ScannedFile> files)
		{
			var task = files.Select(
				async f =>
				{
					try
					{
						await this.fileHelper.Delete(f);
						this.logger.Log($"Delete file '{f.Filename}'.");
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

			await Task.WhenAll(task);
		}
	}
}

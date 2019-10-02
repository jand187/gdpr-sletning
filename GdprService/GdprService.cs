using System;
using System.Collections.Generic;
using System.IO;
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
							this.logger.Log(e.Message, e);
						}
						catch (UnauthorizedAccessException e)
						{
							this.logger.Log(e.Message, e);
						}
					});
		}
	}

	public interface IFileHelper
	{
		void Delete(ScannedFile file);
	}

	public interface ILogger
	{
		void Log(string message, Exception exception);
	}

	public class ScannedFile
	{
		public string Filename { get; set; }
	}
}

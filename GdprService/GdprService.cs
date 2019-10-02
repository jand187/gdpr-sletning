using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GdprService
{
	public interface IGdprService
	{
		Task DeleteFiles(IEnumerable<ScannedFile> files, IFileFilter[] filters);
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

		public async Task DeleteFiles(IEnumerable<ScannedFile> files, params IFileFilter[] filters)
		{
			var filteredFiles = filters.Aggregate(files, (current, filter) => filter.Apply(current));

			var task = filteredFiles.Select(
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

	public class GenericFileFilter : IFileFilter
	{
		private readonly Func<ScannedFile, bool> predicate;

		public GenericFileFilter(Func<ScannedFile, bool> predicate)
		{
			this.predicate = predicate;
		}

		public IEnumerable<ScannedFile> Apply(IEnumerable<ScannedFile> files)
		{
			return files.Where(this.predicate);
		}
	}
}

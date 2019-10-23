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
		private readonly IGdprReport gdprReport;

		public GdprService(IFileHelper fileHelper, ILogger logger, IGdprReport gdprReport)
		{
			this.fileHelper = fileHelper;
			this.logger = logger;
			this.gdprReport = gdprReport;
		}

		public async Task DeleteFiles(IEnumerable<ScannedFile> files, params IFileFilter[] filters)
		{
			var filteredFiles = filters.Aggregate(files, (current, filter) => filter.Apply(current));

			var task = filteredFiles.Select(
				async f =>
				{
					try
					{
						if (filters.All(filter=>filter.IsAllowed(f)))
						{
							await this.fileHelper.Delete(f);
							await this.gdprReport.RegisterDeleted(f);
						}
						else
						{
							await this.gdprReport.RegisterNotDeleted(f, "File was filtered"); //TODO: JDAN add more informative reason.
						}
					}
					catch (FileNotFoundException e)
					{
						this.logger.LogError(e.Message, e);
						await this.gdprReport.RegisterFailed(f, e.Message);
					}
					catch (UnauthorizedAccessException e)
					{
						this.logger.LogError(e.Message, e);
						await this.gdprReport.RegisterFailed(f, e.Message);
					}
				});

			await Task.WhenAll(task);
		}

		public async Task DeleteFilesDryRun(IEnumerable<ScannedFile> files, params IFileFilter[] filters)
		{
			var filteredFiles = filters.Aggregate(files, (current, filter) => filter.Apply(current));

			var task = filteredFiles.Select(
				async f =>
				{
					try
					{
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

	//public class GenericFileFilter : IFileFilter
	//{
	//	private readonly Func<ScannedFile, bool> predicate;

	//	public GenericFileFilter(Func<ScannedFile, bool> predicate = null)
	//	{
	//		this.predicate = predicate;
	//	}

	//	public IEnumerable<ScannedFile> Apply(IEnumerable<ScannedFile> files)
	//	{
	//		return files.Where(this.predicate);
	//	}
	//}
}

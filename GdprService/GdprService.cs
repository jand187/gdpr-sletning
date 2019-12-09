using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

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
						var allFiltersStatus = filters.Select(filter => filter.ProcessThisFile(f)).ToList();
						if (allFiltersStatus.All(filterStatus => filterStatus.Status))
						{
							await this.fileHelper.Delete(f);
							await this.gdprReport.RegisterDeleted(f);
						}
						else
						{
							await this.gdprReport.RegisterNotDeleted(
								f,
								string.Join(
									Environment.NewLine,
									allFiltersStatus.Select(
										filterStatus =>
											filterStatus.Reason))); //TODO: JDAN add more informative reason.
						}
					}
					catch (FileNotFoundException e)
					{
						this.logger.LogError(e.Message, e);
						await this.gdprReport.RegisterFailed(f, $"{f.Filename} File not Found.");
					}
					catch (ArgumentException e)
					{
						this.logger.LogError(e.Message, e);
						await this.gdprReport.RegisterFailed(f, $"{f.Filename}: ArgumentException: {e.Message}.");
					}
					catch (UnauthorizedAccessException e)
					{
						this.logger.LogError(e.Message, e);
						await this.gdprReport.RegisterFailed(f, $"{f.Filename} Access Denied.");
					}
					catch (WebException e)
					{
						this.logger.LogError(e.Message, e);
						await this.gdprReport.RegisterFailed(f, $"{f.Filename} Unauthorized access.");
					}
					catch (ServerUnauthorizedAccessException e)
					{
						this.logger.LogError(e.Message, e);
						await this.gdprReport.RegisterFailed(f, $"{f.Filename} Unauthorized access.");
					}
				});

			await Task.WhenAll(task);
		}
	}
}

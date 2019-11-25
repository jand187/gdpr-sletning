using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GdprService;
using Microsoft.SharePoint.Client;

namespace GdprClientConsole
{
	public class MoreThanFiveYearsOldSharePoint : IFileFilter
	{
		private readonly DateTime thresholdDate;

		public MoreThanFiveYearsOldSharePoint()
		{
			this.thresholdDate = new DateTime(DateTime.Today.AddYears(-5).Year, 1, 1);
		}

		public IEnumerable<ScannedFile> Apply(IEnumerable<ScannedFile> files)
		{
			return files.Where(f => f.LastModified < this.thresholdDate);
		}

		public FilterProcessResult ProcessThisFile(ScannedFile file)
		{
			using (var context = new ClientContext(file.Repository))
			{
				var serverRelativeUrl = GetRelativeUrl(file);
				var spFile = context.Web.GetFileByServerRelativeUrl(serverRelativeUrl);
				context.Load(spFile);
				context.ExecuteQuery();
				
				return new FilterProcessResult(
					spFile.TimeLastModified < this.thresholdDate,
					$"{file.Filename} is modified after {this.thresholdDate} and should not be deleted.",
					file);
			}
		}

		private string GetRelativeUrl(ScannedFile file)
		{
			var path = Regex.Replace(file.Filename, @"^http:\/\/[^\/]+", "");
			return path;
		}

	}

}
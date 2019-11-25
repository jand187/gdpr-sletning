using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GdprService;

namespace GdprClientConsole
{
	public class MoreThanFiveYearsOld : IFileFilter
	{
		private readonly DateTime thresholdDate;

		public MoreThanFiveYearsOld()
		{
			this.thresholdDate = new DateTime(DateTime.Today.AddYears(-5).Year, 1, 1);
		}

		public IEnumerable<ScannedFile> Apply(IEnumerable<ScannedFile> files)
		{
			return files.Where(f => f.LastModified < this.thresholdDate);
		}

		public FilterProcessResult ProcessThisFile(ScannedFile file)
		{
			return new FilterProcessResult(
				new FileInfo(file.Filename).LastWriteTime < this.thresholdDate,
				$"{file.Filename} is modified after {this.thresholdDate} and should not be deleted.",
				file);
		}
	}
}

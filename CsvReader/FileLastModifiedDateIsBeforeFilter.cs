using System;

namespace CsvReader
{
	public class FileLastModifiedDateIsBeforeFilter : IFileFilter<DateTime>
	{
		private readonly DateTime lastModifiedDate;

		public FileLastModifiedDateIsBeforeFilter(DateTime lastModifiedDate)
		{
			this.lastModifiedDate = lastModifiedDate;
		}

		public bool IsValid(DateTime target)
		{
			return this.lastModifiedDate > target;
		}
	}
}
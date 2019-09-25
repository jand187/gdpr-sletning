using System;
using System.IO;

namespace CsvReader
{
	public class FileLastModifiedDateIsBeforeFilter : IFileFilter
	{
		private readonly DateTime lastModifiedDate;

		public FileLastModifiedDateIsBeforeFilter(DateTime lastModifiedDate)
		{
			this.lastModifiedDate = lastModifiedDate;
		}

		public bool IsValid(IFileData file)
		{
			return this.lastModifiedDate > file.LastModifiedDate;
		}
	}
}
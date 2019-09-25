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

		public bool IsValid(ScannedFile file)
		{
			throw new NotImplementedException();
		}
	}

	public class GenericFilter:IFileFilter
	{
		private readonly Func<ScannedFile, bool> predicate;

		public GenericFilter(Func<ScannedFile, bool> predicate)
		{
			this.predicate = predicate;
		}

		public bool IsValid(IFileData file)
		{
			throw new NotImplementedException();
		}

		public bool IsValid(ScannedFile file)
		{
			return this.predicate.Invoke(file);
		}
	}
}
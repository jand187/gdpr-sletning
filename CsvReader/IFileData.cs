using System;

namespace CsvReader
{
	public interface IFileData
	{
		DateTime LastModifiedDate { get; }
		string Filename { get; }
	}

	class FileData : IFileData
	{
		public FileData(string filename)
		{
			Filename = filename;
		}

		public DateTime LastModifiedDate { get; }
		public string Filename { get; }
	}
}
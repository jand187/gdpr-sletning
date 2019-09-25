using System;

namespace CsvReader
{
	public interface IFileFilter
	{
		bool IsValid(IFileData file);
	}


	public interface IFileDataFactory
	{
		IFileData Create(string filename);
	}

	public interface IFileData
	{
		DateTime LastModifiedDate { get; set; }
		string Filename { get; set; }
	}
}

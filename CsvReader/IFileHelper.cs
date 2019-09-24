using System;
using System.IO;

namespace CsvReader
{
	public interface IFileHelper
	{
		DateTime GetCreateDate(FileInfo file);
		string ReadAllText(FileInfo file);
	}
}
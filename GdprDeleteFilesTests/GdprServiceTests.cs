using CsvReader;
using GdprDeleteFiles;
using NUnit.Framework;

namespace GdprDeleteFilesTests
{
	public class GdprServiceTests
	{
		[Test]
		public void DeleteFiles_should_delete_files_older_than_five_years()
		{

			var target = new GdprService();

			target.DeleteFiles(new ScannedFile[] {new ScannedFile(),});
		}
	}
}

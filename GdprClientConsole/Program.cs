using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GdprService;

namespace GdprClientConsole
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var file1 = new ScannedFile
			{
				Filename = @"C:\temp\d.txt"
			};

			var file2 = new ScannedFile
			{
				Filename = @"C:\temp\not existing.txt"
			};

			var fileHelper = new FileHelper();
			var consoleLogger = new ConsoleLogger();

			var service = new GdprService.GdprService(fileHelper, consoleLogger);

			var scannedFileMapper = new ScannedFileMapper();
			var reader = new CsvReader(fileHelper, scannedFileMapper);
			var files = reader.Parse($@"{Environment.CurrentDirectory}\..\..\..\Test Files\Fildrev.csv").Result;

			var fileExistFilter = new GdprService.GenericFileFilter(file => File.Exists(file.Filename));

			service.DeleteFiles(files, fileExistFilter).Wait();

			Console.ReadKey();
		}
	}

	internal class GenericFileFilter : IFileFilter
	{
		private readonly Func<ScannedFile, bool> predicate;

		public GenericFileFilter(Func<ScannedFile, bool> predicate)
		{
			this.predicate = predicate;
		}

		public IEnumerable<ScannedFile> Apply(IEnumerable<ScannedFile> files)
		{
			return files.Where(this.predicate);
		}
	}
}

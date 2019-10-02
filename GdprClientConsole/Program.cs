using System;
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

			service.DeleteFiles(files).Wait();

			Console.ReadKey();
		}
	}
}

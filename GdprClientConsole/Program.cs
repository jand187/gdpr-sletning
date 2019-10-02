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
				Filename = @"C:\temp\d.txt",
			};
			
			var file2 = new ScannedFile
			{
				Filename = @"C:\temp\not existing.txt",
			};


			var fileHelper = new FileHelper();
			var consoleLogger = new ConsoleLogger();

			var service = new GdprService.GdprService(fileHelper, consoleLogger);

			service.DeleteFiles(new[] {file1, file2}).Wait();

			Console.ReadKey();
		}
	}
}

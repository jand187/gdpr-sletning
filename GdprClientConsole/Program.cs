using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GdprService;

namespace GdprClientConsole
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var consoleLogger = new ConsoleLogger();

			var command = CreateCommand(args, consoleLogger);

			command.Execute().Wait();
			Console.ReadKey();
		}

		private static IGdprCommand CreateCommand(string[] args, ConsoleLogger consoleLogger)
		{
			switch (args[0].ToLower())
			{
				case "delete":
					return new GdprDeleteCommand(args, consoleLogger);

				default:
					return new GdprDefaultCommand(args, consoleLogger);
			}
		}
	}

	public class GdprDeleteCommand : IGdprCommand
	{
		private readonly string[] args;
		private readonly ConsoleLogger consoleLogger;
		private readonly FileInfo filename;

		public GdprDeleteCommand(string[] args, ConsoleLogger consoleLogger)
		{
			this.args = args;
			this.consoleLogger = consoleLogger;

			var optionFIndex = args.ToList().IndexOf("-f");
			this.filename = new FileInfo(
				$"{Environment.CurrentDirectory}{args.Skip(optionFIndex + 1).Take(1).Single()}");
		}

		public async Task Execute()
		{
			this.consoleLogger.Log($"Parsing csv-file '{filename.FullName}'.");
			
			var fileHelper = new FileHelper();
			var scannedFileMapper = new ScannedFileMapper();
			
			var reader = new CsvReader(fileHelper, scannedFileMapper);
			var files = reader.Parse($@"{Environment.CurrentDirectory}\..\..\..\Test Files\Fildrev.csv").Result.ToList();
			this.consoleLogger.Log($"Successfully parsed csv-file '{filename.FullName}'. {files.Count()} entries found!");

			this.consoleLogger.Log("Deleting files...");
			var service = new GdprService.GdprService(fileHelper, consoleLogger);
			var fileExistFilter = new GdprService.GenericFileFilter(file => File.Exists(file.Filename));

			await service.DeleteFiles(files, fileExistFilter);
		}
	}

	public class GdprDefaultCommand : IGdprCommand
	{
		private readonly string[] args;
		private readonly ConsoleLogger consoleLogger;

		public GdprDefaultCommand(string[] args, ConsoleLogger consoleLogger)
		{
			this.args = args;
			this.consoleLogger = consoleLogger;
		}

		public Task Execute()
		{
			return Task.Run(
				() =>
				{
					this.consoleLogger.Log($"Command '{this.args[0]}' not recognised.");
					this.consoleLogger.Log($"Supplied args: '{string.Join(" ", this.args)}'");
				});
		}
	}

	public interface IGdprCommand
	{
		Task Execute();
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

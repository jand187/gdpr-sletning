using System;
using System.Collections.Generic;
using System.Linq;
using GdprService;

namespace GdprClientConsole
{
	internal class Program
	{
		private static readonly ILogger Logger;
		private static readonly IFileHelper FileHelper;
		private static readonly IFileHelper ReadOnlyFileHelper;
		private static readonly IScannedFileMapper ScannedFileMapper;

		static Program()
		{
			Logger = new ConsoleLogger();
			FileHelper = new FileHelper(Logger);
			ReadOnlyFileHelper = new ReadOnlyFileHelper(Logger);
			ScannedFileMapper = new ScannedFileMapper();
		}

		private static void Main(string[] args)
		{
			if (!args.Any())
			{
				DisplayCommandLineHelp();
				Console.ReadKey();
				return;
			}
			
			var command = CreateCommand(args);

			command.Execute().Wait();
			Console.ReadKey();
		}

		private static void DisplayCommandLineHelp()
		{
			Console.WriteLine("You must specify a command.");
		}

		private static string[] RequestArgs()
		{
			Console.WriteLine("Which command do you wish to execute:");
			Console.WriteLine("1. Delete");
			Console.WriteLine("2. DryRun (delete)");
			var key = Console.ReadLine();
			Console.Clear();

			switch (key)
			{
				case "1":
					Console.WriteLine("You must supply a filename. Enter it and press enter");
					var filename = Console.ReadLine();
					Console.WriteLine("Dry run (y/n)?");
					var dryRun = Console.ReadLine().ToLower() == "y" ? "-d" : "";

					Console.Clear();
					return new[] {"delete", "-f", filename, dryRun};

				default:
					Console.WriteLine("Command not accepted.");
					Console.WriteLine(string.Empty);
					return RequestArgs();
			}

			throw new NotImplementedException();
		}

		private static IGdprCommand CreateCommand(string[] args)
		{
			switch (args[0].ToLower())
			{
				case "delete":
					// delete -f "\..\..\..\Test Files\Fildrev.csv"
					return new GdprDeleteCommand(args, Logger, FileHelper, ScannedFileMapper);

				case "delete-dry-run":
					// delete-dry-run -f "\..\..\..\Test Files\Fildrev.csv"
					return new GdprDeleteCommand(args, Logger, ReadOnlyFileHelper, ScannedFileMapper);

				case "fixShareNames":
					return new GdprFixShareNames(args, Logger);

				default:
					return new GdprDefaultCommand(args, Logger);
			}
		}
	}
}

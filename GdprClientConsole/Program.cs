using System;
using System.Collections.Generic;
using System.Linq;
using GdprService;

namespace GdprClientConsole
{
	internal class Program
	{
		private static readonly ConsoleLogger Logger;
		private static readonly FileHelper FileHelper;
		private static readonly ScannedFileMapper ScannedFileMapper;

		static Program()
		{
			Logger = new ConsoleLogger();
			FileHelper = new FileHelper();
			ScannedFileMapper = new ScannedFileMapper();
		}

		private static void Main(string[] args)
		{
			if (!args.Any())
			{
				args = RequestArgs();
			}
			
			var command = CreateCommand(args);

			command.Execute().Wait();
			Console.ReadKey();
		}

		private static string[] RequestArgs()
		{
			Console.WriteLine("Whitch command do you wish to execute:");
			Console.WriteLine("1. Delete");
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
					return new GdprDeleteCommand(args, Logger, FileHelper, ScannedFileMapper);
				
				case "fixShareNames":
					return new GdprFixShareNames(args, Logger);

				default:
					return new GdprDefaultCommand(args, Logger);
			}
		}
	}
}

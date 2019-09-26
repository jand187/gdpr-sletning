using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CsvReader;

namespace GdprDeleteFiles
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var arguments = ParseArgs(args);

			Console.WriteLine($"Command: {arguments.Command} Filename: {arguments.Filename}");

			var reader = new CsvReader.DefaultReader(null,null);
			var file = new FileInfo(arguments.Filename);
			var createFileSetTask = reader.CreateFileSet(file);
			createFileSetTask.Wait();

			var fileSet = createFileSetTask.Result.ToList();

			fileSet.ForEach(f => Console.WriteLine(f.FileName + (f.ParseError ? " - *** errors during parsing." : string.Empty)));

			Console.ReadKey();
		}

		private static CsvReaderArgs ParseArgs(string[] args)
		{
			var command = args.First();
			var options = args.Skip(1);

			switch (command.ToLower())
			{
				case "show":

					var index = options.ToList().IndexOf("-f");
					var filename = options.Skip(index).Skip(1).Take(1).Single();

					return new CsvReaderArgs
					{
						Command = command,
						Filename = filename,
					};

				case "Delete":
					return new CsvReaderArgs();

				default:
					Console.WriteLine("No command was found");
					return new CsvReaderArgs();
			}
		}
	}

	internal class CsvReaderArgs
	{
		public string Command { get; set; }
		public string Filename { get; set; }
	}

	internal class DefaultFileHelper : IFileHelper
	{
		public DateTime GetCreateDate(FileInfo file)
		{
			throw new NotImplementedException();
		}

		public string ReadAllText(FileInfo file)
		{
			throw new NotImplementedException();
		}
	}
}

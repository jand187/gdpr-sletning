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
			var arguments = new GdprDeleteFilesArgumentParser().Parse(args);

			Console.WriteLine($"Command: {arguments.Command} Filename: {arguments.Filename}");

			var reader = new CsvReader.DefaultReader(null,null);
			var file = new FileInfo(arguments.Filename);
			var createFileSetTask = reader.CreateFileSet(file);
			createFileSetTask.Wait();

			var fileSet = createFileSetTask.Result.ToList();

			fileSet.ForEach(f => Console.WriteLine(f.FileName + (f.ParseError ? " - *** errors during parsing." : string.Empty)));

			Console.ReadKey();
		}
	}

	public class GdprService
	{
		public GdprService()
		{
		}


		public void DeleteFiles(IEnumerable<ScannedFile> files)
		{
			throw new NotImplementedException();
		}
	}
}

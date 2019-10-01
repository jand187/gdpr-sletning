using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvReader;

namespace GdprDeleteFiles
{
	public class ConsoleCommandFactory
	{
	}

	public interface IConsoleCommand
	{
		void Execute(ScannedFile file);
	}

	public class CsvReaderArgs
	{
		public string Command { get; set; }
		public string Filename { get; set; }
	}
}

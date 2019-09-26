using System;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;

namespace IntegrationTests
{
	internal class ConsoleAppTests
	{
		[Test]
		public void Show_command_should_show_files_ind_csv()
		{
			var executable = $@"{TestContext.CurrentContext.TestDirectory}\..\..\..\GdprDeleteFiles\bin\debug\GdprDeleteFiles.exe";
			var file = new FileInfo($"{TestContext.CurrentContext.TestDirectory}\\..\\..\\..\\Test files\\Fildrev.csv");
			var arguments = $"show -f \"{file.FullName}\"";

			var processStartInfo = new ProcessStartInfo(executable) {Arguments = arguments, RedirectStandardError = true, RedirectStandardOutput = true, UseShellExecute = false};

			var process = new Process {StartInfo = processStartInfo};
			process.Start();

			Console.WriteLine(process.StandardOutput.ReadToEnd());
		}
	}
}

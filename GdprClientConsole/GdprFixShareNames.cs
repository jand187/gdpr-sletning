﻿using System;
using System.IO;
using System.Threading.Tasks;
using GdprService;

namespace GdprClientConsole
{
	public class GdprFixShareNames : IGdprCommand
	{
		private readonly ConsoleLogger consoleLogger;
		private readonly FileInfo file;
		private readonly string[] args;
		private readonly FileInfo replacementFile;

		public GdprFixShareNames(string[] args, ConsoleLogger consoleLogger)
		{
			this.args = args;
			this.consoleLogger = consoleLogger;

			var filenameOption = OptionsHelper.GetOptionParameter(args, "-f");
			this.file = new FileInfo($"{Environment.CurrentDirectory}{filenameOption}");

			var replacementFilenameOption = OptionsHelper.GetOptionParameter(args, "-r");
			this.replacementFile = new FileInfo($"{Environment.CurrentDirectory}{replacementFilenameOption}");
		}

		public async Task Execute()
		{
			await Task.Run(() =>
			{
				this.consoleLogger.Log("This command is not implemented.");
			});
		}
	}
}
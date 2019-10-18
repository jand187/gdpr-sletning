using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GdprService;

namespace GdprClientConsole
{
	public class GdprDeleteCommand : IGdprCommand
	{
		private readonly string[] args;
		private readonly ILogger consoleLogger;
		private readonly ICsvReader csvReader;
		private readonly IEnumerable<IFileFilter> fileFilters;
		private readonly IFileHelper fileHelper;
		private readonly FileInfo filename;
		private readonly IGdprService gdprService;

		public delegate GdprDeleteCommand Factory(string[] args, IFileHelper fileHelper);

		public GdprDeleteCommand(string[] args,
			ILogger consoleLogger,
			IFileHelper fileHelper,
			ICsvReader csvReader,
			IGdprService gdprService,
			params IFileFilter[] fileFilters)
		{
			this.args = args;
			this.consoleLogger = consoleLogger;
			this.fileHelper = fileHelper;
			this.csvReader = csvReader;
			this.gdprService = gdprService;
			this.fileFilters = fileFilters;

			var filenameOption = OptionsHelper.GetOptionParameter(args, "-f");
			this.filename = new FileInfo($"{Environment.CurrentDirectory}{filenameOption}");
		}

		public async Task Execute()
		{
			this.consoleLogger.Log($"Parsing csv-file '{this.filename.FullName}'.");

			var files = this.csvReader.Parse(this.filename.FullName).Result.ToList();
			this.consoleLogger.Log(
				$"Successfully parsed csv-file '{this.filename.FullName}'. {files.Count()} entries found!");

			this.consoleLogger.Log("Deleting files...");

			await this.gdprService.DeleteFiles(files, this.fileFilters.ToArray());
		}
	}
}

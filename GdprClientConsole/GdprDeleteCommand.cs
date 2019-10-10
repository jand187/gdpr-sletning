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
		private readonly ConsoleLogger consoleLogger;
		private readonly FileInfo filename;
		private readonly bool dryRun;
		private readonly IFileHelper fileHelper;
		private readonly IScannedFileMapper scannedFileMapper;
		private readonly IEnumerable<IFileFilter> fileFilters;

		public GdprDeleteCommand(string[] args, ConsoleLogger consoleLogger, IFileHelper fileHelper, IScannedFileMapper scannedFileMapper, params IFileFilter[] fileFilters)
		{
			this.args = args;
			this.consoleLogger = consoleLogger;
			this.fileHelper = fileHelper;
			this.scannedFileMapper = scannedFileMapper;
			this.fileFilters = fileFilters;

			var filenameOption = OptionsHelper.GetOptionParameter(args, "-f");
			this.filename = new FileInfo($"{Environment.CurrentDirectory}{filenameOption}");

			this.dryRun = OptionsHelper.GetSwitch(args, "-d");
		}

		public async Task Execute()
		{
			this.consoleLogger.Log($"Parsing csv-file '{this.filename.FullName}'.");

			var reader = new CsvReader(this.fileHelper, this.scannedFileMapper);
			var files = reader.Parse(this.filename.FullName).Result.ToList();
			this.consoleLogger.Log($"Successfully parsed csv-file '{this.filename.FullName}'. {files.Count()} entries found!");

			this.consoleLogger.Log("Deleting files...");
			var service = new GdprService.GdprService(this.fileHelper, this.consoleLogger);

			if (this.dryRun)
			{
				await service.DeleteFilesDryRun(files, this.fileFilters.ToArray());
			}
			else
			{
				await service.DeleteFiles(files, this.fileFilters.ToArray());
			}
		}
	}
}
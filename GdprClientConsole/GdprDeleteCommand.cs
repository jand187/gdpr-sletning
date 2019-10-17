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
		private readonly IEnumerable<IFileFilter> fileFilters;
		private readonly IFileHelper fileHelper;
		private readonly FileInfo filename;
		private readonly IScannedFileMapper scannedFileMapper;

		public GdprDeleteCommand(string[] args,
			ILogger consoleLogger,
			IFileHelper fileHelper,
			IScannedFileMapper scannedFileMapper,
			params IFileFilter[] fileFilters)
		{
			this.args = args;
			this.consoleLogger = consoleLogger;
			this.fileHelper = fileHelper;
			this.scannedFileMapper = scannedFileMapper;
			this.fileFilters = fileFilters;

			var filenameOption = OptionsHelper.GetOptionParameter(args, "-f");
			this.filename = new FileInfo($"{Environment.CurrentDirectory}{filenameOption}");
		}

		public async Task Execute()
		{
			this.consoleLogger.Log($"Parsing csv-file '{this.filename.FullName}'.");

			var reader = new CsvReader(this.fileHelper, this.scannedFileMapper);
			var files = reader.Parse(this.filename.FullName).Result.ToList();
			this.consoleLogger.Log(
				$"Successfully parsed csv-file '{this.filename.FullName}'. {files.Count()} entries found!");

			this.consoleLogger.Log("Deleting files...");
			var service = new GdprService.GdprService(this.fileHelper, this.consoleLogger);

			await service.DeleteFiles(files, this.fileFilters.ToArray());
		}
	}
}

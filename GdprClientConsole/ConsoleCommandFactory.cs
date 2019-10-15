using System.Collections.Generic;
using System.Linq;
using GdprService;

namespace GdprClientConsole
{
	internal interface ICommandFactory
	{
		IGdprCommand Create(string[] args);
	}

	internal class ConsoleCommandFactory : ICommandFactory
	{
		private readonly IEnumerable<IFileHelper> fileHelpers;
		private readonly ILogger logger;
		private readonly IScannedFileMapper scannedFileMapper;


		private IFileHelper FileHelper
		{
			get
			{
				return this.fileHelpers.Single(f => f.GetType() == typeof(FileHelper));
			}
		}

		private IFileHelper ReadOnlyFileHelper
		{
			get
			{
				return this.fileHelpers.Single(f => f.GetType() == typeof(ReadOnlyFileHelper));
			}
		}

		public ConsoleCommandFactory(ILogger logger,
			IEnumerable<IFileHelper> fileHelpers,
			IScannedFileMapper scannedFileMapper)
		{
			this.logger = logger;
			this.fileHelpers = fileHelpers;
			this.scannedFileMapper = scannedFileMapper;
		}

		public IGdprCommand Create(string[] args)
		{
			switch (args[0].ToLower())
			{
				case "delete":
					// delete -f "\..\..\..\Test Files\Fildrev.csv"
					return new GdprDeleteCommand(args, this.logger, FileHelper, this.scannedFileMapper);

				case "delete-dry-run":
					// delete-dry-run -f "\..\..\..\Test Files\Fildrev.csv"
					return new GdprDeleteCommand(args, this.logger, ReadOnlyFileHelper, this.scannedFileMapper);

				case "fixShareNames":
					return new GdprFixShareNames(args, this.logger);

				default:
					return new GdprDefaultCommand(args, this.logger);
			}
		}
	}
}

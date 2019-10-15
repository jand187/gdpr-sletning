
using GdprService;

namespace GdprClientConsole
{
	internal interface ICommandFactory
	{
		IGdprCommand Create(string[] args);
	}

	internal class ConsoleCommandFactory : ICommandFactory
	{
		private readonly ILogger logger;
		private readonly IFileHelper fileHelper;
		private readonly IFileHelper readOnlyFileHelper; // TODO: JDAN remove this.
		private readonly IScannedFileMapper scannedFileMapper;

		public ConsoleCommandFactory(ILogger logger, IFileHelper fileHelper, IScannedFileMapper scannedFileMapper)
		{
			this.logger = logger;
			this.fileHelper = fileHelper;
			this.scannedFileMapper = scannedFileMapper;
		}

		public IGdprCommand Create(string[] args)
		{
			switch (args[0].ToLower())
			{
				case "delete":
					// delete -f "\..\..\..\Test Files\Fildrev.csv"
					return new GdprDeleteCommand(args, this.logger, this.fileHelper, this.scannedFileMapper);

				case "delete-dry-run":
					// delete-dry-run -f "\..\..\..\Test Files\Fildrev.csv"
					return new GdprDeleteCommand(args, this.logger, this.readOnlyFileHelper, this.scannedFileMapper);

				case "fixShareNames":
					return new GdprFixShareNames(args, this.logger);

				default:
					return new GdprDefaultCommand(args, this.logger);
			}

		}
	}
}
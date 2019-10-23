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
		private readonly GdprDefaultCommand.Factory gdprDefaultCommandFactory;
		private readonly GdprDeleteCommand.Factory gdprDeleteCommandFactory;
		private readonly GdprFixShareNames.Factory gdprFixShareNamesFactory;

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

		public ConsoleCommandFactory(IEnumerable<IFileHelper> fileHelpers,
			GdprDeleteCommand.Factory gdprDeleteCommandFactory,
			GdprFixShareNames.Factory gdprFixShareNamesFactory,
			GdprDefaultCommand.Factory gdprDefaultCommandFactory)
		{
			this.fileHelpers = fileHelpers;
			this.gdprDeleteCommandFactory = gdprDeleteCommandFactory;
			this.gdprFixShareNamesFactory = gdprFixShareNamesFactory;
			this.gdprDefaultCommandFactory = gdprDefaultCommandFactory;
		}

		public IGdprCommand Create(string[] args)
		{
			switch (args[0].ToLower())
			{
				case "delete":
					// delete -f "\..\..\..\Test Files\Fildrev.csv"
					return this.gdprDeleteCommandFactory.Invoke(args);

				case "delete-dry-run":
					// delete-dry-run -f "\..\..\..\Test Files\Fildrev.csv"
					return this.gdprDeleteCommandFactory.Invoke(args);

				case "delete-sharepoint":
					// delete-dry-run -f "\..\..\..\Test Files\Fildrev.csv"
					return this.gdprDeleteCommandFactory.Invoke(args);

				case "fixShareNames":
					return this.gdprFixShareNamesFactory.Invoke(args);

				default:
					return this.gdprDefaultCommandFactory.Invoke(args);
			}
		}
	}
}

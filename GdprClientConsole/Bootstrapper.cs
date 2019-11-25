using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using GdprService;

namespace GdprClientConsole
{
	internal class Bootstrapper
	{
		private readonly string[] args;

		public Bootstrapper(string[] args)
		{
			this.args = args;
		}

		public IContainer BuildContainer()
		{
			var builder = new ContainerBuilder();
			// GdprClientConsole
//			builder.RegisterAssemblyTypes(GetType().Assembly).AsImplementedInterfaces();
			builder.RegisterType<ConsoleCommandFactory>().As<ICommandFactory>();

			builder.RegisterType<GdprDeleteCommand>();
			builder.RegisterType<GdprFixShareNames>();
			builder.RegisterType<GdprDefaultCommand>();


			// GdprService
			builder.RegisterType<CsvReader>().As<ICsvReader>();
			builder.RegisterType<SilentLogger>().As<ILogger>();
			builder.RegisterType<FileReader>().As<IFileReader>();
			builder.RegisterType<GdprService.GdprService>().As<IGdprService>();
			builder.RegisterType<ReadOnlyFileHelper>().As<IFileHelper>();
			builder.RegisterType<ScannedFileMapper>().As<IScannedFileMapper>();


			builder.RegisterType<GdprReport>().As<IGdprReport>().InstancePerLifetimeScope();

			ProcessOverrides(builder);

			return builder.Build();
		}

		private void ProcessOverrides(ContainerBuilder builder)
		{
			switch (this.args.First().ToLower())
			{
				case "delete":
					builder.RegisterType<FileHelper>().As<IFileHelper>();
					builder.RegisterType<MoreThanFiveYearsOld>().As<IFileFilter>();
					break;

				case "delete-dry-run":
					builder.RegisterType<MoreThanFiveYearsOld>().As<IFileFilter>();
					break;

				case "delete-sharepoint":
					builder.RegisterType<SharePointFileHelper>().As<IFileHelper>();
					break;

			}
		}
	}
}

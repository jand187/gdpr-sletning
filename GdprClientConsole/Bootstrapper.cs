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

	public class MoreThanFiveYearsOld : IFileFilter
	{
		private readonly DateTime thresholdDate;

		public MoreThanFiveYearsOld()
		{
			this.thresholdDate = new DateTime(DateTime.Today.AddYears(-5).Year, 1, 1);
		}

		public IEnumerable<ScannedFile> Apply(IEnumerable<ScannedFile> files)
		{
			return files.Where(f => f.LastModified < this.thresholdDate);
		}

		public FilterProcessResult ProcessThisFile(ScannedFile file)
		{
			return new FilterProcessResult(
				new FileInfo(file.Filename).LastWriteTime < this.thresholdDate,
				$"{file.Filename} is modified after {this.thresholdDate} and should not be deleted.",
				file);
		}
	}
}

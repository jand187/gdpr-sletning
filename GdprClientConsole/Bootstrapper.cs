using System;
using System.Collections.Generic;
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
			builder.RegisterAssemblyTypes(GetType().Assembly).AsImplementedInterfaces();

			// builder.RegisterAssemblyTypes(typeof(GdprService.GdprService).Assembly).AsImplementedInterfaces();
			builder.RegisterType<CsvReader>().As<ICsvReader>();
			builder.RegisterType<ConsoleLogger>().As<ILogger>();
			builder.RegisterType<FileReader>().As<IFileReader>();
			builder.RegisterType<GdprService.GdprService>().As<IGdprService>();
			builder.RegisterType<ReadOnlyFileHelper>().As<IFileHelper>();
			builder.RegisterType<ScannedFileMapper>().As<IScannedFileMapper>();

			builder.RegisterType<GdprDeleteCommand>();
			builder.RegisterType<GdprFixShareNames>();
			builder.RegisterType<GdprDefaultCommand>();

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
			}
		}
	}

	internal class MoreThanFiveYearsOld : IFileFilter
	{
		private readonly DateTime thresholdDate;

		public MoreThanFiveYearsOld()
		{
			this.thresholdDate = new DateTime(DateTime.Today.AddYears(-1).Year, 1, 1);
		}

		public IEnumerable<ScannedFile> Apply(IEnumerable<ScannedFile> files)
		{
			return files.Where(f => f.LastModified < this.thresholdDate);
		}
	}
}

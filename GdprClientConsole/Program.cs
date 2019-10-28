using System;
using Autofac;
using GdprService;

namespace GdprClientConsole
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var container = new Bootstrapper(args).BuildContainer();
			using (var scope = container.BeginLifetimeScope())
			{
				var commandFactory = scope.Resolve<ICommandFactory>();
				var report = scope.Resolve<IGdprReport>();

				try
				{
					var command = commandFactory.Create(args);
					command.Execute().Wait();
				}
				catch (Exception e)
				{
					Console.WriteLine();
					Console.WriteLine(e);
					throw;
				}
				finally
				{
					Console.WriteLine(report.Results());
				}

				Console.ReadKey();
			}
		}
	}
}

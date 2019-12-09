using System;
using Autofac;
using GdprService;

namespace GdprClientConsole
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			Console.WriteLine($"GDPR deletion job started: {DateTime.Now}");

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
				}
				finally
				{
					Console.WriteLine(report.Results());
				}

				Console.WriteLine("Done! Press key to exit.");
				Console.ReadKey();
			}
		}
	}
}

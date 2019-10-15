using System;
using System.Collections.Generic;
using System.Linq;
using GdprService;
using Autofac;

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
				var command = commandFactory.Create(args);
				command.Execute().Wait();
				
				Console.ReadKey();
			}
		}
	}
}

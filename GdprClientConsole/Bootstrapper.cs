using Autofac;

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

			builder.RegisterAssemblyTypes(typeof(GdprService.GdprService).Assembly).AsImplementedInterfaces();

			return builder.Build();
		}
	}
}

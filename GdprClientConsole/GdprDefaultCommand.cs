using System.Threading.Tasks;
using GdprService;

namespace GdprClientConsole
{
	public class GdprDefaultCommand : IGdprCommand
	{
		private readonly string[] args;
		private readonly ILogger consoleLogger;

		public delegate GdprDefaultCommand Factory(string[] args);

		public GdprDefaultCommand(string[] args, ILogger consoleLogger)
		{
			// args = delete -f "\..\..\..\Test Files\Fildrev.csv"
			this.args = args;
			this.consoleLogger = consoleLogger;
		}

		public Task Execute()
		{
			return Task.Run(
				() =>
				{
					this.consoleLogger.Log($"Command '{this.args[0]}' not recognised.");
					this.consoleLogger.Log($"Supplied args: '{string.Join(" ", this.args)}'");
				});
		}
	}
}
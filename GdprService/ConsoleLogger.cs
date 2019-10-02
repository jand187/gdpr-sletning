using System;
using System.Threading.Tasks;

namespace GdprService
{
	public class ConsoleLogger : ILogger
	{
		public async void LogError(string message, Exception exception)
		{
			await Task.Run(
				() => Console.WriteLine(
					$"{message}{Environment.NewLine}--------------------------{Environment.NewLine}{exception}{Environment.NewLine}"));
		}

		public async void Log(string message)
		{
			await Task.Run(() => Console.WriteLine(message));
		}
	}
}

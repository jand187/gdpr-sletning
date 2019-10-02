using System;

namespace GdprService
{
	public class ConsoleLogger : ILogger
	{
		public void LogError(string message, Exception exception)
		{
			Console.WriteLine($"{message}{Environment.NewLine}--------------------------{Environment.NewLine}{exception}{Environment.NewLine}");
		}
	}
}
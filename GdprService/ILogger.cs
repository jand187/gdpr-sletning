using System;

namespace GdprService
{
	public interface ILogger
	{
		void LogError(string message, Exception exception);
		void Log(string message);
	}
}
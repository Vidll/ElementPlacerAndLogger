using System;
using WindowsFormsApp1;

namespace WindowsFormsApp1
{
	public interface ILogger : IDisposable
	{
		void Log(LogLevel level, string message, bool showMessage = false);
	}
}

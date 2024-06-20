using System;
using System.IO;

namespace WindowsFormsApp1
{
	public abstract class BaseLogger : ILogger
	{
		protected string name;
		protected DirectoryInfo path;
		protected long maxSize;

		protected BaseLogger(string path, string name, long maxSize)
		{
			this.path = new DirectoryInfo(path);
			if (!this.path.Exists)
				this.path.Create();

			this.name = name;
			this.maxSize = maxSize;
		}

		public abstract void Log(LogLevel level, string message, bool showMessage = false);

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing) { }
	}

	public enum LogLevel
	{
		Trace,
		Info,
		Debug,
		Warning,
		Error
	}
}
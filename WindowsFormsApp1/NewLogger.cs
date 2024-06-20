using System;
using System.Globalization;
using System.IO.Compression;
using System.IO;
using System.Text;

namespace WindowsFormsApp1
{
	public class NewLogger : BaseLogger
	{
		private StreamWriter log;
		private bool disposed = false;

		public NewLogger(string path, string name = "log.txt", long maxSize = 31457280) // 30 MB
		: base(path, name, maxSize) 
		{ 
			OpenLogFile(); 
		}

		private void OpenLogFile()
		{
			RotateLogFile();

			var logFilePath = Path.Combine(path.FullName, name);
			log = new StreamWriter(logFilePath, true, Encoding.UTF8)
			{
				AutoFlush = true
			};
		}

		private void RotateLogFile()
		{
			var logFilePath = Path.Combine(path.FullName, name);

			if (File.Exists(logFilePath))
			{
				var currentLogSize = new FileInfo(logFilePath).Length;

				if (currentLogSize >= maxSize)
				{
					var archiveName = $"{name}-{DateTime.Now:yyyyMMddHHmmss}.gz";
					var archivePath = Path.Combine(path.FullName, archiveName);

					using (var logFileStream = new FileStream(logFilePath, FileMode.Open, FileAccess.Read))
					using (var archiveStream = new FileStream(archivePath, FileMode.Create))
					using (var gzipStream = new GZipStream(archiveStream, CompressionMode.Compress))
					{
						logFileStream.CopyTo(gzipStream);
					}

					File.Delete(logFilePath);
				}
			}
		}

		public override void Log(LogLevel level, string message, bool showMessage = false)
		{
			var timestamp = DateTime.UtcNow.ToString("dd-MM-yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);
			var logMessage = $"{timestamp} [{level,-7}]: {message}";

			log.WriteLine(logMessage);

			if (showMessage)
				ShowMessage(logMessage);
		}

		private void ShowMessage(string message) { Console.WriteLine(message); }

		protected override void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
					log?.Dispose();

				disposed = true;
			}
			base.Dispose(disposing);
		}
	}
}
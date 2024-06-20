using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
	internal static class Program
	{
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
			
			//Start logger
			/*using (ILogger logger = new NewLogger("logs", "application.log"))
			{
				logger.Log(LogLevel.Info, "Application started.");
				logger.Log(LogLevel.Debug, "Debugging information.");
				logger.Log(LogLevel.Warning, "A warning message.");
				logger.Log(LogLevel.Error, "An error occurred.");

				for (int i = 0; i < 100000; i++)
				{
					logger.Log(LogLevel.Trace, $"Trace message {i}");
				}

				logger.Log(LogLevel.Info, "Application finished.");
			}*/
		}
	}
}

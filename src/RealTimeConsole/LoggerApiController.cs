using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

using static System.Console;

namespace RealTimeConsole
{
	[RoutePrefix("api/logger")]
	public class LoggerApiController : ApiController
	{
		[HttpPost]
		[Route("write")]
		public void WriteLog(Log log)
		{
			if (log.Category == "Error")
			{
				ForegroundColor = ConsoleColor.Red;
			}
			else if (log.Category == "Warning")
			{
				ForegroundColor = ConsoleColor.Yellow;
			}
			else if (log.Category == "Information")
			{
				ForegroundColor = ConsoleColor.White;
			}
			else
			{
				ForegroundColor = ConsoleColor.Gray;
			}

			var message = $"{DateTime.Now:yyMMddHHMMss.fff}|{log.MachineName}|{log.ApplicationName}|{log.Category}|{log.Message} {log.DetailedMessage}";

			if (log.Type == "write"
				|| log.Type == "writeobject")
			{
				Write(message);
			}
			else
			{
				WriteLine(message);
			}
		}

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeTraceListener
{
	public class Log
	{
		public string Message { get; set; }
		public string DetailedMessage { get; set; }
		public string Category { get; set; }
		public string Type { get; set; }
		public string ApplicationName { get; set; }
		public string MachineName { get; set; }
	}
}

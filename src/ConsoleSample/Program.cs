using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Diagnostics.Trace;

namespace ConsoleSample
{
	class Program
	{
		static void Main(string[] args)
		{
			while(true)
			{
				System.Diagnostics.Debug.Write("write");
				System.Diagnostics.Debug.WriteLine("writeline");

				Write("write");
				WriteLine("writeline");

				TraceError("traceerror");
				TraceError("traceerror {0}", "test");

				TraceInformation("traceinformation");
				TraceInformation("traceinformation {0}", "test");

				TraceWarning("tracewarning");
				TraceWarning("tracewarning {0}", "test");

				Console.ReadKey();
				break;
			}
		}
	}
}

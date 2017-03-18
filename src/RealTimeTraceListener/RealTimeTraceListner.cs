using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeTraceListener
{
	public class RealTimeTraceListner : System.Diagnostics.TraceListener
	{
		protected override string[] GetSupportedAttributes()
		{
			return new[] { "url", "Url" };
		}

		public override void Write(string message)
		{
			Enqueue(new Log() {
				Message = message,
				Type = "write"
			});
		}

		public override void WriteLine(string message)
		{
			Enqueue(new Log() {
				Message = message,
				Type = "writeline"
			});
		}

		public override void Fail(string message)
		{
			Enqueue(new Log() {
				Message = message,
				Type = "fail"
			});
		}

		public override void Fail(string message, string detailMessage)
		{
			Enqueue(new Log() {
				Message = message,
				Type = "fail",
				DetailedMessage = detailMessage
			});
		}

		public override void Write(object o)
		{
			Enqueue(new Log() {
				Message = o.ToString(),
				Type = "writeobject",
			});
		}

		public override void Write(string message, string category)
		{
			Enqueue(new Log() {
				Message = message,
				Type = "writeobject",
				Category = category
			});
		}

		public override void WriteLine(object o)
		{
			Enqueue(new Log() {
				Message = o.ToString(),
				Type = "writelineobject",
			});
		}

		public override void Write(object o, string category)
		{
			Enqueue(new Log() {
				Message = o.ToString(),
				Type = "writeobject",
				Category = category
			});
		}

		public override void WriteLine(object o, string category)
		{
			Enqueue(new Log() {
				Message = o.ToString(),
				Type = "writelineobject",
				Category = category
			});
		}

		public override void WriteLine(string message, string category)
		{
			Enqueue(new Log() {
				Message = message,
				Type = "writelineobject",
				Category = category
			});
		}

		public void Enqueue(Log log)
		{
			AsyncLogger.Current.BaseAddress = this.Attributes["url"];
			AsyncLogger.Current.LogMessage(log);
		}

		protected override void Dispose(bool disposing)
		{
			AsyncLogger.Current.Terminate();
			base.Dispose(disposing);
		}
	}
}

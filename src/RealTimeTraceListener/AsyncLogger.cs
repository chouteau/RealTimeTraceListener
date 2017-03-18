using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;

namespace RealTimeTraceListener
{
	internal class AsyncLogger : IDisposable
	{
		private static Lazy<AsyncLogger> m_LazyCurrent = new Lazy<AsyncLogger>(() =>
		{
			return new AsyncLogger();
		}, true);

		private Queue<Action> m_Queue = new Queue<Action>();
		private ManualResetEvent m_NewLog = new ManualResetEvent(false);
		private ManualResetEvent m_Terminate = new ManualResetEvent(false);
		private bool m_Terminated = false;
		private Thread m_LoggingThread;

		private Log m_LogFormat = null;

		private AsyncLogger()
		{
			m_LoggingThread = new Thread(new ThreadStart(ProcessQueue));
			m_LoggingThread.Name = "RealTimeLoggerThread";
			m_LoggingThread.IsBackground = true;
			m_LoggingThread.Start();
		}

		public static AsyncLogger Current
		{
			get
			{
				return m_LazyCurrent.Value;
			}
		}

		public string BaseAddress { get; set; }

		void ProcessQueue()
		{
			while (!m_Terminated)
			{
				var waitHandles = new WaitHandle[] { m_Terminate, m_NewLog };
				int result = ManualResetEvent.WaitAny(waitHandles, 60 * 1000, true);
				if (result == 0)
				{
					m_Terminated = true;
					break;
				}
				m_NewLog.Reset();

				if (m_Queue.Count == 0)
				{
					continue;
				}

				Queue<Action> queueCopy;
				lock (m_Queue)
				{
					queueCopy = new Queue<Action>(m_Queue);
					m_Queue.Clear();
				}

				if (m_Terminated)
				{
					break;
				}

				foreach (var log in queueCopy)
				{
					try
					{
						log();
					}
					catch { }
				}
			}
		}

		public void LogMessage(Log log)
		{
			lock (m_Queue)
			{
				m_Queue.Enqueue(() => AsyncLogMessage(log));
			}
			m_NewLog.Set();
		}

		private void AsyncLogMessage(Log log)
		{
			var message = log.Message.Split(' ');
			if (m_LogFormat == null)
			{
				m_LogFormat = new Log();
				if (message.Count() == 1)
				{
					m_LogFormat.ApplicationName = System.AppDomain.CurrentDomain.FriendlyName;
					m_LogFormat.Category = "Debug";
					m_LogFormat.Message = log.Message;
					SendMessage();
				}
				else if (message.Count() > 1)
				{
					m_LogFormat.ApplicationName = message[0];
					m_LogFormat.Category = message[1].TrimEnd(':');
				}
			}
			else
			{
				m_LogFormat.Message = log.Message;
				SendMessage();
			}
		}

		public void SendMessage()
		{
			m_LogFormat.MachineName = System.Environment.MachineName;
			using (var httpClient = new HttpClient())
			{
				httpClient.BaseAddress = new Uri(BaseAddress);
				httpClient.PostAsJsonAsync($"api/logger/write", m_LogFormat).Wait(1 * 1000);
			}
			m_LogFormat = null;
		}

		public void Terminate()
		{
			m_Terminated = true;
			if (m_Terminate != null)
			{
				m_Terminate.Set();
			}
			if (m_LoggingThread != null
				&& !m_LoggingThread.Join(TimeSpan.FromSeconds(5)))
			{
				m_LoggingThread.Abort();
			}
		}

		public void Dispose()
		{
			if (m_Terminate != null)
			{
				m_Terminate.Set();
			}
			if (m_LoggingThread != null)
			{
				m_LoggingThread.Join();
			}
		}
	}
}

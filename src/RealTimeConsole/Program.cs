using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Owin;
using System.Web.Http;

namespace RealTimeConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			var url = System.Configuration.ConfigurationManager.AppSettings["url"] ?? "http://+:12345";
			Microsoft.Owin.Hosting.WebApp.Start(url, (appBuilder) =>
			{
		        var config = new HttpConfiguration();
				config.MapHttpAttributeRoutes();
				appBuilder.UseWebApi(config);
			});

			Console.ReadKey();
		}
	}
}

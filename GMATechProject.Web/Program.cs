namespace GMATechProject.Web
{
	#region Using Directives
	
    using System;
    using System.Configuration;
	using System.Linq;
    using System.Threading;
    
    using Nancy.Hosting.Self;
	using Nancy.TinyIoc;
	using Nancy.ViewEngines.Razor;

	using CommandLine;

	#endregion
	
    class Program
    {
        static void Main (string[] args)
		{
			var parsedArgs = Parser.Default.ParseArguments<Options> (args);

			if (!parsedArgs.Errors.Any () && parsedArgs.Value != null) {
				var options = parsedArgs.Value;

				var domain = options.Domain ?? ConfigurationManager.AppSettings.Get ("Nancy.Host") ?? "127.0.0.1";
				var port = options.Port ?? ConfigurationManager.AppSettings.Get ("Nancy.Port") ?? "9999";
        	
				var uri = new Uri ("http://" + domain + ":" + port);
        	    
				var configuration = new HostConfiguration();
				configuration.UnhandledExceptionCallback = HandleException;

				// initialize an instance of NancyHost (found in the Nancy.Hosting.Self package)
				var nancyHost = new NancyHost (new Bootstrapper (), uri);


				nancyHost.Start (); // start hosting
			
				while (true) {
					Thread.Sleep (10000000);
				}

				nancyHost.Stop (); // stop hosting
			}
		}

		private static void HandleException(Exception exception){
			var m = exception.Message;
			var s = exception.StackTrace;
		}
    }

	class Options
	{
		[Option('p', "port", Required=false, HelpText = "Http port to use. Default is the one specified in the app.config file or 9999 if not defined.")]
		public string Port { get; set; }

		[Option('d', "domain", Required=false, HelpText = "Http domain to use. Default is the one specified in the app.config file or 127.0.0.1 if not defined.")]
		public string Domain { get; set; }
	}
}

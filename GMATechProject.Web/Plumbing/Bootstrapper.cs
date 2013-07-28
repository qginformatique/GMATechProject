namespace GMATechProject.Web
{
	#region Using Directives

	using Nancy;
	using Nancy.Authentication.Forms;
	using Nancy.Bootstrapper;
    using Nancy.Conventions;
	using Nancy.Diagnostics;
	using Nancy.TinyIoc;

	using GMATechProject.Domain;
	using GMATechProject.Web.Plumbing.Serialization;
    
	#endregion
    
	/// <summary>
	/// Class used by Nancy to configure itself.
	/// </summary>
    public class Bootstrapper : DefaultNancyBootstrapper
    {
    	/// <summary>
    	/// Overrides the provider for rootpath. This allows to run the application with self hosting and without nginx, 
    	/// for developpement purpose, without having to copy all resources (views, static files) into the bin directory.
    	/// </summary>
		protected override IRootPathProvider RootPathProvider 
		{
			get { return new RootPathProvider(); }
		}
		
		/// <summary>
		/// Defines the conventions used to find static files and views
		/// </summary>
		/// <param name="conventions"></param>
		protected override void ConfigureConventions (NancyConventions conventions)
		{
			// We use all base conventions
			base.ConfigureConventions (conventions);

			// Add conventions for our static resources
			conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Resources", @"Resources"));
			conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Scripts", @"Scripts"));
			conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Upload", @"Upload"));
		}
		
		/// <summary>
		/// Overrides the ApplicationStartup to ensure all our assemblies are loaded
		/// </summary>
		protected override void ApplicationStartup (TinyIoCContainer container, IPipelines pipelines)
		{
			//StaticConfiguration.EnableRequestTracing = true;

			pipelines.BeforeRequest += context => {
				context.Trace.TraceLog.WriteLog(s => s.AppendLine("Request begins for " + context.Request.Path));

				return null;
			};

			// Enable expiration headers check and addition
			pipelines.AfterRequest += context => {
				context.CheckForIfNonMatch();
				context.CheckForIfModifiedSince();

				context.Trace.TraceLog.WriteLog(s => s.AppendLine("Request ends for " + context.Request.Path));
				context.Response.ContentType += "; charset=utf-8";
			};

			base.ApplicationStartup (container, pipelines);
			
			// Request that all GMATechProject assemblies be loaded
			AppDomainAssemblyTypeScanner.LoadAssemblies (@"GMATechProject*.dll");

			// Register explicitly some services
			container.Register<IPasswordUtility, PasswordUtility>();
			container.Register<IUserMapper, UserMapper>();
			container.Register<IApplication, Application>();

			// Get the application instance
			var application = container.Resolve<IApplication>();

			// Ask it to initialize any starting data if needed
			application.InitializeApplicationData();

			// Configure the FormsAuthentication
			var formAuthentication = new FormsAuthenticationConfiguration(){
				RedirectUrl = "~/admin/authentication", 
				UserMapper = container.Resolve<IUserMapper>(), 
				DisableRedirect = true
			};

			// Enable it
			FormsAuthentication.Enable(pipelines, formAuthentication);

			StaticConfiguration.DisableErrorTraces = false;
		}

		/// <summary>
		/// Overrides the InternalConfiguration to specify our custom services
		/// </summary>
		protected override NancyInternalConfiguration InternalConfiguration {
			get 
			{
				return NancyInternalConfiguration
					.WithOverrides(
						configuration => {
							// Clear all serializers
							configuration.Serializers.Clear();
							// Register our custom JSON serializer which uses NewtonSoft JSON.NET
							configuration.Serializers.Insert(0, typeof(JsonNetSerializer));
	               	});
			}
		}

		protected override DiagnosticsConfiguration DiagnosticsConfiguration
		{
			get { return new DiagnosticsConfiguration { Password = @"FibreSub2414"}; }
		}
	}
}
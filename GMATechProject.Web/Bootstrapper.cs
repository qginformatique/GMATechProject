using System.Collections.Generic;
using System.Reflection;

namespace ABC.Web
{
    using Nancy;
    using Nancy.Conventions;

    public class Bootstrapper : DefaultNancyBootstrapper
    {
		protected override void ConfigureApplicationContainer(TinyIoC.TinyIoCContainer container)
		{		
			var assemblies = new List<Assembly>();
			assemblies.Add(Assembly.GetExecutingAssembly());
			assemblies.Add(Assembly.GetAssembly(typeof(ABC.Domain.Security.Roles)));
			
			container.AutoRegister(assemblies);
			base.ConfigureApplicationContainer(container);
		}
		
		protected override void ConfigureConventions (NancyConventions conventions)
		{
			base.ConfigureConventions (conventions);

			conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Resources", @"Resources"));
			conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Scripts", @"Scripts"));
			conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Upload", @"Upload"));
		}
    }
}
using React;
using WebApplication.Implementation.Infrastructure.Extensions;

namespace WebApplication
{
	public static class ReactConfig
	{
		public static void Configure()
		{
			// ES6 features are enabled by default. Uncomment the below line to disable them.
			// See http://reactjs.net/guides/es6.html for more information.
            ReactSiteConfiguration.Configuration.SetUseHarmony(true);

            // Uncomment the below line if you are using Flow
			// See http://reactjs.net/guides/flow.html for more information.
            // ReactSiteConfiguration.Configuration.SetStripTypes(true);

			// If you want to use server-side rendering of React components, 
			// add all the necessary JavaScript files here. This includes 
			// your components as well as all of their dependencies.
			// See http://reactjs.net/ for more information. Example:

            ReactSiteConfiguration.Configuration.AddScripts("~/Content/Scripts/Shared", "*.js");
            ReactSiteConfiguration.Configuration.AddScripts("~/Content/Scripts", "*.jsx", true);
		}
	}
}
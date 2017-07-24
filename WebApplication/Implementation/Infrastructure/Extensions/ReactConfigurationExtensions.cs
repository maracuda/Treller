using System;
using System.IO;
using System.Linq;
using React;

namespace WebApplication.Implementation.Infrastructure.Extensions
{
    public static class ReactConfigurationExtensions
    {
        public static void AddScripts(this IReactSiteConfiguration configuration, string directoryPath, string searchPattern, bool searchSubdirectories = false)
        {
            var fullPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directoryPath.Replace("~/", string.Empty)));
            var files = Directory.GetFiles(fullPath, searchPattern, searchSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                .Select(x => x.Replace(AppDomain.CurrentDomain.BaseDirectory, "~/").Replace("\\", "/"));

            foreach (var file in files)
            {
                configuration.AddScript(file);
            }
        }
    }
}
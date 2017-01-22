using System;
using System.IO;
using SKBKontur.Treller.Storage;

namespace SKBKontur.Treller.Tests.Tests.IntegrationTests.Configuration
{
    public class TestEnvironment : IEnvironment
    {
        public string BasePath
        {
            get
            {
                var pathToTests = AppDomain.CurrentDomain.BaseDirectory;
                var rootPath = Path.GetPathRoot(pathToTests);
                while (!pathToTests.EndsWith("Tests\\") && !string.Equals(rootPath, pathToTests))
                {
                    pathToTests = Path.GetFullPath(Path.Combine(pathToTests, "..\\"));
                }

                if (string.Equals(rootPath, pathToTests))
                    throw new Exception($"Fail to find path to Tests proj from {AppDomain.CurrentDomain.BaseDirectory}.");

                return pathToTests;
            }
        }
    }
}
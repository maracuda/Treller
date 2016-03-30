using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Storages
{
    public class CachedFileStorage : ICachedFileStorage
    {
        private readonly static string FileNamePattern = Path.Combine(HttpRuntime.AppDomainAppPath, "Store_{0}.json");
        private readonly ConcurrentDictionary<string, dynamic> cache = new ConcurrentDictionary<string, dynamic>();

        public T Find<T>(string storeName)
        {
            object result;
            if (cache.TryGetValue(storeName, out result))
            {
                return (T)result;
            }

            var fileName = GetFileName(storeName);
            if (File.Exists(fileName))
            {
                result = JsonConvert.DeserializeObject(File.ReadAllText(fileName, Encoding.UTF8), typeof(T));
                cache.TryAdd(storeName, result);
                return (T)result;
            }

            return default(T);
        }

        public void Write<T>(string storeName, T serializedEntity)
        {
            dynamic stored;
            cache.TryRemove(storeName, out stored);
            cache.TryAdd(storeName, serializedEntity);

            var contents = JsonConvert.SerializeObject(serializedEntity);
            var stopwatch = Stopwatch.StartNew();

            while (!RecurcyWrite(storeName, contents))
            {
                if (stopwatch.Elapsed.TotalSeconds > 5)
                {
                    stopwatch.Stop();
                    throw new Exception(string.Format("Can't write file {0} for 5 seconds", storeName));
                }
            }
        }

        private static bool RecurcyWrite(string fileStoreName, string fileBody)
        {
            try
            {
                File.WriteAllText(GetFileName(fileStoreName), fileBody);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static string GetFileName(string storeName)
        {
            return string.Format(FileNamePattern, storeName);
        }
    }
}
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Storages
{
    public class CachedFileStorage : ICachedFileStorage
    {
        private readonly static string FileNamePattern = Path.Combine(HttpRuntime.AppDomainAppPath, "Store_{0}.json");
        private readonly ConcurrentDictionary<string, dynamic> _cache = new ConcurrentDictionary<string, dynamic>();

        public T Find<T>(string storeName)
        {
            object result;
            if (_cache.TryGetValue(storeName, out result))
            {
                return (T)result;
            }

            var fileName = GetFileName(storeName);
            if (File.Exists(fileName))
            {
                result = JsonConvert.DeserializeObject(File.ReadAllText(fileName, Encoding.UTF8), typeof(T));
                _cache.TryAdd(storeName, result);
                return (T)result;
            }

            return default(T);
        }

        public void Write<T>(string storeName, T serializedEntity)
        {
            dynamic stored;
            _cache.TryRemove(storeName, out stored);
            _cache.TryAdd(storeName, serializedEntity);

            var contents = JsonConvert.SerializeObject(serializedEntity);
            File.WriteAllText(GetFileName(storeName), contents);
        }

        private static string GetFileName(string storeName)
        {
            return string.Format(FileNamePattern, storeName);
        }
    }
}
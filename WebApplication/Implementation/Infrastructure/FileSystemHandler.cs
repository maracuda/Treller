using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web;
using SKBKontur.Infrastructure.Common;
using SKBKontur.Treller.Serialization;

namespace SKBKontur.Treller.WebApplication.Implementation.Infrastructure
{
    public class FileSystemHandler : IFileSystemHandler
    {
        private static readonly Encoding defaultEncoding = Encoding.UTF8;
        private readonly IJsonSerializer jsonSerializer;
        private readonly string rootPath;

        public FileSystemHandler(IJsonSerializer jsonSerializer)
        {
            this.jsonSerializer = jsonSerializer;
            string basePath;
            try
            {
                basePath = HttpRuntime.AppDomainAppPath;
                rootPath = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(basePath)), "TrellerData");
            }
            catch (Exception)
            {
                basePath = AppDomain.CurrentDomain.BaseDirectory;
                rootPath = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(basePath))), "TrellerData");
            }
        }

        public TEntity FindSafeInJsonUtf8File<TEntity>(string fileName)
        {
            var result = FindSafeInJsonUtf8File(fileName, typeof (TEntity));

            return result is TEntity
                    ? (TEntity) result
                    : default(TEntity);
        }

        public object FindSafeInJsonUtf8File(string fileName, Type type)
        {
            var fileText = ReadUTF8(fileName);
            return jsonSerializer.Deserialize(type, fileText);
        }

        public string ReadUTF8(string fileName)
        {
            var fullName = GetFullPath(fileName);

            return File.Exists(fullName) 
                    ? File.ReadAllText(fullName, defaultEncoding) 
                    : string.Empty;
        }

        public void WriteInJsonUtf8File<TEntity>(string fileName, TEntity entity)
        {
            var path = GetFullPath(fileName);
            var json = jsonSerializer.Serialize(entity);
            FaultTolerantWrite(path, json, defaultEncoding);
        }

        public void WriteUTF8(string fileName, string str)
        {
            var path = GetFullPath(fileName);
            FaultTolerantWrite(path, str, defaultEncoding);
        }

        private static void FaultTolerantWrite(string path, string str, Encoding encoding)
        {
            var stopwatch = Stopwatch.StartNew();
            Exception exception;
            var attempt = 0;
            while (!RecurcyWrite(path, str, encoding, ref attempt, out exception))
            {
                if (stopwatch.Elapsed.TotalSeconds > 5 && attempt > 5)
                {
                    stopwatch.Stop();
                    throw new Exception($"Can't write file {path} for 5 seconds", exception);
                }
            }
            stopwatch.Stop();
        }

        private static bool RecurcyWrite(string path, string str, Encoding encoding, ref int attempt, out Exception exception)
        {
            try
            {
                File.WriteAllText(path, str, encoding);
                exception = null;
                return true;
            }
            catch (Exception ex)
            {
                exception = ex;
                return false;
            }
            finally
            {
                attempt++;
            }
        }

        public void Delete(string fileName)
        {
            try
            {
                File.Delete(GetFullPath(fileName));
            }
            catch (Exception e)
            {
                throw new Exception($"Fail to delete file {fileName}.", e);
            }
        }

        private string GetFullPath(string fileName)
        {
            return Path.Combine(rootPath, fileName);
        }
    }
}
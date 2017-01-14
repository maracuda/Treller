using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SKBKontur.Treller.Storage.FileStorage
{
    public class FileSystemHandler : IFileSystemHandler
    {
        private static readonly Encoding defaultEncoding = Encoding.UTF8;
        private const string dataDirName = "TrellerData";
        private readonly string rootPath;

        public FileSystemHandler(IEnvironment environment)
        {
            try
            {
                rootPath = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(environment.BasePath)), dataDirName);
            }
            catch (Exception)
            {
                rootPath = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory))), dataDirName);
            }
        }

        public string ReadUTF8(string fileName)
        {
            var path = GetFullPath(fileName);
            return File.Exists(path) 
                    ? File.ReadAllText(path, defaultEncoding) 
                    : string.Empty;
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

        public string GetFullPath(string fileName)
        {
            return Path.Combine(rootPath, fileName);
        }

        public bool Contains(string fileName)
        {
            return File.Exists(GetFullPath(fileName));
        }
    }
}
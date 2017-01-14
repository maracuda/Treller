
namespace SKBKontur.Treller.Storage.FileStorage
{
    public interface IFileSystemHandler
    {
        void Delete(string fileName);
        void WriteUTF8(string fileName, string str);
        string ReadUTF8(string fileName);
        bool Contains(string fileName);
        string GetFullPath(string fileName);
    }
}
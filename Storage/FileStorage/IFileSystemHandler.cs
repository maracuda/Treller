namespace SKBKontur.Treller.Storage.FileStorage
{
    public interface IFileSystemHandler
    {
        TEntity FindSafeInJsonUtf8File<TEntity>(string fileName);
        void WriteInJsonUtf8File<TEntity>(string fileName, TEntity entity);
        void Delete(string fileName);
        void WriteUTF8(string fileName, string str);
        string ReadUTF8(string fileName);
    }
}
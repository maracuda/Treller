using System;

namespace SKBKontur.Infrastructure.Common
{
    public interface IFileSystemHandler
    {
        TEntity FindSafeInJsonUtf8File<TEntity>(string fileName);
        object FindSafeInJsonUtf8File(string fileName, Type type);
        void WriteInJsonUtf8File<TEntity>(string fileName, TEntity entity);
    }
}
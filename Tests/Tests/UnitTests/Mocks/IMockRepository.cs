using System;

namespace SKBKontur.Treller.Tests.Tests.UnitTests.Mocks
{
    public interface IMockRepository : IDisposable
    {
        T Create<T>();
        IDisposable Record();
        void Verify();
    }
}
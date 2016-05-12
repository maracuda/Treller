using System;
using Rhino.Mocks;

namespace SKBKontur.Treller.Tests.MockWrappers
{
    public class MyMock : IDisposable
    {
        private readonly MockRepository _mockRepository;

        public MyMock()
        {
            _mockRepository = new MockRepository();
        }

        public void Dispose()
        {
            _mockRepository.Record().Dispose();
        }

        public T Create<T>()
        {
            return _mockRepository.StrictMock<T>();
        }

        public IDisposable Record()
        {
            return _mockRepository.Record();
        }

        public IDisposable RecordOrdered()
        {
            return _mockRepository.Ordered();
        }

        public void RunAll()
        {
            _mockRepository.VerifyAll();
        }
    }
}
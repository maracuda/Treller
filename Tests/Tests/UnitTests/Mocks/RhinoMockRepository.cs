using System;
using Rhino.Mocks;

namespace Tests.Tests.UnitTests.Mocks
{
    public class RhinoMockRepository : IMockRepository
    {
        private readonly MockRepository mockRepository;

        public RhinoMockRepository()
        {
            mockRepository = new MockRepository();
        }

        public void Dispose()
        {
            mockRepository.Record().Dispose();
        }

        public T Create<T>()
        {
            return mockRepository.StrictMock<T>();
        }

        public IDisposable Record()
        {
            return mockRepository.Record();
        }

        public void Verify()
        {
            mockRepository.VerifyAll();
        }
    }
}
using SKBKontur.Treller.Tests.Tests.UnitTests.Mocks;

namespace SKBKontur.Treller.Tests.Tests.UnitTests
{
    public abstract class UnitTest
    {
        protected readonly IMockRepository mockRepository;

        protected UnitTest()
        {
            mockRepository = new RhinoMockRepository();
        }

        ~UnitTest()
        {
            mockRepository.Dispose();
            mockRepository.Verify();
        }
    }
}
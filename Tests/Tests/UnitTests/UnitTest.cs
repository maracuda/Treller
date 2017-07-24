using Tests.Tests.UnitTests.Mocks;

namespace Tests.Tests.UnitTests
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
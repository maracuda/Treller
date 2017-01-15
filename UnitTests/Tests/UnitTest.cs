using SKBKontur.Treller.UnitTests.Mocks;

namespace SKBKontur.Treller.UnitTests.Tests
{
    public abstract class UnitTest
    {
        protected readonly MyMock mock;

        protected UnitTest()
        {
            mock = new MyMock();
        }

        ~UnitTest()
        {
            mock.Dispose();
            mock.RunAll();
        }
    }
}
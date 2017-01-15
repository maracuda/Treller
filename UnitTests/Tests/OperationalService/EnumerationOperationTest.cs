using System;
using System.Threading;
using System.Threading.Tasks;
using Rhino.Mocks;
using SKBKontur.Treller.Storage;
using SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Operations;
using Xunit;

namespace SKBKontur.Treller.UnitTests.Tests.OperationalService
{
    public class EnumerationOperationTest : UnitTest
    {
        private readonly IKeyValueStorage keyValueStorage;

        public EnumerationOperationTest()
        {
            keyValueStorage = mock.Create<IKeyValueStorage>();
        }

        [Fact]
        public void TestRunEnumerationOperation()
        {
            var oldTimestamp = 1234L;
            var newTimestamp = 3456L;

            var i = 0;
            var enumeration = new Func<long, long>((old) =>
            {
                Assert.Equal(oldTimestamp, old);
                i++;
                return newTimestamp;
            });

            using (mock.Record())
            {
                keyValueStorage.Expect(f => f.Find<long>("zzzTimestamp.json")).Return(oldTimestamp);
                keyValueStorage.Expect(f => f.Write("zzzTimestamp.json", newTimestamp));
            }

            var operation = new EnumerationOperation(keyValueStorage, "zzz", enumeration, () => 1L);
            var operationResult = operation.Run();
            Assert.False(operationResult.HasValue);
            Assert.Equal(i, 1);
        }

        [Fact]
        public void TestRunEnumerationOperationWithException()
        {
            var oldTimestamp = 1234L;
            var ex = new Exception();

            var i = 0;
            var enumeration = new Func<long, long>((old) =>
            {
                Assert.Equal(oldTimestamp, old);
                throw ex;

            });

            using (mock.Record())
            {
                keyValueStorage.Expect(f => f.Find<long>("zzzTimestamp.json")).Return(oldTimestamp);
            }

            var operation = new EnumerationOperation(keyValueStorage, "zzz", enumeration, () => 1L);
            var operationResult = operation.Run();
            Assert.True(operationResult.HasValue);
            Assert.Equal(ex, operationResult.Value);
        }

        [Fact]
        public void TestRunEnumerationOperationFirstTime()
        {
            var newTimestamp = 3456L;

            var i = 0;
            var enumeration = new Func<long, long>((old) =>
            {
                Assert.Equal(1L, old);
                i++;
                return newTimestamp;
            });

            using (mock.Record())
            {
                keyValueStorage.Expect(f => f.Find<long>("zzzTimestamp.json")).Return(0);
                keyValueStorage.Expect(f => f.Write("zzzTimestamp.json", newTimestamp));
            }

            var operation = new EnumerationOperation(keyValueStorage, "zzz", enumeration, () => 1L);
            var operationResult = operation.Run();
            Assert.False(operationResult.HasValue);
            Assert.Equal(i, 1);
        }

        [Fact]
        public void TestRunSimpleOperationTwiceAtOnTime()
        {
            const int timeoutMs = 500;
            var newTimestamp = 3456L;

            var i = 0;
            var enumeration = new Func<long, long>((old) =>
            {
                Assert.Equal(1L, old);
                i++;
                return newTimestamp;
            });

            using (mock.Record())
            {
                keyValueStorage.Expect(f => f.Find<long>("zzzTimestamp.json")).Return(0);
                keyValueStorage.Expect(f => f.Write("zzzTimestamp.json", newTimestamp));
            }

            var operation = new EnumerationOperation(keyValueStorage, "zzz", enumeration, () => 1L);

            Task.Run(() =>
            {
                Assert.Equal(OperationState.Idle, operation.State);
                var firstRunResult = operation.Run();
                Assert.False(firstRunResult.HasValue);
            });

            Task.Run(() =>
            {
                Assert.Equal(OperationState.Running, operation.State);
                var secondRunResult = operation.Run();
                Assert.False(secondRunResult.HasValue);
            });
            Thread.Sleep(timeoutMs + 50);
            Assert.Equal(OperationState.Idle, operation.State);
            Assert.Equal(1, i);
        }
    }
}
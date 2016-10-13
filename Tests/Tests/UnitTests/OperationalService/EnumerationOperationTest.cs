using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Storages;
using SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Operations;

namespace SKBKontur.Treller.Tests.Tests.UnitTests.OperationalService
{
    public class EnumerationOperationTest : UnitTest
    {
        private ICachedFileStorage cachedFileStorage;

        public override void SetUp()
        {
            base.SetUp();

            cachedFileStorage = mock.Create<ICachedFileStorage>();
        }

        [Test]
        public void TestRunEnumerationOperation()
        {
            var oldTimestamp = 1234L;
            var newTimestamp = 3456L;

            var i = 0;
            var enumeration = new Func<long, long>((old) =>
            {
                Assert.AreEqual(oldTimestamp, old);
                i++;
                return newTimestamp;
            });

            using (mock.Record())
            {
                cachedFileStorage.Expect(f => f.Find<long>("zzzTimestamp.json")).Return(oldTimestamp);
                cachedFileStorage.Expect(f => f.Write("zzzTimestamp.json", newTimestamp));
            }

            var operation = new EnumerationOperation(cachedFileStorage, "zzz", enumeration, () => 1L);
            var operationResult = operation.Run();
            Assert.IsFalse(operationResult.HasValue);
            Assert.AreEqual(i, 1);
        }

        [Test]
        public void TestRunEnumerationOperationWithException()
        {
            var oldTimestamp = 1234L;
            var ex = new Exception();

            var i = 0;
            var enumeration = new Func<long, long>((old) =>
            {
                Assert.AreEqual(oldTimestamp, old);
                throw ex;

            });

            using (mock.Record())
            {
                cachedFileStorage.Expect(f => f.Find<long>("zzzTimestamp.json")).Return(oldTimestamp);
            }

            var operation = new EnumerationOperation(cachedFileStorage, "zzz", enumeration, () => 1L);
            var operationResult = operation.Run();
            Assert.IsTrue(operationResult.HasValue);
            Assert.AreEqual(ex, operationResult.Value);
        }

        [Test]
        public void TestRunEnumerationOperationFirstTime()
        {
            var newTimestamp = 3456L;

            var i = 0;
            var enumeration = new Func<long, long>((old) =>
            {
                Assert.AreEqual(1L, old);
                i++;
                return newTimestamp;
            });

            using (mock.Record())
            {
                cachedFileStorage.Expect(f => f.Find<long>("zzzTimestamp.json")).Return(0);
                cachedFileStorage.Expect(f => f.Write("zzzTimestamp.json", newTimestamp));
            }

            var operation = new EnumerationOperation(cachedFileStorage, "zzz", enumeration, () => 1L);
            var operationResult = operation.Run();
            Assert.IsFalse(operationResult.HasValue);
            Assert.AreEqual(i, 1);
        }

        [Test]
        public void TestRunSimpleOperationTwiceAtOnTime()
        {
            const int timeoutMs = 500;
            var newTimestamp = 3456L;

            var i = 0;
            var enumeration = new Func<long, long>((old) =>
            {
                Assert.AreEqual(1L, old);
                i++;
                return newTimestamp;
            });

            using (mock.Record())
            {
                cachedFileStorage.Expect(f => f.Find<long>("zzzTimestamp.json")).Return(0);
                cachedFileStorage.Expect(f => f.Write("zzzTimestamp.json", newTimestamp));
            }

            var operation = new EnumerationOperation(cachedFileStorage, "zzz", enumeration, () => 1L);

            Task.Run(() =>
            {
                Assert.AreEqual(OperationState.Idle, operation.State);
                var firstRunResult = operation.Run();
                Assert.IsFalse(firstRunResult.HasValue);
            });

            Task.Run(() =>
            {
                Assert.AreEqual(OperationState.Running, operation.State);
                var secondRunResult = operation.Run();
                Assert.IsFalse(secondRunResult.HasValue);
            });
            Thread.Sleep(timeoutMs + 50);
            Assert.AreEqual(OperationState.Idle, operation.State);
            Assert.AreEqual(1, i);
        }
    }
}
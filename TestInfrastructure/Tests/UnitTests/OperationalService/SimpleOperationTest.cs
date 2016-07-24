using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Operations;

namespace SKBKontur.Treller.Tests.Tests.UnitTests.OperationalService
{
    public class SimpleOperationTest : UnitTest
    {
        [Test]
        public void TestRunSimpleOperation()
        {
            var i = 0;
            var operation = new SimpleOperation("zzz", () => { i++; });
            var operationResult = operation.Run();
            Assert.IsFalse(operationResult.HasValue);
            Assert.AreEqual(1, i);
        }

        [Test]
        public void TestRunSimpleOperationWithException()
        {
            var exception = new Exception();
            var operation = new SimpleOperation("zzz", () => { throw exception; });
            var operationResult = operation.Run();
            Assert.IsTrue(operationResult.HasValue);
            Assert.AreEqual(exception, operationResult.Value);
        }

        [Test]
        public void TestRunSimpleOperationTwiceAtOnTime()
        {
            var i = 0;
            const int timeoutMs = 500;
            var operation = new SimpleOperation("zzz", () => { i++; Thread.Sleep(timeoutMs); });

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
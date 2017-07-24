using System;
using System.Threading;
using System.Threading.Tasks;
using OperationalService.Operations;
using Xunit;

namespace Tests.Tests.UnitTests.OperationalService
{
    public class SimpleOperationTest : UnitTest
    {
        [Fact]
        public void TestRunSimpleOperation()
        {
            var i = 0;
            var operation = new SimpleOperation("zzz", () => { i++; });
            var operationResult = operation.Run();
            Assert.False(operationResult.HasValue);
            Assert.Equal(1, i);
        }

        [Fact]
        public void TestRunSimpleOperationWithException()
        {
            var exception = new Exception();
            var operation = new SimpleOperation("zzz", () => { throw exception; });
            var operationResult = operation.Run();
            Assert.True(operationResult.HasValue);
            Assert.Equal(exception, operationResult.Value);
        }

        [Fact]
        public void TestRunSimpleOperationTwiceAtOnTime()
        {
            var i = 0;
            const int timeoutMs = 500;
            var operation = new SimpleOperation("zzz", () => { i++; Thread.Sleep(timeoutMs); });

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